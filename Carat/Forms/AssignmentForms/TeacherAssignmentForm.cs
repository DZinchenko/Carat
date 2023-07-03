using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Carat.Interfaces;
using Carat.Data.Repositories;
using Carat.Data.Entities;
using Carat.EF.Repositories;

namespace Carat
{
    public partial class TeacherAssignmentForm : Form, IDataUserForm
    {
        public class ComboBoxCustomItem
        {
            public string Name;
            public double Min;
            public double Current;
            public double Max;

            public ComboBoxCustomItem(string teacherName, double min, double current, double max)
            {
                Name = teacherName;
                Min = min;
                Current = current;
                Max = max;
            }

            public Color GetColor()
            {
                Color result = Color.LightGreen;

                if (Current > Max)
                {
                    result = Color.IndianRed;
                }
                else if (Current < Min)
                {
                    result = Color.White;
                }

                return result;
            }

            public override string ToString()
            {
                return Name
                        + " (min="
                        + Min.ToString("F0")
                        + " - є: "
                        + Current.ToString("F0")
                        + " - max="
                        + Max.ToString("F0") + ")";
            }
        }

        private MainForm m_parentForm = null;
        private SelectGroups m_selectGroupsForm = null;
        private ISubjectRepository m_subjectRepository = null;
        private IGroupRepository m_groupRepository = null;
        private ICurriculumItemRepository m_curriculumItemRepository = null;
        private IWorkRepository m_workRepository = null;
        private IWorkTypeRepository m_workTypeRepository = null;
        private ITeacherRepository m_teacherRepository = null;
        private IGroupsToTAItemRepository m_groupsToTeacherRepository = null;
        private ITAItemRepository m_TAItemRepository = null;
        private IPositionRepository m_positionRepository = null;

        private bool isSelectionChanging = false;
        private string m_dbPath;
        private List<int> m_groupsCopyBuffer = new List<int>();

        private double selectedFreeHours = 0;
        private int selectedWorkId = 0;
        private int selectedWorkIndex = 0;
        private int selectedCurriculumSubjectId = 0;
        private int selectedTeacherRowInd = -1;

        private string m_educType;
        private string m_educForm;
        private uint m_course;
        private uint m_semestr;
        private string m_educLevel;
        private bool m_isEmptyWorks;
        private bool isLoading = false;
        private int sortColumnIndex = 0;

        private int? replacingTeacherRowInd = null;
        private int? allHoursAssignmentCurriculumSubjectId = null;

        const string IncorrectDataMessage = "Некоректні дані";
        const string TeacherErrorMessage = "Цей викладач вже прив'язаний до даного виду роботи.";

        public TeacherAssignmentForm(MainForm parentForm,
                              string dbPath,
                              string educType,
                              string educForm,
                              uint course,
                              uint semestr,
                              string educLevel,
                              bool isEmptyWorks)
        {
            InitializeComponent();
            m_dbPath = dbPath;

            m_educType = educType;
            m_educForm = educForm;
            m_course = course;
            m_semestr = semestr;
            m_educLevel = educLevel;
            m_isEmptyWorks = isEmptyWorks;

            comboBoxTATeachers.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxTATeachers.Enabled = false;

            m_parentForm = parentForm;
            m_subjectRepository = new SubjectRepository(dbPath);
            m_groupRepository = new GroupRepository(dbPath);
            m_curriculumItemRepository = new CurriculumItemRepository(dbPath);
            m_workRepository = new WorkRepository(dbPath);
            m_workTypeRepository = new WorkTypeRepository(dbPath);
            m_teacherRepository = new TeacherRepository(dbPath);
            m_groupsToTeacherRepository = new GroupsToTAItemRepository(dbPath);
            m_TAItemRepository = new TAItemRepository(dbPath);
            m_positionRepository = new PositionRepository(dbPath);
        }

        public void UpdateWorks(bool isEmptyWorks)
        {
            m_isEmptyWorks = isEmptyWorks;

            if (dataGridViewTASubjects.SelectedCells.Count > 0)
            {
                var selectedRowIndex = dataGridViewTASubjects.SelectedCells[0].RowIndex;
                dataGridViewTASubjects.Rows[selectedRowIndex].Selected = false;
                dataGridViewTASubjects.Rows[selectedRowIndex].Selected = true;
            }
        }

        public void LoadData()
        {
            var curriculumItems = GetAllSortedCurriculumItems();
            var subjects = m_subjectRepository.GetSubjects(curriculumItems.Select(item => item.SubjectId).ToList());

            foreach (var item in curriculumItems)
            {
                dataGridViewTASubjects.Rows.Add(subjects[item.SubjectId].Name, item.Course);
            }

            LoadTeachers();
        }

        private void LoadTeachers()
        {
            comboBoxTATeachers.Items.Clear();
            comboBoxTATeachers.DroppedDown = false;

            var teachers = m_teacherRepository.GetAllTeachers(a => a.Name);
            var positions = m_positionRepository.GetAllPositions();
            comboBoxTATeachers.Items.Clear();

            foreach (var teacher in teachers)
            {
                var position = positions.First(p => p.Id == teacher.PositionId);
                comboBoxTATeachers.Items.Add(
                    new ComboBoxCustomItem(
                        teacher.Name,
                        position.MinHours * teacher.StaffUnit,
                        m_TAItemRepository.GetAssignedTeacherHours(teacher.Id, "Бюджет"),
                        position.MaxHours * teacher.StaffUnit));
            }
        }

        private uint getCourseBySelected()
        {
            return m_curriculumItemRepository.GetCurriculumItem(m_workRepository.GetWork(selectedWorkId).CurriculumItemId).Course;
        }

        private void TeacherAssignment_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_parentForm.TAForm = null;
            m_parentForm.SetButtonState();
        }

        private void dataGridViewTASubjects_SelectionChanged(object sender, EventArgs e)
        {
            if (this.isLoading || !(dataGridViewTASubjects.SelectedRows.Count == 1))
            {
                return;
            }

            isSelectionChanging = true;

            var curriculumItems = GetAllSortedCurriculumItems();
            var rowIndex = dataGridViewTASubjects.SelectedRows[0].Index;

            if (curriculumItems.Count <= rowIndex)
            {
                return;
            }

            m_groupsCopyBuffer.Clear();

            selectedCurriculumSubjectId = curriculumItems[rowIndex].Id;

            LoadWorks(curriculumItems[rowIndex].Id);

            isSelectionChanging = false;
        }

        private void LoadWorks(int curriculumItemId)
        {
            this.isLoading = true;

            dataGridViewTAWorks.Rows.Clear();

            var works = m_workRepository.GetWorks(curriculumItemId, m_isEmptyWorks);
            var taItemDict = m_TAItemRepository.GetTAItems(works);
            var newRows = new List<DataGridViewRow>();

            foreach (var work in works)
            {
                if (!taItemDict.TryGetValue(work.Id, out var taItems))
                {
                    taItems = new List<TAItem>();
                }
                double sum = 0;

                foreach (var taItem in taItems)
                {
                    sum += taItem.WorkHours;
                }
                var newRow = (DataGridViewRow)dataGridViewTAWorks.RowTemplate.Clone();
                newRow.CreateCells(dataGridViewTAWorks, m_workTypeRepository.GetWorkType(work.WorkTypeId)?.Name?.ToString(), (work.TotalHours - sum).ToString(Tools.HoursAccuracy));
                newRows.Add(newRow);
            }

            dataGridViewTAWorks.Rows.AddRange(newRows.ToArray());
            dataGridViewTAWorks.Refresh();
            this.isLoading = false;
        }

        private void CheckComboBoxTeacherState()
        {
            if (isLoading || isSelectionChanging)
            {
                return;
            }

            var rowIndex = dataGridViewTAWorks.SelectedCells[0].RowIndex;
            var works = m_workRepository.GetWorks(selectedCurriculumSubjectId, m_isEmptyWorks);

            if (rowIndex >= works.Count)
            {
                return;
            }

            string sFreeHours = dataGridViewTAWorks.Rows[rowIndex].Cells[1].Value.ToString();
            double dFreeHours;

            try
            {
                dFreeHours = Convert.ToDouble(sFreeHours);
            }
            catch (Exception)
            {
                return;
            }

            comboBoxTATeachers.Enabled = Tools.isGreaterThanZero(dFreeHours);
        }

        private void dataGridViewTAWorks_SelectionChanged(object sender, EventArgs e)
        {
            comboBoxTATeachers.Enabled = false;

            this.selectedTeacherRowInd = -1;

            if (!(dataGridViewTAWorks.SelectedCells.Count == 1 || (dataGridViewTAWorks.SelectedRows.Count == 1)))
            {
                return;
            }

            var rowIndex = dataGridViewTAWorks.SelectedCells[0].RowIndex;
            var works = m_workRepository.GetWorks(selectedCurriculumSubjectId, m_isEmptyWorks);

            if (rowIndex >= works.Count)
            {
                return;
            }

            string sFreeHours = dataGridViewTAWorks.Rows[rowIndex].Cells[1].Value.ToString();
            double dFreeHours;

            try
            {
                dFreeHours = Convert.ToDouble(sFreeHours);
            }
            catch (Exception)
            {
                return;
            }

            selectedWorkId = works[rowIndex].Id;
            selectedWorkIndex = rowIndex;
            selectedFreeHours = dFreeHours;

            isSelectionChanging = true;
            LoadTAItems();
            isSelectionChanging = false;

            comboBoxTATeachers.Enabled = Tools.isGreaterThanZero(dFreeHours);
        }

        private void LoadTAItems()
        {
            this.isLoading = true;
            for (int i = 0, limit = dataGridViewTATeachers.Rows.Count; i < limit; ++i)
            {
                RemoveLastTAItem();
            }

            var taItems = m_TAItemRepository.GetTAItems(selectedWorkId);

            foreach (var item in taItems)
            {
                var teacherName = m_teacherRepository.GetTeacher(item.TeacherId)?.Name;
                var rowInd = dataGridViewTATeachers.Rows.Add(teacherName, item?.WorkHours.ToString(Tools.HoursAccuracy), "");

                var contextMenuStrip = new ContextMenuStrip();
                contextMenuStrip.Items.Add(new ToolStripMenuItem("Копіювати групи", null, (obj, e) => { this.CopyGroups(rowInd); }));
                contextMenuStrip.Items.Add(new ToolStripMenuItem("Вставити групи", null, (obj, e) => { this.PasteGroups(rowInd); }));
                contextMenuStrip.Items.Add(new ToolStripMenuItem("Замінити викладача", null, (obj, e) => { this.StartTeacherReplacement(rowInd); }));
                dataGridViewTATeachers.Rows[rowInd].Cells[0].ContextMenuStrip = contextMenuStrip;
            }
            this.isLoading = false;
        }

        private void RemoveLastTAItem()
        {
            int index = dataGridViewTATeachers.Rows.Count - 1;

            if (index < 0)
            {
                return;
            }

            dataGridViewTATeachers.Rows.Remove(dataGridViewTATeachers.Rows[index]);
        }

        private void dataGridViewTATeachers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (this.isLoading || m_TAItemRepository == null)
            {
                return;
            }

            if (e.RowIndex < 0)
            {
                return;
            }

            var TAItems = m_TAItemRepository.GetTAItems(selectedWorkId);

            if (e.RowIndex < TAItems.Count)
            {
                var item = TAItems[e.RowIndex];

                if (!UpdateTAItem(item, e, false))
                {
                    return;
                }

                m_TAItemRepository.UpdateTAItem(item);

                SyncHours(item.WorkId);
            }
            else
            {
                var item = new TAItem();

                if (!UpdateTAItem(item, e, true))
                {
                    RemoveLastTAItem();
                    return;
                }

                m_TAItemRepository.AddTAItem(item);

                var TAItemsWithSaved = m_TAItemRepository.GetTAItems(selectedWorkId);
                var savedItem = TAItemsWithSaved[e.RowIndex];
                var groups = m_groupRepository.GetGroups(a => a.Name, savedItem.Course, savedItem.EducForm, savedItem.EducLevel);

                foreach (var group in groups)
                {
                    var groupToTeacherItem = new GroupsToTAItem();
                    groupToTeacherItem.TAItemID = savedItem.Id;
                    groupToTeacherItem.GroupId = group.Id;
                    m_groupsToTeacherRepository.AddGroupsToTAItem(groupToTeacherItem);
                }

                SyncHours(item.WorkId);
            }
            LoadTAItems();
            dataGridViewTATeachers.ClearSelection();
            dataGridViewTATeachers.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            CheckComboBoxTeacherState();
        }

        private bool isValidName(string name)
        {
            int duplicatesCounter = 0;

            if (name == null || name == "")
            {
                return false;
            }

            for (int i = 0; i < dataGridViewTATeachers.Rows.Count; ++i)
            {
                if (dataGridViewTATeachers[0, i].Value?.ToString().ToLower() == name.ToLower())
                {
                    ++duplicatesCounter;
                }
            }

            return duplicatesCounter > 1 ? false : true;
        }

        private void SyncData()
        {
            this.isLoading = true;
            LoadWorks(selectedCurriculumSubjectId);
            LoadTAItems();
            LoadTeachers();
            this.isLoading = false;
        }

        private void SyncHours(int workId)
        {
            var taItems = m_TAItemRepository.GetTAItems(workId);
            double sum = 0;

            foreach (var item in taItems)
            {
                sum += item.WorkHours;
            }

            dataGridViewTAWorks[1, selectedWorkIndex].Value = (m_workRepository.GetWork(workId).TotalHours - sum).ToString(Tools.HoursAccuracy);
        }

        private bool UpdateTAItem(TAItem item, DataGridViewCellEventArgs e, bool isNewObject)
        {
            try
            {
                var Name = dataGridViewTATeachers[0, e.RowIndex].Value?.ToString()?.Trim();

                if (!isValidName(Name))
                {
                    MessageBox.Show(IncorrectDataMessage, Tools.MessageBoxErrorTitle(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (!isNewObject)
                    {
                        SyncData();
                    }

                    return false;
                }

                switch (e.ColumnIndex)
                {
                    case 0:
                        {
                            var teacher = m_teacherRepository.GetTeacherByName(
                                dataGridViewTATeachers[e.ColumnIndex, e.RowIndex]?.Value?.ToString()?.Trim());
                            var work = m_workRepository.GetWork(item.WorkId);

                            if (teacher == null)
                                throw new Exception();

                            item.TeacherId = teacher.Id;
                            item.WorkId = selectedWorkId;
                            item.Course = getCourseBySelected();
                            item.EducType = m_educType;
                            item.EducForm = m_educForm;
                            item.EducLevel = m_educLevel;
                            item.Semestr = m_semestr;
                            item.WorkHours = Convert.ToDouble(dataGridViewTAWorks[1, selectedWorkIndex].Value);

                            break;
                        }
                    case 1:
                        {
                            var workHours = Convert.ToDouble(dataGridViewTATeachers[e.ColumnIndex, e.RowIndex].Value?.ToString());
                            var freeHours = Convert.ToDouble(dataGridViewTAWorks[1, selectedWorkIndex].Value);
                            var work = m_workRepository.GetWork(item.WorkId);
                            double sum = 0;

                            for (int i = 0; i < dataGridViewTATeachers.RowCount; ++i)
                            {
                                sum += Convert.ToDouble(dataGridViewTATeachers.Rows[i].Cells[1].Value.ToString());
                            }

                            if (Tools.isLessThanZero(workHours) || Tools.isGreaterThan(sum, Math.Round(work.TotalHours, 2)))
                            {
                                throw new Exception();
                            }

                            dataGridViewTAWorks[1, selectedWorkIndex].Value = (work.TotalHours - sum).ToString(Tools.HoursAccuracy);

                            item.WorkHours = workHours;

                            break;
                        }
                    case 2:
                        {
                            break;
                        }
                    default: { return false; }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(IncorrectDataMessage);
                SyncData();
                return false;
            }

            return true;
        }

        private void dataGridViewTATeachers_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if(this.isLoading || e.RowIndex < 0)
            {
                return;
            }

            var items = m_TAItemRepository.GetAllTAItems(selectedWorkId, m_educType, m_educForm, getCourseBySelected(), m_semestr, m_educLevel);

            if (e.RowIndex >= items.Count)
            {
                return;
            }

            if (!isSelectionChanging)
            {
                for (int i = e.RowIndex, limit = e.RowIndex + e.RowCount; i < limit; ++i)
                {
                    var groupsToTAItems = m_groupsToTeacherRepository.GetGroupsToTAItemsByTAItemId(items[i].Id);

                    m_groupsToTeacherRepository.RemoveGroupsToTAItems(groupsToTAItems);
                    m_TAItemRepository.RemoveTAItem(items[i]);
                }

                SyncHours(selectedWorkId);
            }

            CheckComboBoxTeacherState();
        }

        private void comboBoxTATeachers_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboBoxTATeachers.SelectedIndex < 0)
            {
                return;
            }

            var cbItem = comboBoxTATeachers.SelectedItem.ToString();
            var teacherName = cbItem.Substring(0, cbItem.IndexOf('(') - 1);
            var teacher = m_teacherRepository.GetTeacherByName(teacherName);

            if (this.replacingTeacherRowInd.HasValue)
            {
                this.CommitTeacherReplacement(teacher);
            }
            else if (this.allHoursAssignmentCurriculumSubjectId.HasValue)
            {
                this.CommitAllHoursAssignment(allHoursAssignmentCurriculumSubjectId.Value, teacher.Id);
            }
            else
            {
                var dgvIndex = dataGridViewTATeachers.Rows.Count;

                if (teacher != null)
                {
                    dataGridViewTATeachers.Rows.Add();
                    dataGridViewTATeachers.Rows[dgvIndex].SetValues(teacher.Name, dataGridViewTAWorks.Rows[selectedWorkIndex].Cells[1].Value.ToString(), "");

                    if(dataGridViewTATeachers.Rows.Count != dgvIndex) //check that new teacher wasn't deleted for some reason
                    {
                        //Scroll DataGridView to bottom and start editing hours
                        dataGridViewTATeachers.CurrentCell = dataGridViewTATeachers.Rows[dgvIndex].Cells[1];
                        dataGridViewTATeachers.BeginEdit(true);
                    }
                }
            }

            LoadTeachers();
        }

        private void comboBoxTATeachers_DropDownClosed(object sender, EventArgs e)
        {
            this.replacingTeacherRowInd = null;
            this.allHoursAssignmentCurriculumSubjectId = null;
        }

        private void dataGridViewTATeachers_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewTATeachers.RowCount <= 0 || dataGridViewTATeachers.SelectedCells.Count <= 0)
            {
                if (m_selectGroupsForm != null)
                    m_selectGroupsForm.Visible = false;
                m_selectGroupsForm?.Close();
                m_selectGroupsForm = null;
                this.selectedTeacherRowInd = -1;
                return;
            }

            if (dataGridViewTATeachers.SelectedCells[0].RowIndex == this.selectedTeacherRowInd)
            {
                return;
            }

            if (m_selectGroupsForm != null)
                m_selectGroupsForm.Visible = false;
            m_selectGroupsForm?.Close();
            m_selectGroupsForm = null;

            var rowIndex = dataGridViewTATeachers.SelectedCells[0].RowIndex;
            this.selectedTeacherRowInd = rowIndex;
            var taItems = m_TAItemRepository.GetTAItems(selectedWorkId);

            if (rowIndex >= taItems.Count)
            {
                return;
            }

            var TAItem = taItems[rowIndex];
            m_selectGroupsForm = new SelectGroups(TAItem, m_dbPath);

            m_selectGroupsForm.TopLevel = false;
            m_selectGroupsForm.Size = panelGroups.Size;

            LoadTeachers();
            panelGroups.Controls.Add(m_selectGroupsForm);
            panelGroups.Tag = m_selectGroupsForm;
            m_selectGroupsForm.BringToFront();
            m_selectGroupsForm.Show();
        }

        private void dataGridViewTASubjects_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0 || e.ColumnIndex == 1)
            {
                dataGridViewTASubjects.EndEdit();
                dataGridViewTATeachers.EndEdit();
                dataGridViewTAWorks.EndEdit();

                sortColumnIndex = e.ColumnIndex;

                PerformSort();
            }
        }

        private List<CurriculumItem> GetAllSortedCurriculumItems()
        {
            if (sortColumnIndex == 0)
            {
                return m_curriculumItemRepository.GetAllCurriculumItems(
                    item => m_subjectRepository.GetSubject(item.SubjectId).Name, m_educType, m_educForm, m_course, m_semestr, m_educLevel);
            }
            else
            {
                return m_curriculumItemRepository.GetAllCurriculumItems(
                    item => item.Course + m_subjectRepository.GetSubject(item.SubjectId).Name, m_educType, m_educForm, m_course, m_semestr, m_educLevel);
            }
        }

        private void PerformSort()
        {
            if (!isLoading)
            {
                var scrollingColumnIndex = dataGridViewTASubjects.FirstDisplayedScrollingColumnIndex;
                var scrollingRowIndex = dataGridViewTASubjects.FirstDisplayedScrollingRowIndex;
                isLoading = true;
                dataGridViewTASubjects.Rows.Clear();
                LoadData();
                isLoading = false;
                dataGridViewTASubjects.FirstDisplayedScrollingColumnIndex = scrollingColumnIndex;
                dataGridViewTASubjects.FirstDisplayedScrollingRowIndex = scrollingRowIndex;
            }
        }

        private void comboBoxTATeachers_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < comboBoxTATeachers.Items.Count && e.Index >= 0)
            {
                var item = comboBoxTATeachers.Items[e.Index] as ComboBoxCustomItem;

                if (item != null)
                {
                    Brush brush = ((e.State & DrawItemState.Selected) == DrawItemState.Selected) ?
                         new SolidBrush(Color.SlateGray) : new SolidBrush(item.GetColor());
                    e.Graphics.FillRectangle(brush, e.Bounds);
                    e.Graphics.DrawString(item.ToString(), e.Font, new SolidBrush(Color.Black), new Point(e.Bounds.X, e.Bounds.Y));
                }
                else
                {
                    e.Graphics.DrawString(comboBoxTATeachers.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.Black), new Point(e.Bounds.X, e.Bounds.Y));
                }

                e.DrawFocusRectangle();
            }
        }

        private void dataGridViewTASubjects_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu m = new ContextMenu();
                m.MenuItems.Add(new MenuItem("Видалити навантаження"));
                m.MenuItems.Add(new MenuItem("Надати все навантаження викладачу"));

                int currentMouseOverRow = dataGridViewTASubjects.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    isSelectionChanging = true;
                    dataGridViewTASubjects.ClearSelection();
                    dataGridViewTASubjects.Rows[currentMouseOverRow].Selected = true;
                    isSelectionChanging = false;

                    m.MenuItems[0].Click += DeleteAssignment;
                    m.MenuItems[1].Click += StartAllHoursAssignment;
                    m.Show(dataGridViewTASubjects, new Point(e.X, e.Y));
                }
            }
        }

        private void DeleteAssignment(object sender, EventArgs e)
        {
            m_parentForm.Enabled = false;
            try
            {
                var works = m_workRepository.GetWorks(selectedCurriculumSubjectId, true);
                var taItems = new List<TAItem>();
                var groupsToTAItems = new List<GroupsToTAItem>();

                foreach (var work in works)
                {
                    taItems.AddRange(m_TAItemRepository.GetTAItems(work.Id));
                }

                foreach (var taItem in taItems)
                {
                    groupsToTAItems.AddRange(m_groupsToTeacherRepository.GetGroupsToTAItem(taItem.Id));
                }

                foreach (var groupToTAItem in groupsToTAItems)
                {
                    m_groupsToTeacherRepository.RemoveGroupsToTAItem(groupToTAItem);
                }

                foreach (var taItem in taItems)
                {
                    m_TAItemRepository.RemoveTAItem(taItem);
                }

                isSelectionChanging = true;
                var selectedRowIndex = dataGridViewTASubjects.SelectedRows[0].Index;
                dataGridViewTASubjects.ClearSelection();
                dataGridViewTASubjects.Rows[selectedRowIndex].Selected = true;
                isSelectionChanging = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.LoadTeachers();

            m_parentForm.Enabled = true;
        }

        private void CopyGroups(int teacherRowInd)
        {
            var taItems = m_TAItemRepository.GetTAItems(selectedWorkId);

            var TAItem = taItems[teacherRowInd];
            var groupsToTAItem = m_groupsToTeacherRepository.GetGroupsToTAItem(TAItem.Id);

            m_groupsCopyBuffer.Clear();

            foreach (var el in groupsToTAItem)
            {
                m_groupsCopyBuffer.Add(el.GroupId);
            }

            dataGridViewTATeachers.ClearSelection();
            dataGridViewTATeachers.Rows[teacherRowInd].Selected = true;

            MessageBox.Show("Групи скопійовані");
        }

        private void PasteGroups(int teacherRowInd)
        {
            var taItems = m_TAItemRepository.GetTAItems(selectedWorkId);

            var TAItem = taItems[teacherRowInd];
            var groupsToTAItem = m_groupsToTeacherRepository.GetGroupsToTAItem(TAItem.Id);

            if (m_groupsCopyBuffer.Count == 0)
            {
                return;
            }

            foreach (var el in groupsToTAItem)
            {
                m_groupsToTeacherRepository.RemoveGroupsToTAItem(el);
            }

            foreach (var el in m_groupsCopyBuffer)
            {
                var newGroup = new GroupsToTAItem();
                newGroup.GroupId = el;
                newGroup.TAItemID = TAItem.Id;
                m_groupsToTeacherRepository.AddGroupsToTAItem(newGroup);
            }

            dataGridViewTATeachers.ClearSelection();
            dataGridViewTATeachers.Rows[teacherRowInd].Selected = true;

            MessageBox.Show("Групи замінені");
        }

        private void StartTeacherReplacement(int teacherRowInd)
        {
            this.replacingTeacherRowInd = teacherRowInd;
            this.comboBoxTATeachers.DroppedDown = true;
        }

        private void CommitTeacherReplacement(Teacher newTeacher)
        {
            if (m_TAItemRepository.IsTeacherAssignedToWork(newTeacher.Id, selectedWorkId))
            {
                this.comboBoxTATeachers.DroppedDown = false;
                MessageBox.Show(TeacherErrorMessage, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var taItem = m_TAItemRepository.GetTAItems(selectedWorkId)[(int)this.replacingTeacherRowInd];
                taItem.TeacherId = newTeacher.Id;
                this.m_TAItemRepository.UpdateTAItem(taItem);
                this.SyncData();
            }
        }

        private void StartAllHoursAssignment(object sender, EventArgs e)
        {
            this.allHoursAssignmentCurriculumSubjectId = this.selectedCurriculumSubjectId;
            comboBoxTATeachers.DroppedDown = true;
        }

        private void CommitAllHoursAssignment(int curriculumItemId, int teacherId)
        {
            var curricullumItem = this.m_curriculumItemRepository.GetCurriculumItem(curriculumItemId);
            var works = m_workRepository.GetWorks(curriculumItemId, m_isEmptyWorks);
            m_TAItemRepository.DeleteAllTAItemsForWorks(
                works.Select(w => w.Id).ToList(),
                this.m_educType,
                this.m_educForm,
                this.getCourseBySelected(),
                this.m_semestr,
                m_educLevel);


            var newTAItems = works.Select(w => new TAItem()
            {
                TeacherId = teacherId,
                WorkId = w.Id,
                WorkHours = w.TotalHours,
                Course = getCourseBySelected(),
                EducType = m_educType,
                EducForm = m_educForm,
                EducLevel = m_educLevel,
                Semestr = m_semestr
            }).ToList();

            m_TAItemRepository.AddTAItems(newTAItems);

            var groupsToTAItems = new List<GroupsToTAItem>();
            var taItemsGrouped = newTAItems.GroupBy(x => (Course: x.Course, EducForm: x.EducForm, EducLevel: x.EducLevel));

            foreach(var taItemGroup in taItemsGrouped)
            {
                var key = taItemGroup.Key;
                var groups = m_groupRepository.GetGroups(key.Course, key.EducForm, key.EducLevel);
                groupsToTAItems.AddRange(taItemGroup.SelectMany(ta => groups.Select(g => new GroupsToTAItem { GroupId = g.Id, TAItemID = ta.Id })));
            }

            m_groupsToTeacherRepository.AddGroupsToTAItems(groupsToTAItems);

            this.SyncData();
        }
    }
}
