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
    /// Логика взаимодействия для ShopPage.xaml
    /// </summary>
    public partial class ShowEventsPage : Page
    {
        private double PagesCount;
        private int NumberOfPage = 0;
        private int maxItemShow = 5;
        public ShowEventsPage()
        {
            InitializeComponent();

            var allTypes = AppData.db.EventType.ToList();
            allTypes.Insert(0, new EventType
            {
                Type = "Все типы"
            });
            ComboType.ItemsSource = allTypes;

            ComboType.SelectedIndex = 0;

            var allStatus = AppData.db.EventStatus.ToList();
            allStatus.Insert(0, new EventStatus
            {
                Status = "Все статусы"
            });

            ComboStatus.ItemsSource = allStatus;

            ComboStatus.SelectedIndex = 0;

            var currentServices = AppData.db.Event.ToList();
            PagesCount = Math.Ceiling(Convert.ToDouble(currentServices.Count) / Convert.ToDouble(maxItemShow));
            LViewTours.ItemsSource = currentServices.Skip(maxItemShow*NumberOfPage).Take(maxItemShow).ToList();
        }
        private void UpdateShop()
        {
            var currentServices = AppData.db.Event.ToList();
            if (ComboType.SelectedIndex > 0)
                currentServices = currentServices.Where(c => c.EventType == ComboType.SelectedValue).ToList();
            if (ComboStatus.SelectedIndex > 0)
                currentServices = currentServices.Where(c => c.EventStatus == ComboStatus.SelectedValue).ToList();

            currentServices = currentServices.Where(c => c.Name.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            PagesCount = Math.Ceiling(Convert.ToDouble(currentServices.Count) / Convert.ToDouble(maxItemShow));
            LViewTours.ItemsSource = currentServices.Skip(maxItemShow *NumberOfPage).Take(maxItemShow).ToList();

            CheckPages();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            NumberOfPage = 0;
            selectedPageTbx.Text = "1";
            UpdateShop();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NumberOfPage = 0;
            selectedPageTbx.Text = "1";
            UpdateShop();
        }

        private void BtnPageNext_Click(object sender, RoutedEventArgs e)
        {
            if (NumberOfPage+1 < PagesCount)
            {
                NumberOfPage++;
                selectedPageTbx.Text = (NumberOfPage+1).ToString();
                UpdateShop();
            }
        }

        private void BtnPagePrev_Click(object sender, RoutedEventArgs e)
        {
            if (NumberOfPage > 0)
            {
                NumberOfPage--;
                selectedPageTbx.Text = (NumberOfPage+1).ToString();
                UpdateShop();
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserEventAddPage(null));
        }

        private void ListViewItem_LeftMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListViewItem).DataContext as Event;
            if(item != null)
            {
                NavigationService.Navigate(new EventShowDetailsPage(item));
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                AppData.db.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                var allTypes = AppData.db.EventType.ToList();
                allTypes.Insert(0, new EventType
                {
                    Type = "Все типы"
                });
                ComboType.ItemsSource = allTypes;

                ComboType.SelectedIndex = 0;

                var currentServices = AppData.db.Event.ToList();
                PagesCount = Math.Ceiling(Convert.ToDouble(currentServices.Count) / Convert.ToDouble(maxItemShow));
                LViewTours.ItemsSource = currentServices.Skip(maxItemShow * NumberOfPage).Take(maxItemShow).ToList();
            }
        }

        private void ComboStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NumberOfPage = 0;
            selectedPageTbx.Text = "1";
            UpdateShop();
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
            if (NumberOfPage < PagesCount-1)
            {
                BtnPageNext.IsEnabled = true;
            }
            else
            {
                BtnPageNext.IsEnabled = false;
            }
        }
    }
}