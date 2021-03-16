using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Carat.Data.Entities;
using Carat.Data.Repositories;
using Carat.EF.Repositories;

namespace Carat
{
    public partial class SelectGroups : Form
    {
        private IGroupRepository m_groupRepository;
        private IGroupsToTAItemRepository m_groupsToTAItemRepository;
        private TAItem m_TAItem;
        private List<Group> m_groups;

        public SelectGroups(TAItem TAItem, string dbPath)
        {
            InitializeComponent();

            m_TAItem = TAItem;
            m_groupRepository = new GroupRepository(dbPath);
            m_groupsToTAItemRepository = new GroupsToTAItemRepository(dbPath); 
            m_groups = m_groupRepository.GetGroups(m_TAItem.Course, m_TAItem.EducForm, m_TAItem.EducLevel);

            LoadGroups();
        }

        private void LoadGroups()
        {
            var groupsToTAItems = m_groupsToTAItemRepository.GetGroupsToTAItem(m_TAItem.Id);

            foreach (var group in m_groups)
            {
                checkedListBoxGroups.Items.Add(group.Name);

                if (groupsToTAItems.Any(b => b.GroupId == group.Id))
                {
                    checkedListBoxGroups.SetItemCheckState(checkedListBoxGroups.Items.Count - 1, CheckState.Checked);
                }
            }
        }

        private void checkedListBoxGroups_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }

            if (e.NewValue == CheckState.Checked)
            {
                var newGroupsToTAItem = new GroupsToTAItem();

                newGroupsToTAItem.GroupId = m_groups[e.Index].Id;
                newGroupsToTAItem.TAItemID = m_TAItem.Id;

                if (m_groupsToTAItemRepository.GetGroupsToTAItem(m_TAItem.Id, m_groups[e.Index].Id) == null)
                    m_groupsToTAItemRepository.AddGroupsToTAItem(newGroupsToTAItem);
            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                var groupToTAItem = m_groupsToTAItemRepository.GetGroupsToTAItem(m_TAItem.Id, m_groups[e.Index].Id);

                if (groupToTAItem != null)
                    m_groupsToTAItemRepository.RemoveGroupsToTAItem(groupToTAItem);
            }
        }
    }
}
