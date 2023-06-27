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
        private int maxItemShow = 5;
        private int currentPageNumber = 1;
        private int pagesCount;

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

            var allStatus = AppData.db.EventStatus.Where(e => e.Id != 3 && e.Id != 4).ToList();
            allStatus.Insert(0, new EventStatus
            {
                Status = "Все статусы"
            });

            ComboStatus.ItemsSource = allStatus;

            ComboStatus.SelectedIndex = 0;

            UpdateEvent();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            currentPageNumber = 1;
            selectedPageTbx.Text = "1";
            UpdateEvent();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentPageNumber = 1;
            selectedPageTbx.Text = "1";
            UpdateEvent();
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

        private void UpdateEvent()
        {
            var currentServices = AppData.db.Event.Where(a => a.StatusId == 1 || a.StatusId == 2).ToList();
            if (ComboType.SelectedIndex > 0)
                currentServices = currentServices.Where(c => c.EventType == ComboType.SelectedValue).ToList();
            if (ComboStatus.SelectedIndex > 0)
                currentServices = currentServices.Where(c => c.EventStatus == ComboStatus.SelectedValue).ToList();

            currentServices = currentServices.Where(c => c.Name.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

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

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserEventAddPage(null));
        }

        private void ListViewItem_LeftMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListViewItem).DataContext as Event;
            if (item != null)
            {
                NavigationService.Navigate(new EventShowDetailsPage(item));
            }
        }

        private void ComboStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentPageNumber = 0;
            selectedPageTbx.Text = "1";
            UpdateEvent();
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

                UpdateEvent();
            }
        }
    }
}