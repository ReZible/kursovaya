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
    /// Логика взаимодействия для EventConfirmationPage.xaml
    /// </summary>
    public partial class EventConfirmationPage : Page
    {
        private int maxItemShow = 5;
        private int currentPageNumber = 1;
        private int pagesCount;
        public EventConfirmationPage()
        {
            InitializeComponent();
        }

        private void UpdateEvent()
        {
            var currentServices = AppData.db.Event.Where(a => a.StatusId == 3).ToList();
            var count = currentServices.Count();
            pagesCount = (int)Math.Ceiling((double)count / maxItemShow);
            LViewEvents.ItemsSource = currentServices
                .Skip((currentPageNumber - 1) * maxItemShow)
                .Take(maxItemShow).ToList();

            CheckPages();
            if (currentServices.Count() > 0)
            {   
                TblZeroEvent.Visibility = Visibility.Hidden;
            } else
            {
                TblZeroEvent.Visibility = Visibility.Visible;
            }
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
        private void CheckPages()
        {
            BtnPagePrev.IsEnabled = currentPageNumber > 1;
            BtnPageNext.IsEnabled = currentPageNumber < pagesCount;
        }

        private void ListViewItem_LeftMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListViewItem).DataContext as Event;
            if (item != null)
            {
                NavigationService.Navigate(new EventConfiramationShowPage(item));
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                UpdateEvent();
            }
        }
    }
}
