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
            var currentServices = AppData.db.Event.ToList();
            PagesCount = Math.Ceiling(Convert.ToDouble(currentServices.Count) / Convert.ToDouble(maxItemShow));
            LViewTours.ItemsSource = currentServices.Skip(maxItemShow * NumberOfPage).Take(maxItemShow).ToList();
        }

        private void LBMyTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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

        }
    }
}
