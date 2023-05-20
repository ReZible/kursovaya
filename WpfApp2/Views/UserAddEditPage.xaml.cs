using System;
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
    /// Логика взаимодействия для UserAddEditPage.xaml
    /// </summary>
    public partial class UserAddEditPage : Page
    {
        private User people = new User();
        public UserAddEditPage(User selectedUser)
        {
            InitializeComponent();
            if (selectedUser != null)
            {
                people = selectedUser;
                ComboRoles.SelectedItem = selectedUser.Role;
            }
            InitializeComponent();
            DataContext = people;
            ComboRoles.ItemsSource = AppData.db.Role.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(people.Login))
                errors.AppendLine("Укажите логин");
            if (string.IsNullOrWhiteSpace(people.Name))
                errors.AppendLine("Укажите имя");
            if (string.IsNullOrWhiteSpace(people.Password))
                errors.AppendLine("Укажите пароль");
            if (ComboRoles.SelectedItem == null)
                errors.AppendLine("Укажите роль");

            var repeatLogin = AppData.db.User.FirstOrDefault(u => u.Login == people.Login && u.Id != people.Id);
            if (repeatLogin != null)
                errors.AppendLine("Данный логин занят");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            var currentRole = ComboRoles.SelectedItem as Role;
            if (people.Id == 0)
            {
                people.RoleId = currentRole.Id;
                AppData.db.User.Add(people);
            }
            try
            {
                people.RoleId = currentRole.Id;
                AppData.db.SaveChanges();
                MessageBox.Show("Данные сохранены");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
