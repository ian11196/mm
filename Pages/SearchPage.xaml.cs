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
using GSBFLauncher.CustomClasses;
using GSBFLauncher.CustomControls;

namespace GSBFLauncher.Pages
{
    /// <summary>
    /// Interaction logic for StorePage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        public SearchPage()
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

        public void DoSearch(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
            Windows.LauncherPage.LoadingPanel.Visibility = Visibility.Visible;
            var products = Runtime.WC.Product.GetAll().GetAwaiter().GetResult();
            Runtime.StoreList = products;
            var searchList = products.FindAll(item => item.name.StartsWith(text, StringComparison.InvariantCultureIgnoreCase));
            BuildStoreListing(searchList);
            Windows.LauncherPage.LoadingPanel.Visibility = Visibility.Hidden;
        }

        #region Build Library
        public void BuildStoreListing(List<WooCommerceNET.WooCommerce.v2.Product> games)
        {
            CleanList();
            if (games.Count > 0)
            {
                foreach (var storeGame in games)
                {
                    if (storeGame.in_stock.Value)
                    {
                        LibraryItemCard storeItem = new LibraryItemCard();

                        storeItem.ID = storeGame.id.ToString();
                        storeItem.Title = storeGame.name;
                        storeItem.URL = storeGame.external_url;
                        storeItem.Genre = "Action";
                        // Thumbnail Values
                        var image = new Image();
                        var fullFilePath = @storeGame.images[0].src;
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
                        bitmap.EndInit();
                        image.Source = bitmap;
                        storeItem.ImageSource = image.Source;
                        StoreStack.Children.Add(storeItem);
                    }
                }
            }
        }

        void CleanList()
        {
            StoreStack.Children.Clear();
        }
        #endregion

    }
}
