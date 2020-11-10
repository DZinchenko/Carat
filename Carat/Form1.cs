using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Carat
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();

            customizeDesign();
        }

        private void customizeDesign()
        {
            hideSubMenu(tablesPanel);
            hideSubMenu(distributionSubMenuPanel);
            hideSubMenu(reportsPanel);
        }

        private void hideSubMenu(Panel panel)
        {
            panel.Visible = false;
        }

        private void invertSubMenuVisibleState(Panel panel)
        {
            panel.Visible = (!(panel.Visible));
        }

        private void moveCurrentPanelPointer(Button button, Panel panel)
        {
            if (panel.Visible == true)
            {
                currentPanel.Location = button.Location;
                currentPanel.Size = button.Size;
            }
        }

        private void tablesSubMenuButton_Click(object sender, EventArgs e)
        {
            invertSubMenuVisibleState(tablesPanel);
            moveCurrentPanelPointer(tablesSubMenuButton, tablesPanel);
        }

        private void distributionSubMenuButton_Click(object sender, EventArgs e)
        {
            invertSubMenuVisibleState(distributionSubMenuPanel);
            moveCurrentPanelPointer(distributionSubMenuButton, distributionSubMenuPanel);
        }

        private void reportsSubMenuButton_Click(object sender, EventArgs e)
        {
            invertSubMenuVisibleState(reportsPanel);
            moveCurrentPanelPointer(reportsSubMenuButton, reportsPanel);
        }

        private void helpButton_Click_1(object sender, EventArgs e)
        {
            moveCurrentPanelPointer(helpButton, panelMainLeft);
        }

        private void aboutButton_Click_1(object sender, EventArgs e)
        {
            moveCurrentPanelPointer(aboutButton, panelMainLeft);
        }
    }
}
