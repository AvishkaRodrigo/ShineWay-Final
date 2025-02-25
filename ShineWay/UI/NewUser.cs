﻿using ShineWay.Messages;
using System;
using ShineWay.Classes;
using System.Text;
using System.Text.RegularExpressions;
using ShineWay.DataBase;
using System.Windows.Forms;

namespace ShineWay.UI
{
    public partial class NewUser : Form
    {
        bool isPasswordHideClicked = true;
        bool isConfirmPasswordHideClicked = true;

        System.Drawing.Color closeBtnColor;
        string passwordPattern = "[a-zA-Z]+[0-9]+";
        string userName;

        public NewUser(string userName, string name)
        {
            InitializeComponent();
            closeBtnColor = btn_Close.ForeColor;
            this.userName = userName;
            label_welcome.Text = $"Welcome {name.Split(" ")[0]}!";
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Close_MouseHover(object sender, EventArgs e)
        {
            btn_Close.ForeColor = System.Drawing.Color.Red;
        }

        private void btn_Close_MouseLeave(object sender, EventArgs e)
        {
            btn_Close.ForeColor = closeBtnColor;
        }

        private void btn_register_MouseHover(object sender, EventArgs e)
        {
            btn_register.Image = ShineWay.Properties.Resources.registerHover;
        }

        private void btn_register_MouseLeave(object sender, EventArgs e)
        {
            btn_register.Image = ShineWay.Properties.Resources.register;
        }

        private void btn_register_Click(object sender, EventArgs e)
        {
            if(txt_newPassword.Text.Trim().Length == 0 || txt_confirmPassword.Text.Trim().Length == 0)
            {
                CustomMessage message = new CustomMessage("Password cannot be Empty!", "Error Dialog", ShineWay.Properties.Resources.error, DialogResult.OK);
                message.convertToOkButton();
                message.ShowDialog();
            }
            else
            {
                if (Regex.IsMatch(txt_newPassword.Text, passwordPattern) && txt_newPassword.Text.Length >= 8 && txt_newPassword.Text.Length <= 15 && txt_newPassword.Text == txt_confirmPassword.Text)
                {
                    try
                    {
                        string query = $"UPDATE `users` SET `password`=\"{Encrypt.encryption(txt_newPassword.Text)}\",`isFirstTimeUser`=\"{0}\" WHERE `username` = \"{userName}\"";
                        DbConnection.Update(query);
                        
                        CustomMessage message = new CustomMessage("Password Updated!\nLogin Again.", "Updated", ShineWay.Properties.Resources.correct, DialogResult.OK);
                        message.convertToOkButton();
                        message.ShowDialog();
                        txt_confirmPassword.Text = "";
                        txt_newPassword.Text = "";
                        Application.Restart();
                    }
                    catch (Exception exx)
                    {
                        CustomMessage message = new CustomMessage("Failed to update Password\nTry Again!", "Update Failed", ShineWay.Properties.Resources.error, DialogResult.OK);
                        message.convertToOkButton();
                        message.ShowDialog();

                    }
                    
                }
                else
                {
                    if (!(txt_newPassword.Text == txt_confirmPassword.Text))
                    {
                        CustomMessage message = new CustomMessage("Password fields Does \nnot match!", "Error Dialog", ShineWay.Properties.Resources.error, DialogResult.OK);
                        message.convertToOkButton();
                        message.ShowDialog();
                    }
                    else if (txt_newPassword.Text.Length < 8)
                    {
                        CustomMessage message = new CustomMessage("Minimum length of the \npassword is 8 charactors!", "Error Dialog", ShineWay.Properties.Resources.information, DialogResult.OK);
                        message.convertToOkButton();
                        message.ShowDialog();
                    }
                    else if (txt_newPassword.Text.Length > 15)
                    {
                        CustomMessage message = new CustomMessage("Maximum length of the \npassword is 15 charactors!", "Error Dialog", ShineWay.Properties.Resources.information, DialogResult.OK);
                        message.convertToOkButton();
                        message.ShowDialog();
                    }
                    else
                    {
                        CustomMessage message = new CustomMessage("Password should contain \nboth numbers and letters!", "Error Dialog", ShineWay.Properties.Resources.information, DialogResult.OK);
                        message.convertToOkButton();
                        message.ShowDialog();
                    }

                }
            }
            
        }

        private void btn_showPassword_Click(object sender, EventArgs e)
        {
            if (isPasswordHideClicked)
            {
                btn_showPassword.Image = ShineWay.Properties.Resources.hideEye;
                txt_newPassword.UseSystemPasswordChar = false;
                isPasswordHideClicked = false;
            }
            else
            {
                btn_showPassword.Image = ShineWay.Properties.Resources.eye;
                txt_newPassword.UseSystemPasswordChar = true;
                isPasswordHideClicked = true;
            }
            
        }

        private void btn_sowConfirmPassword_Click(object sender, EventArgs e)
        {
            if (isConfirmPasswordHideClicked)
            {
                btn_sowConfirmPassword.Image = ShineWay.Properties.Resources.hideEye;
                txt_confirmPassword.UseSystemPasswordChar = false;
                isConfirmPasswordHideClicked = false;
            }
            else
            {
                btn_sowConfirmPassword.Image = ShineWay.Properties.Resources.eye;
                txt_confirmPassword.UseSystemPasswordChar = true;
                isConfirmPasswordHideClicked = true;
            }
        }

        private void txt_newPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter|| e.KeyCode == Keys.Down)
            {
                txt_confirmPassword.Focus();
            }
        }

        private void txt_confirmPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_register.PerformClick();
            }
            else if(e.KeyCode == Keys.Up)
            {
                txt_newPassword.Focus();
                e.SuppressKeyPress = true; //to remove the 'ding' sound
            }
            else if (e.KeyCode == Keys.Down)
            {
                btn_register.Focus();
            }
        }
    }
}
