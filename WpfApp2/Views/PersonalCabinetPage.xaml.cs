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
        private double PagesCount;
        private int NumberOfPage = 0;
        private int maxItemShow = 5;
        public PersonalCabinetPage()
        {
            InitializeComponent();
            var currentServices = AppData.db.Event.Where(c => c.OrganizeId == AppData.CurrentUser.Id).ToList();

            PagesCount = Math.Ceiling(Convert.ToDouble(currentServices.Count) / Convert.ToDouble(maxItemShow));
            LViewTours.ItemsSource = currentServices.Skip(maxItemShow * NumberOfPage).Take(maxItemShow).ToList();

            TbName.Text = AppData.CurrentUser.Name;
            TbLogin.Text = AppData.CurrentUser.Login;
            TbPassword.Text = AppData.CurrentUser.Password;
            TbRole.Text = AppData.CurrentUser.Role.Name;
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
                var currentServices = AppData.db.Event.Where(c => c.OrganizeId == AppData.CurrentUser.Id).ToList();
                PagesCount = Math.Ceiling(Convert.ToDouble(currentServices.Count) / Convert.ToDouble(maxItemShow));

                if (AppData.CurrentUser != null)
                {
                    LViewTours.ItemsSource = currentServices.Skip(maxItemShow * NumberOfPage).Take(maxItemShow).ToList();
                    TbName.Text = AppData.CurrentUser.Name;
                    TbLogin.Text = AppData.CurrentUser.Login;
                    TbPassword.Text = AppData.CurrentUser.Password;
                    TbRole.Text = AppData.CurrentUser.Role.Name;
                }
            }
        }
        private void UpdateShop()
        {
            var currentServices = AppData.db.Event.Where(c => c.OrganizeId == AppData.CurrentUser.Id).ToList();

            PagesCount = Math.Ceiling(Convert.ToDouble(currentServices.Count) / Convert.ToDouble(maxItemShow));
            LViewTours.ItemsSource = currentServices.Skip(maxItemShow * NumberOfPage).Take(maxItemShow).ToList();

            CheckPages();
        }

        private void CheckPages()
        {
            if (NumberOfPage > 0)
            {
                BtnPagePrev.IsEnabled = true;
            }
            else
            {
                BtnPagePrev.IsEnabled = false;
            }
            if (NumberOfPage < PagesCount - 1)
            {
                BtnPageNext.IsEnabled = true;
            }
            else
            {
                BtnPageNext.IsEnabled = false;
            }
        }

        private void BtnPageNext_Click(object sender, RoutedEventArgs e)
        {
            if (NumberOfPage + 1 < PagesCount)
            {
                NumberOfPage++;
                selectedPageTbx.Text = (NumberOfPage + 1).ToString();
                UpdateShop();
            }
        }

        private void BtnPagePrev_Click(object sender, RoutedEventArgs e)
        {
            if (NumberOfPage > 0)
            {
                NumberOfPage--;
                selectedPageTbx.Text = (NumberOfPage + 1).ToString();
                UpdateShop();
            }
        }
    }
}