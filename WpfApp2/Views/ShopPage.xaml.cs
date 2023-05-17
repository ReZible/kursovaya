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
    public partial class ShopPage : Page
    {
        private int PagesCount;
        private int NumberOfPage = 0;
        private int maxItemShow = 5;
        public ShopPage()
        {
            InitializeComponent();

            var allTypes = AppData.db.EventType.ToList();
            allTypes.Insert(0, new EventType
            {
                Type = "Все типы"
            });
            ComboType.ItemsSource = allTypes;

            ComboType.SelectedIndex = 0;

            var currentSrevices = AppData.db.Event.ToList();
            PagesCount = currentSrevices.Count / maxItemShow;
            LViewTours.ItemsSource = currentSrevices.Skip(maxItemShow*NumberOfPage).Take(maxItemShow).ToList();
        }
        private void UpdateShop()
        {
            var currentServices = AppData.db.Event.ToList();
            if (ComboType.SelectedIndex > 0)
                currentServices = currentServices.Where(c => c.EventType == ComboType.SelectedValue).ToList();

            currentServices = currentServices.Where(c => c.Name.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
         
            PagesCount = currentServices.Count / maxItemShow;
            LViewTours.ItemsSource = currentServices.Skip(maxItemShow * NumberOfPage).Take(maxItemShow).ToList();
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

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BtnPageNext_Click(object sender, RoutedEventArgs e)
        {
            if (NumberOfPage < PagesCount)
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