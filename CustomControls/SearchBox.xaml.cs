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
using GSBFLauncher.Pages;
using GSBFLauncher.CustomClasses;

namespace GSBFLauncher.CustomControls
{
    /// <summary>
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl
    {
        private bool SearchBoxFocused = false;
        public string Text { get; set; }

        public SearchBox()
        {
            InitializeComponent();
        }

        private void SearchPressed(object sender, RoutedEventArgs e)
        {
            SearchPage searchPage = new SearchPage();
            if (SearchBoxFocused)
            {
                System.Diagnostics.Debug.WriteLine(Text);

                Windows.LauncherPage.LauncherFrame.Navigate(searchPage, UriKind.RelativeOrAbsolute);
                searchPage.DoSearch(Text);
                Windows.UnSelectAll();
            }
            else
                System.Diagnostics.Debug.WriteLine("not focused");
        }

        #region Search Box Code Block
        private void OnSearchFocused(object sender, RoutedEventArgs e)
        {
            SearchBoxFocused = true;
        }
        private void OnSearchLostFocus(object sender, RoutedEventArgs e)
        {
            SearchBoxFocused = false;
        }
        #endregion

        private void Text_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox t = (TextBox)sender;
            Text = t.Text;
        }
    }
}
