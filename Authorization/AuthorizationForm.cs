using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Authorization
{
    public partial class AuthorizationForm : Form
    {
        private static UserDbConection DB = new UserDbConection();
        public AuthorizationForm()
        {
            InitializeComponent();
            LogInButton.Click += (e, sender) => 
            {
                if (InputName.Text.Length < 6)
                {
                    MessageBox.Show("Имя должно содержать не менее 6 символов",
                       "Ошибка",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    return;
                }
                if (InputPassword.Text.Length < 6)
                {
                    MessageBox.Show("Пароль должен содержать не менее 6 символов",
                       "Ошибка",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    return;
                }
                if (!DB.LogIn(InputName.Text, InputPassword.Text, checkBoxIsAdmin.Checked))
                {
                    MessageBox.Show("Ошибка входа: неверное имя, пароль или роль",
                       "Ошибка",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    return;
                }
                if (DB.LogIn(InputName.Text, InputPassword.Text, checkBoxIsAdmin.Checked))
                {

                }
            };
            RegButton.Click += (e, sender) =>
            {
                if (InputName.Text.Length < 6)
                {
                    MessageBox.Show("Имя должно содержать не менее 6 символов",
                       "Ошибка",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    return;
                }
                if (InputPassword.Text.Length < 6)
                {
                    MessageBox.Show("Пароль должен содержать не менее 6 символов",
                       "Ошибка",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    return;
                }
                if (!DB.Registr(InputName.Text, InputPassword.Text))
                {
                    MessageBox.Show("Такой пользователь уже существует",
                       "Ошибка",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    return;
                }
                if (DB.Registr(InputName.Text, InputPassword.Text))
                {
                    MessageBox.Show("Регистрация прошла успешно!",
                       "Успех",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Information);
                    InputName.Text = "";
                    InputPassword.Text = "";
                }
            };
        }
    }
}
