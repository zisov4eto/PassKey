﻿namespace PassKey.Sliders.MenuSliders
{
    using System;
    using System.Windows.Forms;
    using UserInfo;
    using Data;
    using SecurityUtilities;
    using Exceptions;
    using MetroFramework;

    public partial class ChangePasswordPanel : BaseMenuSliderPanel
    {
        public ChangePasswordPanel(Form form, LoggedUser user)
            : base(form, user)
        {
            InitializeComponent();
            this.changeButton.UseSelectable = false;
        }

        private void changeButton_Click(object sender, EventArgs e)
        {
            this.oldPassLabel.Visible = false;
            this.newPassLabel.Visible = false;

            string currentPassword = this.oldPasswordTextBox.Text;
            string currentPasswordHash = HashUtilities.HashPassword(currentPassword);
            string hashedPassword = RegistryData.GetUserPassword(this.User.Username);
            if (currentPasswordHash == hashedPassword)
            {
                try
                {
                    ValidateNewPassword(this.newPassTextBox.Text, this.confirmNewPassTextBox.Text);
                    string oldEncryptedData = RegistryData.GetUserData(this.User.Username);
                    string oldDecryptedData = CryptographicUtilities.Decrypt(oldEncryptedData, this.User.Key);
                    string newPassword = HashUtilities.HashPassword(this.newPassTextBox.Text);
                    byte[] newKey = HashUtilities.HashKey(this.newPassTextBox.Text);
                    string newData = CryptographicUtilities.Encrypt(oldDecryptedData, newKey);
                    RegistryData.SetNewPassword(this.User.Username, newPassword);
                    RegistryData.SetUserData(this.User.Username, newData);
                    this.User.SetNewKey(newKey);

                    MetroMessageBox.Show(this.MainForm, string.Empty, GlobalMessages.PasswordChanged
                        , MessageBoxButtons.OK, MessageBoxIcon.Information, 80);

                    this.Swipe(false);
                }
                catch (InvalidPasswordLenghtException ipe)
                {
                    this.newPassLabel.Text = ipe.Message;
                    this.newPassLabel.Visible = true;
                }
                catch (PasswordMismatchException pme)
                {
                    this.newPassLabel.Text = pme.Message;
                    this.newPassLabel.Visible = true;
                }
            }
            else
            {
                this.oldPassLabel.Text = GlobalMessages.InvalidPassword;
                this.oldPassLabel.Visible = true;
            }
        }

        private void ValidateNewPassword(string password, string confirmedPassword)
        {
            if (password != confirmedPassword)
            {
                throw new PasswordMismatchException(GlobalMessages.PasswordsMismatch);
            }

            if (string.IsNullOrWhiteSpace(password) ||
                password.Length < Constants.MinPasswordLenght)
            {
                throw new InvalidPasswordLenghtException(string.Format(GlobalMessages.InvalidPasswordLenght
                    , Constants.MinPasswordLenght
                    , Constants.MaxPasswordLenght));
            }
        }

        private void backLink_Click(object sender, EventArgs e)
        {
            this.Swipe(false);
        }
    }
}