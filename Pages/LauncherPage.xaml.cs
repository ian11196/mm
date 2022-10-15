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

namespace GSBFLauncher.Pages
{
    /// <summary>
    /// Interaction logic for StorePage.xaml
    /// </summary>
    public partial class LauncherPage : Page
    {
        public bool SearchBoxFocused = false;

        public LauncherPage()
        {
            InitializeComponent();
            Windows.LauncherPage = this;
            //Windows.StorePage.LoadStoreData();
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

        private void StoreButton_Pressed(object sender, RoutedEventArgs e)
        {
            Runtime.FadeIn(LauncherFrame, 0, 1, 0.15);
            LauncherFrame.Navigate(Windows.StorePage, UriKind.RelativeOrAbsolute);
        }

        private void LibraryButton_Pressed(object sender, RoutedEventArgs e)
        {
            Runtime.FadeIn(LauncherFrame, 0, 1, 0.15);
            LauncherFrame.Navigate(Windows.LibraryPage, UriKind.RelativeOrAbsolute);
        }

        private void PublishButton_Pressed(object sender, RoutedEventArgs e)
        {
            Runtime.FadeIn(LauncherFrame, 0, 1, 0.15);
            LauncherFrame.Navigate(Windows.PublishPage, UriKind.RelativeOrAbsolute);
        }
    }
}
