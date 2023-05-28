﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2.Model;

namespace WpfApp2.Views
{
    /// <summary>
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        private User people = new User();
        public RegisterPage()
        {
            InitializeComponent();
            DataContext = people;
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if(string.IsNullOrWhiteSpace(people.Login) || !Regex.IsMatch(people.Login, @"^[a-zA-Z0-9]+$"))
                errors.AppendLine("Укажите корректный логин. Логин может состоять из латиницы и цифр");
            if (string.IsNullOrWhiteSpace(people.Name))
                errors.AppendLine("Укажите имя");
            if (string.IsNullOrWhiteSpace(PbPassword.Password))
                errors.AppendLine("Укажите пароль");
            else if(!Regex.IsMatch(PbPassword.Password, @"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=\S+$).{8,}$")) 
                errors.AppendLine("Пароль должен состоять из заглавных,строчных символов,а также включать цифры, длина пароля не менее 8 символов");
            else if (string.IsNullOrWhiteSpace(PbPasswordRepeat.Password))
                errors.AppendLine("Подтвердите пароль");
            else if (PbPassword.Password != PbPasswordRepeat.Password)
                errors.AppendLine("Пароли не совпадают");

            /* Password Regex:
                    * ^              # start-of-string
                   (?=.*[0-9])       # a digit must occur at least once
                   (?=.*[a-z])       # a lower case letter must occur at least once
                   (?=.*[A-Z])       # an upper case letter must occur at least once
                   (?=\S+$)          # no whitespace allowed in the entire string
                   .{8,}             # anything, at least eight places though
                   $                 # end-of-string
               */

            var repeatLogin = AppData.db.User.FirstOrDefault(u => u.Login == people.Login);
            if (repeatLogin != null)
                errors.AppendLine("Данный логин занят");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (people.Id == 0)
                people.RoleId = 2;
                people.Password = PbPassword.Password;
                AppData.db.User.Add(people);
            try 
            {
                AppData.db.SaveChanges();
                MessageBox.Show("Вы успешно зарегистрировались!");
                NavigationService.Navigate(new SignInPage());
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void GoSingInBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SignInPage());
        }

        private void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}
