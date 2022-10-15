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
using WooCommerceNET.WooCommerce.v2;
using WooCommerceNET;
using WordpressRestApi;
using WordPressPCL;
using WordPressPCL.Models;

namespace GSBFLauncher.Pages
{
    /// <summary>
    /// Interaction logic for StorePage.xaml
    /// </summary>
    public partial class PublishPage : System.Windows.Controls.Page
    {
        private BitmapImage frontImage;
        private string img1 = null;
        private string img2 = null;
        private string img3 = null;
        private string img4 = null;
        private string img5 = null;
        private string img6 = null;

        public PublishPage()
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

        private void Upload_FrontImage(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            var codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            var codecFilter = "Image Files|";
            foreach (var codec in codecs)
            {
                codecFilter += codec.FilenameExtension + ";";
            }
            dlg.Filter = codecFilter;

            var result = dlg.ShowDialog();

            if (result.Value)
            {
                frontImage = new BitmapImage(new Uri(dlg.FileName));
                FrontImageButton.Content = dlg.FileName;
            }
        }
        private void Upload_Images(object sender, RoutedEventArgs e)
        {
            Button _button = (Button)sender;
            
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            
            var codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            var codecFilter = "Image Files|";
            foreach (var codec in codecs)
            {
                codecFilter += codec.FilenameExtension + ";";
            }
            dlg.Filter = codecFilter;

            var result = dlg.ShowDialog();

            if (result.Value)
            {                
                switch (int.Parse(_button.Tag.ToString()))
                {
                    case 0:
                        img1 = new BitmapImage(new Uri(dlg.FileName)).UriSource.AbsolutePath;
                        break;
                    case 1:
                        img2 = new BitmapImage(new Uri(dlg.FileName)).UriSource.AbsolutePath;
                        break;
                    case 2:
                        img3 = new BitmapImage(new Uri(dlg.FileName)).UriSource.AbsolutePath;
                        break;
                    case 3:
                        img4 = new BitmapImage(new Uri(dlg.FileName)).UriSource.AbsolutePath;
                        break;
                    case 4:
                        img5 = new BitmapImage(new Uri(dlg.FileName)).UriSource.AbsolutePath;
                        break;
                    case 5:
                        img6 = new BitmapImage(new Uri(dlg.FileName)).UriSource.AbsolutePath;
                        break;

                }

                _button.Content = dlg.FileName;
            }
        }
        public async void SubmitGame(object sender, RoutedEventArgs e)
        {
            if (CanSubmit())
            {
                Windows.LauncherPage.LoadingPanel.Visibility = Visibility.Visible;
                var pathFile = frontImage.UriSource.AbsolutePath;
                byte[] fImageBytes = File.ReadAllBytes(pathFile);

                MultipartFormDataContent form = new MultipartFormDataContent();
                var httpClient = new HttpClient();

                form.Add(new StringContent(UserData.ID), "uuid");
                form.Add(new StringContent(PublisherName.Text), "pubname");
                form.Add(new StringContent(GameTitle.Text), "gametitle");
                form.Add(new StringContent(URL.Text), "url");
                form.Add(new StringContent(Genre.Text), "genre");
                form.Add(new StringContent(ShortDescription.Text), "shortdesc");
                form.Add(new StringContent(LongDescription.Text), "longdesc");
                form.Add(new StringContent(PrivacyPolicy.Text), "pp");
                form.Add(new StringContent(SystemRequirements.Text), "requirements");

                form.Add(new ByteArrayContent(fImageBytes), "frontimg");
                form.Add(new ByteArrayContent(File.ReadAllBytes(img1)), "img1");
                form.Add(new ByteArrayContent(File.ReadAllBytes(img2)), "img2");
                form.Add(new ByteArrayContent(File.ReadAllBytes(img3)), "img3");
                form.Add(new ByteArrayContent(File.ReadAllBytes(img4)), "img4");
                form.Add(new ByteArrayContent(File.ReadAllBytes(img5)), "img5");
                form.Add(new ByteArrayContent(File.ReadAllBytes(img6)), "img6");

                var response = await httpClient.PostAsync(URLs.PublishURL, form);
                response.EnsureSuccessStatusCode();
                string sd = response.Content.ReadAsStringAsync().Result;
                JObject res = JObject.Parse(sd);
                int code = (int)res["code"];
                string message = (string)res["message"];
                if (code == 200)
                {
                    GameProductData data = new GameProductData();
                    data.Name = GameTitle.Text;
                    data.Description = LongDescription.Text;
                    data.ImageURL = message;
                    CreateGame(data);

                    Windows.LauncherPage.LoadingPanel.Visibility = Visibility.Hidden;
                    SubmissionForm.Visibility = Visibility.Hidden;
                    SuccessMessage.Visibility = Visibility.Visible;
                    ErrorMessage.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Windows.LauncherPage.LoadingPanel.Visibility = Visibility.Hidden;
                    SubmissionForm.Visibility = Visibility.Visible;
                    SuccessMessage.Visibility = Visibility.Hidden;
                    ErrorMessage.Text = "Submission error: " + sd;
                    ErrorMessage.Visibility = Visibility.Visible;
                }

                System.Diagnostics.Debug.WriteLine(sd);
            }
            else
            {
                ErrorMessage.Text = "Please input all fields.";
                ErrorMessage.Visibility = Visibility.Visible;
                return;
            }
        }

        public void CreateGame(GameProductData data)
        {
            Product product = new Product();
            product.name = data.Name;
            product.regular_price = 0;
            product.in_stock = false;
            product._virtual = true;
            product.downloadable = true;

            List<ProductDownloadLine> productDownloads = new List<ProductDownloadLine>();
            productDownloads.Add(new ProductDownloadLine
            {
                name = data.Name,
                file = "https://beyondinfinitystudio.com/wp-content/uploads/2021/12/Build.zip"
            });

            product.downloads = productDownloads;

            List<ProductImage> productImageList = new List<ProductImage>();

            productImageList.Add(new ProductImage()
            {
                name = "front",
                src = data.ImageURL,
                position = 0
            });
            product.images = productImageList;
            Runtime.WC.Product.Add(product).GetAwaiter().GetResult();
        }

        public bool CanSubmit()
        {
            bool _canSubmit = UserData.ID != null && PublisherName.Text != null && GameTitle.Text != null && URL.Text != null && Genre.Text != "-" && ShortDescription.Text != null && LongDescription.Text != null && PrivacyPolicy.Text != null && SystemRequirements.Text != null && img1 != null && img2 != null && img3 != null && img4 != null && img5 != null && img6 != null;

            if (_canSubmit)
                return true;
            else
                return false;
        }
    }
}
public class GameProductData
{
    public string Name;
    public string Description;
    public string ImageURL;
}
