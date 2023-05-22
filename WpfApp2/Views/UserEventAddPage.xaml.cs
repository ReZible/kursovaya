using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

            if (Event.Img != null)
            {
                try
                {
                    ImageService.Source = new ImageSourceConverter()
                        .ConvertFrom(Event.Img) as ImageSource;
                }
                catch (Exception ex)
                {
                    ImageService.Source = new ImageSourceConverter()
                        .ConvertFrom(File.ReadAllBytes("../../Resources/default_large.png")) as ImageSource;
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                try
                {
                    ImageService.Source = new ImageSourceConverter()
                        .ConvertFrom(File.ReadAllBytes("../../Resources/default_large.png")) as ImageSource;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
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
            var currentServiceType = ComboEventType.SelectedItem as EventType;
            if (Event.Id == 0)
            {
                Event.TypeId = currentServiceType.Id;
                AppData.db.Event.Add(Event);
            }
            try
            {
                Event.Name = new Regex(@"\s+").Replace(Event.Name, " ").Trim();
                Event.Description = new Regex(@"\s+").Replace(Event.Description, " ").Trim();
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
