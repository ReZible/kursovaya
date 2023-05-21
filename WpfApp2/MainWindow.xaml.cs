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
using WpfApp2.Views;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        object frameContent = null;
        public MainWindow()
        {
            InitializeComponent();
            AppData.CurrentUser = null;
            MainFrame.Navigate(new SignInPage());
        }

        private void MainFrame_ContentLoaded(object sender, EventArgs e)
        {
            if (MainFrame.CanGoBack && AppData.CurrentUser != null)
            {
                BtnGoBack.Visibility = Visibility.Visible;
            }
            else
            {
                BtnGoBack.Visibility = Visibility.Hidden;
            }


            if (AppData.CurrentUser != null)
            {
                MainMenu.Visibility = Visibility.Visible;
                if (AppData.CurrentUser.RoleId == 2)
                {
                    MenuAdmin.Visibility = Visibility.Collapsed;
                } else
                {
                    MenuAdmin.Visibility = Visibility.Visible;
                }
            } else
            {
                while (MainFrame.CanGoBack)
                {
                    MainFrame.NavigationService.RemoveBackEntry();
                }
            }
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.GoBack();
        }

        private void GoPersonalCabinet_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PersonalCabinetPage());
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SignInPage());
            AppData.CurrentUser = null;
            MainMenu.Visibility = Visibility.Hidden;
        }

        private void GoUserData_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UserPage());
        }

        private void GoEventData_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new EventPage());
        }

        private void GoToMainPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ShowEventsPage());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            frameContent = MainFrame.Content;
        }
    }
}
