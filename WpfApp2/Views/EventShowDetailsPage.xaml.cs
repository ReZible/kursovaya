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

namespace WpfApp2.Views
{
    /// <summary>
    /// Логика взаимодействия для EventShowDetailsPage.xaml
    /// </summary>
    public partial class EventShowDetailsPage : Page
    {
        private Event Event = new Event();
        private EventUsers EventsUsers = new EventUsers();
        private byte[] _mainImageData = null;
        public EventShowDetailsPage(Event selectedType)
        {
            InitializeComponent();
            Event = selectedType;
            _mainImageData = Event.Img;
            DataContext = Event;
            ComboEventType.ItemsSource = AppData.db.EventType.ToList();
            ComboEventStatus.ItemsSource = AppData.db.EventStatus.ToList();
        }

        private void BtnTakePart_Click(object sender, RoutedEventArgs e)
        {
            var isMember = AppData.db.EventUsers.FirstOrDefault(u => u.EventId == Event.Id && u.UserId == AppData.CurrentUser.Id);

            if( isMember == null )
            {
                EventsUsers.EventId = Event.Id;
                EventsUsers.UserId = AppData.CurrentUser.Id;
                AppData.db.EventUsers.Add(EventsUsers);
                AppData.db.SaveChanges();
                MessageBox.Show("Вы приняли участие");
                BtnTakePart.IsEnabled = false;
                TblMember.Visibility = Visibility.Visible;
            } 
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            /*NavigationService.Navigate(new UserEventAddEditPage(Event));*/
            BtnSave.Visibility = Visibility.Visible;
            BtnEdit.IsEnabled = false;
            TbDescription.IsEnabled = true;
            TbName.IsEnabled = true;
            ComboEventStatus.IsEnabled = true;
            ComboEventType.IsEnabled = true;
            TblImg.Visibility = Visibility.Visible;
            btnSelectImage.Visibility = Visibility.Visible;
        }

        private void CheckIsOrganize(object sender, RoutedEventArgs e)
        {
            if (AppData.CurrentUser.Id == Event.OrganizeId)
            {
                BtnTakePart.Visibility = Visibility.Collapsed;
            } else
            {
                BtnEdit.Visibility = Visibility.Collapsed;
            }

            if(Event.StatusId == 2)
            {
                BtnEdit.IsEnabled = false;
                BtnTakePart.IsEnabled = false;
            }

            var isMember = AppData.db.EventUsers.FirstOrDefault(u => u.EventId == Event.Id && u.UserId == AppData.CurrentUser.Id);

            if( isMember != null)
            {
                BtnTakePart.IsEnabled = false;
                TblMember.Visibility = Visibility.Visible;
            }
            
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(Event.Name))
                errors.AppendLine("Укажите название");
            if (string.IsNullOrWhiteSpace(Event.Description))
                errors.AppendLine("Укажите описание");
            if (_mainImageData == null)
                errors.AppendLine("Укажите изображение");
            if (ComboEventStatus.SelectedItem == null)
                errors.AppendLine("Укажите статус мероприятия");
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
                Event.Img = _mainImageData;
                Event.OrganizeId = AppData.CurrentUser.Id;
                Event.TypeId = currentServiceType.Id;
                AppData.db.SaveChanges();
                MessageBox.Show("Данные сохранены");

                TbDescription.Text = Event.Description;
                TbName.Text = Event.Name;
                BtnSave.Visibility = Visibility.Hidden;
                TbDescription.IsEnabled = false;
                TbName.IsEnabled = false;
                ComboEventStatus.IsEnabled = false;
                ComboEventType.IsEnabled = false;
                TblImg.Visibility = Visibility.Collapsed;
                btnSelectImage.Visibility = Visibility.Collapsed;

                if (Event.StatusId == 2)
                {
                    BtnEdit.IsEnabled = false;
                } else
                {
                    BtnEdit.IsEnabled = true;
                }
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
