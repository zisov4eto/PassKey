﻿namespace PassKey.Sliders.MenuSliders
{
    using System;
    using System.Windows.Forms;
    using UserInfo;
    using MetroFramework.Controls;
    using Data;

    public partial class EditMenuPanel : BaseMenuSliderPanel
    {
        private int selectedRowIndex;
        private MetroGrid dataGrid;

        public EditMenuPanel(Form form, LoggedUser user, MetroGrid dataGrid)
            : base(form, user)
        {
            InitializeComponent();
            this.dataGrid = dataGrid;
            this.doneButton.UseSelectable = false;
            GetItemToEdit(dataGrid);    
        }        

        private void GetItemToEdit(MetroGrid dataGrid)
        {
            this.selectedRowIndex = dataGrid.CurrentCell.RowIndex;
            string hostName = dataGrid.Rows[selectedRowIndex].Cells[0].Value.ToString();
            string email = dataGrid.Rows[selectedRowIndex].Cells[1].Value.ToString();
            string username = dataGrid.Rows[selectedRowIndex].Cells[2].Value.ToString();
            string password = dataGrid.Rows[selectedRowIndex].Cells[3].Value.ToString();

            this.hostNameTextBox.Text = hostName;
            this.emailTextBox.Text = email;
            this.usernameTextBox.Text = username;
            this.passwordTextBox.Text = password;

            this.dataGrid.Enabled = false;    
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            this.User.RemoveData(selectedRowIndex);
            UserDataInfo dataInfo = new UserDataInfo(this.hostNameTextBox.Text
                , this.emailTextBox.Text, this.usernameTextBox.Text, this.passwordTextBox.Text);
            this.User.InsertData(selectedRowIndex, dataInfo);

            DataTranslator.Compose(this.User.Data, this.User.Username, this.User.Key);

            this.dataGrid.Enabled = true;
            this.Swipe(false);
        }

        private void metroLink1_Click(object sender, EventArgs e)
        {
            this.dataGrid.Enabled = true;
            this.Swipe(false);
        }
    }
}