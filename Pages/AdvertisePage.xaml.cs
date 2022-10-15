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
using GSBFLauncher.CustomControls;
using GSBFLauncher.CustomClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;

namespace GSBFLauncher.Pages
{
    /// <summary>
    /// Interaction logic for StorePage.xaml
    /// </summary>
    public partial class AdvertisePage : Page
    {
        private string publisherName;
        private BitmapImage frontImage;
        public AdvertisePage()
        {
            InitializeComponent();
            
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if ((Application.Current.MainWindow != null))
                ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(
                    new Uri("../Pages/SignInPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void BtnSettings_OnClick(object sender, RoutedEventArgs e)
        {
            if ((Application.Current.MainWindow != null))
                ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(
                    new Uri("../Pages/SettingsPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void SearchBox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private async void Upload_FrontImage(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            Nullable<bool> result = dlg.ShowDialog();

            if (result.HasValue)
            {
                FrontImage.Source = new BitmapImage(new Uri(dlg.FileName));
                frontImage = (BitmapImage)FrontImage.Source;

                var bitmap = new BitmapImage(new Uri(dlg.FileName));
                var uri = bitmap.UriSource;
                var path = uri.AbsolutePath;
            }
        }

        public async void SubmitGame(object sender, RoutedEventArgs e)
        {
            var pathFile = frontImage.UriSource.AbsolutePath;
            byte[] fImageBytes = File.ReadAllBytes(pathFile);

            MultipartFormDataContent form = new MultipartFormDataContent();
            var httpClient = new HttpClient();

            form.Add(new StringContent(UserData.ID), "uuid");
            form.Add(new StringContent(PublisherName.Text), "pubname");
            form.Add(new StringContent(URL.Text), "url");
            form.Add(new StringContent(Genre.Text), "genre");
            form.Add(new StringContent(ShortDescription.Text), "shortdesc");
            form.Add(new StringContent(LongDescription.Text), "longdesc");
            form.Add(new StringContent(PrivacyPolicy.Text), "pp");
            form.Add(new ByteArrayContent(fImageBytes), "frontimg");

            var response = await httpClient.PostAsync(URLs.PublishURL, form);
            response.EnsureSuccessStatusCode();
            string sd = response.Content.ReadAsStringAsync().Result;

            System.Diagnostics.Debug.WriteLine(sd);
        }
    }
}
