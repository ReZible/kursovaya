﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            if(string.IsNullOrWhiteSpace(people.Login))
                 errors.AppendLine("Укажите логин");
            if (string.IsNullOrWhiteSpace(people.Name))
                errors.AppendLine("Укажите имя");
            if (string.IsNullOrWhiteSpace(people.Password))
                errors.AppendLine("Укажите пароль");  

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
                AppData.db.User.Add(people);
            try 
            { 
                AppData.db.SaveChanges();
                MessageBox.Show("Вы успешно зарегистрировались!");
                NavigationService.GoBack();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void GoSingInBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}