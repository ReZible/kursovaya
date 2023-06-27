using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для EventConfiramationShowPage.xaml
    /// </summary>
    public partial class EventConfiramationShowPage : Page
    {
        private Event Event = new Event();
        private EventUsers EventsUsers = new EventUsers();
        private byte[] _mainImageData = null;
        public EventConfiramationShowPage(Event selectedType)
        {
            InitializeComponent();
            Event = selectedType;
            _mainImageData = Event.Img;
            DataContext = Event;
            ComboEventType.ItemsSource = AppData.db.EventType.ToList();
            ComboEventStatus.ItemsSource = AppData.db.EventStatus.ToList();
        }
        private void BtnApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Event.StatusId = 1;
                AppData.db.SaveChanges();
                MessageBox.Show("Мероприятие одобрено!");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void BtnReject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Event.StatusId = 4;
                AppData.db.SaveChanges();
                MessageBox.Show("Мероприятие отклонено!");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
