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
    /// Логика взаимодействия для UserEventAddEditPage.xaml
    /// </summary>
    public partial class UserEventAddEditPage : Page
    {
        private Event Event = new Event();
        public UserEventAddEditPage(Event selectedEvent)
        {
            InitializeComponent();
            if (selectedEvent != null)
            {
                Event = selectedEvent;
                ComboEventType.SelectedItem = selectedEvent.EventType;
            }

            InitializeComponent();

            DataContext = Event;
            ComboEventType.ItemsSource = AppData.db.EventType.ToList();
            ComboEventStatus.ItemsSource = AppData.db.EventStatus.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Event.Name))
                errors.AppendLine("Укажите название");
            /*  if (string.IsNullOrWhiteSpace(service.Price.ToString()))
                  errors.AppendLine("Укажите цену");
              if (string.IsNullOrWhiteSpace(service.PeriodWork.ToString()))
                  errors.AppendLine("Укажите время выполнения(ч.)");*/
            if (ComboEventType.SelectedItem == null)
                errors.AppendLine("Укажите тип мероприятия");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (Event.Img == null)
            {

                Event.Img = "Views/Resources/notFound.png";
            }
            else
            {
                Event.Img = $"Views/Resources/{Event.Img}";
            }
            var currentServiceType = ComboEventType.SelectedItem as EventType;
            if (Event.Id == 0)
            {
                Event.TypeId = currentServiceType.Id;
                AppData.db.Event.Add(Event);
            }
            try
            {
                Event.OrganizeId = AppData.CurrentUser.Id;
                Event.TypeId = currentServiceType.Id;
                AppData.db.SaveChanges();
                MessageBox.Show("Данные сохранены");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}