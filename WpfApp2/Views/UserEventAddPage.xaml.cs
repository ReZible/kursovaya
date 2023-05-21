using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для UserEventAddPage.xaml
    /// </summary>
    public partial class UserEventAddPage : Page
    {
        private Event Event = new Event();
        private byte[] _mainImageData = null;
        public UserEventAddPage(Event selectedEvent)
        {
            InitializeComponent();
            if (selectedEvent != null)
            {
                Event = selectedEvent;
                ComboEventType.SelectedItem = selectedEvent.EventType;
            }

            DataContext = Event;
            ComboEventType.ItemsSource = AppData.db.EventType.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if(string.IsNullOrWhiteSpace(Event.Name))
                errors.AppendLine("Укажите название");
            if (string.IsNullOrWhiteSpace(Event.Description))
                errors.AppendLine("Укажите описание");
            if (_mainImageData == null)
                errors.AppendLine("Укажите изображение");
            if (ComboEventType.SelectedItem == null)
                errors.AppendLine("Укажите тип мероприятия");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (Event.Img == null)
            {

                /*Event.Img = "Views/Resources/notFound.png";*/
            }
            else
            {
                /*Event.Img = $"Views/Resources/{Event.Img}";*/
            }
            var currentServiceType = ComboEventType.SelectedItem as EventType;
            if (Event.Id == 0)
            {
                Event.TypeId = currentServiceType.Id;
                AppData.db.Event.Add(Event);
            }
            try
            {
                Event.StatusId = 1;
                Event.Img = _mainImageData;
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

        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            openFileDialog.Title = "Выберите изображение";

            if (openFileDialog.ShowDialog() == true)
            {
                _mainImageData = File.ReadAllBytes(openFileDialog.FileName);
                ImageService.Source = new ImageSourceConverter()
                    .ConvertFrom(_mainImageData) as ImageSource;
            }
        }
    }
}
