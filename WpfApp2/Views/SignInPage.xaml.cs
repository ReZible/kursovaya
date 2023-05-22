using System;
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
    /// Логика взаимодействия для SignInPage.xaml
    /// </summary>
    public partial class SignInPage : Page
    {
        public SignInPage()
        {
            InitializeComponent();
        }

        private void BtnSignIn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(TxbLogin.Text))
                errors.AppendLine("Укажите логин.");

            if (string.IsNullOrWhiteSpace(TxbPassword.Password))
                errors.AppendLine("Укажите пароль.");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            var CurrentUser =  AppData.db.User.Where(p => p.Login == TxbLogin.Text && p.Password == TxbPassword.Password)
                .AsEnumerable()
                .FirstOrDefault(p => p.Login == TxbLogin.Text && p.Password == TxbPassword.Password);

            if ( CurrentUser != null)
            {   
                AppData.CurrentUser = CurrentUser;
                if(CurrentUser.RoleId == 1)
                {
                    NavigationService.Navigate(new ShowEventsPage());
                }
                else
                {
                    NavigationService.Navigate(new ShowEventsPage());
                }
                
            }
            else
            {
                MessageBox.Show("Пользователь не найден");
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterPage());
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
