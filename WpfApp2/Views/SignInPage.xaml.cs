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
                errors.AppendLine("Укажите логин");
            if (string.IsNullOrWhiteSpace(TxbPassword.Text))
                errors.AppendLine("Укажите пароль");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            var CurrentUser = AppData.db.User.FirstOrDefault(u => u.Login == TxbLogin.Text && u.Password == TxbPassword.Text);

            if( CurrentUser != null)
            {   
                if(CurrentUser.RoleId == 1)
                {
                    NavigationService.Navigate(new AdminPage());
                }
                else
                {
                    NavigationService.Navigate(new ShopPage());
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
    }
}
