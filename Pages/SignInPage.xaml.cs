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
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GSBFLauncher.CustomClasses;

namespace GSBFLauncher.Pages
{
    /// <summary>
    /// Interaction logic for SignInPage.xaml
    /// </summary>
    public partial class SignInPage : Page
    {
        public SignInPage()
        {
            InitializeComponent();
        }

        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            if ((Application.Current.MainWindow != null))
                ((MainWindow) Application.Current.MainWindow).MainFrame.Navigate(
                    new Uri("../Pages/PreparingToLaunchStore.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if ((Application.Current.MainWindow != null))
                ((MainWindow)Application.Current.MainWindow).MainFrame.Navigate(
                    new Uri("../Pages/SettingsPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void LoginServiceButtons_Click(object sender, RoutedEventArgs e)
        {

        }

        #region Login Button Click Event
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsernameField.Text != "" || PasswordField.CypherText != "")
                Login(UsernameField.Text, PasswordField.CypherText);
            else
            {
                ErrorMessage.Text = "Please input all details";
                ErrorMessage.Visibility = Visibility.Visible;
            }
        }
        #endregion

        #region Do Login
        public async void Login(string username, string password)
        {
            try
            {
                LoadingPanel.Visibility = Visibility.Visible;

                var dict = new Dictionary<string, string>();
                dict.Add("type", "2");
                dict.Add("username", username);
                dict.Add("password", password);
                var client = new HttpClient();
                var req = new HttpRequestMessage(HttpMethod.Post, URLs.AccountURL) { Content = new FormUrlEncodedContent(dict) };
                var res = await client.SendAsync(req);
                string jsonString = res.Content.ReadAsStringAsync().Result;

                if (jsonString != "err")
                    LoginSuccess(jsonString);
                else
                    LoginFailed(false);
            }
            catch
            {
                Console.WriteLine("server dead");
                LoginFailed(true);
            }
        }
        #endregion

        #region On Login Success
        private void LoginSuccess(string jsonResponse)
        {
            ErrorMessage.Visibility = Visibility.Hidden;

            JObject results = JObject.Parse(jsonResponse);
            UserData.ID = results["ID"].ToString();
            UserData.Username = UsernameField.Text;
            UserData.Email = results["email"].ToString();
            UserData.NickName = results["nickname"].ToString();
            UserData.Avatar = results["avatar"].ToString();
            UserData.Library = results["library"].ToString();
            UserData.XP = results["xp"].ToString();
            UserData.flist = results["friends"].ToString();
            //StaticClasses.Windows.library.BuildLibrary(UserData.Library);

            Console.WriteLine("name: " + UserData.NickName);
            Properties.Settings.Default.Username = UsernameField.Text;
            LoadingPanel.Visibility = Visibility.Hidden;

            Windows.LibraryPage.BuildLibrary(UserData.Library);

            if ((Application.Current.MainWindow != null))
                Windows.mainWindow.MainFrame.Navigate(
                    new Uri("../Pages/PreparingToLaunchStore.xaml", UriKind.RelativeOrAbsolute));
        }
        #endregion

        #region On Login Fail
        private void LoginFailed(bool exception)
        {
            if (exception)
            {
                ErrorMessage.Visibility = Visibility.Visible;
                ErrorMessage.Text = "Oops! Failed connecting to server.";
                LoadingPanel.Visibility = Visibility.Hidden;

                LoginButton.IsEnabled = true;
            }
            else
            {
                ErrorMessage.Visibility = Visibility.Visible;
                ErrorMessage.Text = "Incorrect username or password!";
                LoadingPanel.Visibility = Visibility.Hidden;

                LoginButton.IsEnabled = true;
            }
        }
        #endregion

        #region SignUp Page Button Click Event
        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = Visibility.Hidden;
            RegisterPanel.Visibility = Visibility.Visible;
            LoginButton.IsDefault = false;
            RegisterButton.IsDefault = true;
            UsernameInput.Focus();
        }
        #endregion

        #region Sign In Page Button Click Event
        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = Visibility.Visible;
            RegisterPanel.Visibility = Visibility.Hidden;
            LoginButton.IsDefault = true;
            RegisterButton.IsDefault = false;
        }
        #endregion

        #region Register Button Click Event
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckFields())
            {
                ErrorMessageReg.Text = "Please input all details.";
                ErrorMessageReg.Visibility = Visibility.Visible;
            }
            else
                Register(UsernameField.Text, NicknameInput.Text, EmailInput.Text, PasswordField.CypherText);
        }
        #endregion

        #region Do Register
        public async void Register(string username, string nickname, string email, string password)
        {
            try
            {
                ErrorMessageReg.Visibility = Visibility.Hidden;
                Loader.AnimationType = Syncfusion.Windows.Controls.Notification.AnimationTypes.Pen;
                LoadingPanel.Visibility = Visibility.Visible;

                var dict = new Dictionary<string, string>();
                dict.Add("type", "0");
                dict.Add("username", username);
                dict.Add("nickname", nickname);
                dict.Add("email", email);
                dict.Add("password", password);
                var client = new HttpClient();
                var req = new HttpRequestMessage(HttpMethod.Post, URLs.AccountURL) { Content = new FormUrlEncodedContent(dict) };
                var res = await client.SendAsync(req);
                string jsonString = res.Content.ReadAsStringAsync().Result;
                System.Diagnostics.Debug.WriteLine(jsonString);
                if (jsonString == "005")
                    RegisterFailed(false);
                else
                    RegisterSuccess(jsonString);
            }
            catch
            {
                Console.WriteLine("server dead");
                RegisterFailed(true);
            }
        }
        #endregion

        #region On Register Success
        private void RegisterSuccess(string jsonResponse)
        {
            LoadingPanel.Visibility = Visibility.Hidden;
            Loader.AnimationType = Syncfusion.Windows.Controls.Notification.AnimationTypes.Flower;

            ErrorMessageReg.Visibility = Visibility.Visible;
            ErrorMessageReg.Text = "Registration successful! Login now.";
            ErrorMessageReg.Foreground = new SolidColorBrush(Colors.Green);
        }
        #endregion

        #region On Register Fail
        private void RegisterFailed(bool exception)
        {
            if (exception)
            {
                ErrorMessageReg.Visibility = Visibility.Visible;
                ErrorMessageReg.Foreground = (Brush)(FindResource("TertiaryBrush"));
                ErrorMessageReg.Text = "Oops! Failed connecting to server.";
                Loader.AnimationType = Syncfusion.Windows.Controls.Notification.AnimationTypes.Flower;
                LoadingPanel.Visibility = Visibility.Hidden;

                LoginButton.IsEnabled = true;
            }
            else
            {
                ErrorMessageReg.Visibility = Visibility.Visible;
                ErrorMessageReg.Foreground = (Brush)(FindResource("TertiaryBrush"));
                ErrorMessageReg.Text = "Username or email already exists.";
                Loader.AnimationType = Syncfusion.Windows.Controls.Notification.AnimationTypes.Flower;
                LoadingPanel.Visibility = Visibility.Hidden;
            }
        }
        #endregion

        #region Validation Checks
        private bool CheckFields()
        {
            if (UsernameField.Text != "" || NicknameInput.Text != "" || EmailInput.Text != "" || PasswordField.CypherText != "")
                return false;
            else
                return true;
        }

        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
        #endregion

    }
}
