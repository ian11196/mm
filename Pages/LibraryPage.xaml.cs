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

namespace GSBFLauncher.Pages
{
    /// <summary>
    /// Interaction logic for StorePage.xaml
    /// </summary>
    public partial class LibraryPage : Page
    {
        public LibraryPage()
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

        #region Build Library
        public void BuildLibrary(string libraryData)
        {
            CleanList();
            LibraryList[] js = JsonConvert.DeserializeObject<LibraryList[]>(libraryData);
            if (js != null)
            {
                foreach (LibraryList libraryList in js)
                {
                    LibraryItemCard libraryItem = new LibraryItemCard();

                    //ExtMethods.CopyProperties(GameCard, libraryItem);
                    libraryItem.ID = libraryList.ID;
                    libraryItem.Title = libraryList.Title;
                    libraryItem.URL = libraryList.URL;
                    libraryItem.Genre = "Action";
                    System.Diagnostics.Debug.WriteLine(libraryList.Thumbnail);
                    // Thumbnail Values
                    var image = new Image();
                    var fullFilePath = @libraryList.Thumbnail;
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
                    bitmap.EndInit();
                    image.Source = bitmap;
                    libraryItem.ImageSource = image.Source;
                    LibraryStack.Children.Add(libraryItem);
                }
            }
        }

        void CleanList()
        {
            LibraryStack.Children.Clear();
        }
        #endregion

    }
}
