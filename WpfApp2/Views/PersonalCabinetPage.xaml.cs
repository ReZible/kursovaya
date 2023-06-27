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
using System.Xml.Linq;
using WpfApp2.Model;

namespace WpfApp2.Views
{
    /// <summary>
    /// Логика взаимодействия для PersonalCabinetPage.xaml
    /// </summary>
    public partial class PersonalCabinetPage : Page
    {
        private int maxItemShow = 3;
        private int currentPageNumber = 1;
        private int pagesCount;
        public PersonalCabinetPage()
        {
            InitializeComponent();

            TbName.Text = AppData.CurrentUser.Name;
            TbLogin.Text = AppData.CurrentUser.Login;
            TbPassword.Password = AppData.CurrentUser.Password;
            TbRole.Text = AppData.CurrentUser.Role.Name;

            UpdateEvent();
        }

        private void ListViewItem_LeftMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListViewItem).DataContext as Event;
            if (item != null)
            {
                NavigationService.Navigate(new EventShowDetailsPage(item));
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserSelfEditPage(AppData.CurrentUser));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                AppData.db.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());

                if (AppData.CurrentUser != null)
                {
                    TbName.Text = AppData.CurrentUser.Name;
                    TbLogin.Text = AppData.CurrentUser.Login;
                    TbPassword.Password = AppData.CurrentUser.Password;
                    TbRole.Text = AppData.CurrentUser.Role.Name;

                    UpdateEvent();
                }
            }
        }
        private void UpdateEvent()
        {
            var currentServices = AppData.db.Event.Where(c => c.OrganizeId == AppData.CurrentUser.Id).ToList();

            var count = currentServices.Count();
            pagesCount = (int)Math.Ceiling((double)count / maxItemShow);
            LViewTours.ItemsSource = currentServices
                .Skip((currentPageNumber - 1) * maxItemShow)
                .Take(maxItemShow).ToList();

            CheckPages();
        }

        private void CheckPages()
        {
            BtnPagePrev.IsEnabled = currentPageNumber > 1;
            BtnPageNext.IsEnabled = currentPageNumber < pagesCount;
        }

        private void BtnPageNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageNumber < pagesCount)
            {
                currentPageNumber++;
                selectedPageTbx.Text = currentPageNumber.ToString();
                UpdateEvent();
            }
        }

        private void BtnPagePrev_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageNumber > 1)
            {
                currentPageNumber--;
                selectedPageTbx.Text = currentPageNumber.ToString();
                UpdateEvent();
            }
        }
    }
}