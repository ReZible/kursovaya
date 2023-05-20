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
    /// Логика взаимодействия для EventShowDetailsPage.xaml
    /// </summary>
    public partial class EventShowDetailsPage : Page
    {
        private Event Event = new Event();
        private EventUsers EventsUsers = new EventUsers();
        public EventShowDetailsPage(Event selectedType)
        {
            InitializeComponent();
            Event = selectedType;
            DataContext = Event;
        }

        private void btnTakePart_Click(object sender, RoutedEventArgs e)
        {
            var isMember = AppData.db.EventUsers.FirstOrDefault(u => u.EventId == Event.Id && u.UserId == AppData.CurrentUser.Id);

            if( isMember == null )
            {
                EventsUsers.EventId = Event.Id;
                EventsUsers.UserId = AppData.CurrentUser.Id;
                AppData.db.EventUsers.Add(EventsUsers);
                AppData.db.SaveChanges();
                MessageBox.Show("Вы приняли участие");
            } else
            {
                MessageBox.Show("Вы уже в списке усчастников");
                btnTakePart.IsEnabled = false;
            }
  
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UserEventAddEditPage(Event));
        }

        private void CheckIsOrganize(object sender, RoutedEventArgs e)
        {
            if (AppData.CurrentUser.Id == Event.OrganizeId)
            {
                btnTakePart.Visibility = Visibility.Collapsed;
            } else
            {
                btnEdit.Visibility = Visibility.Collapsed;
            }

            if(Event.StatusId == 2)
            {
                btnEdit.IsEnabled = false;
                btnTakePart.IsEnabled = false;
            }
        }
    }
}
