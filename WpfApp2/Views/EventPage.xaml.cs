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
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class EventPage : Page
    {
        private Event Event = new Event();
        public EventPage()
        {
            InitializeComponent();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                AppData.db.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());

                if (Event.Img == null)
                {

                    /*Event.Img = "Views/Resources/notFound.png";*/
                }
                else
                {
                    /*Event.Img = $"Views/Resources/{Event.Img}";*/
                }
                DGridMenu.ItemsSource = AppData.db.Event.ToList();
            }
        }

        private void SelectRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
                NavigationService.Navigate(new EventAddEditPage((sender as DataGridRow).DataContext as Event));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EventAddEditPage(null));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var servicesForRemoving = DGridMenu.SelectedItems.Cast<Event>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить следующие {servicesForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    AppData.db.Event.RemoveRange(servicesForRemoving);
                    AppData.db.SaveChanges();
                    MessageBox.Show("Данные удалены!");
                    DGridMenu.ItemsSource = AppData.db.Event.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
