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
        public MainWindow()
        {
            InitializeComponent();
            AppData.CurrentUser = null;
            MainFrame.Navigate(new SignInPage());
        }

        private void MainFrame_ContentLoaded(object sender, EventArgs e)
        {
            if (MainFrame.CanGoBack)
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
            }

        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.GoBack();
        }

        private void OpenPersonalCabinet(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PersonalCabinetPage());
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GoToMainPage(object sender, RoutedEventArgs e)
        {

        }

        private void CheckUsers(object sender, RoutedEventArgs e)
        {

        }

        private void CheckDisciplines(object sender, RoutedEventArgs e)
        {

        }

        private void CheckAllTasks(object sender, RoutedEventArgs e)
        {

        }

        private void CheckAllTaskBases(object sender, RoutedEventArgs e)
        {

        }
    }
}
