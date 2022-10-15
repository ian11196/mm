using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml;
using System.Reflection;
using Newtonsoft.Json;
using System.Windows.Media;
//using TestServerClientPackets.ExamplePacketsTwo;
using GSBFLauncher.Pages;
using GSBFLauncher;
using WooCommerceNET.WooCommerce.v2;
using WooCommerceNET;


namespace GSBFLauncher.CustomClasses
{
    public enum ActivityState { Online, Playing };

    #region URLs and Directory Definitions
    public static class URLs
    {
        public const string AccountURL = "https://beyondinfinitystudio.com/cradmin/test.php";
        public const string StoreSearchURL = "https://beyondinfinitystudio.com/cradmin/store.php";
        public const string PublishURL = "https://beyondinfinitystudio.com/cradmin/publish.php";

        public static readonly string GAME_TITLE = "RuffyWorld";
        public static readonly string LAUNCHER_NAME = "Ruffy Launcher";

        public static readonly string DESTINATION_PATH = Path.Combine(Directory.GetCurrentDirectory(), GAME_TITLE);
        public static readonly string LIBRARY_PATH = Path.Combine(Directory.GetCurrentDirectory(), "Library");
        public static readonly string DOWNLOAD_PATH = Path.Combine(LIBRARY_PATH, "downloading");
        public static readonly string ZIP_PATH = Path.Combine(LIBRARY_PATH, GAME_TITLE + ".zip");
        //public static readonly string GAME_EXECUTABLE_PATH = Path.Combine(LIBRARY_PATH, GAME_TITLE + ".exe");
        public static readonly string CLIENT_DOWNLOAD_URL = "https://beyondinfinitystudio.com/wp-content/uploads/2021/12/Build.zip";
        public static readonly string VERSION_URL = "https://beyondinfinitystudio.com/wp-content/uploads/2021/12/version.txt";

        public static string GAME_PATH(string Name)
        {
            return Path.Combine(LIBRARY_PATH, Name);
        }

        public static string GAME_DOWNLOAD_PATH(string Name)
        {
            return Path.Combine(DOWNLOAD_PATH, Name + ".zip");
        }

        public static string GAME_EXECUTABLE_PATH(string Name)
        {
            return Path.Combine(GAME_PATH(Name), Name + ".exe");
        }
    }
    #endregion

    #region Networking Manager
    public static class Networking
    {
        //public static NetworkManager NetworkManager = new NetworkManager();
    }
    #endregion

    #region Runtime Methods
    public enum SoundType { Activity };
    public enum NotifierType { Activity, Achievement };
    public static class Runtime
    {
        public static MediaPlayer SoundPlayer = new MediaPlayer();
        static Uri NotifSound = new Uri("pack://siteoforigin:,,,/Assets/Sounds/ActivitySound.wav");

        public static void PlaySound(SoundType soundType, float volume)
        {
            switch (soundType)
            {
                case SoundType.Activity:
                    SoundPlayer.Open(NotifSound);
                    SoundPlayer.Volume = volume;
                    SoundPlayer.Play();
                    break;
            }
        }

        public static async void SetPresence(ActivityState activityState, string content)
        {
            List<int> friendIDs = new List<int>();
            foreach (var friend in UserData.Friends)
            {
                friendIDs.Add(int.Parse(friend.ID));
            }

            string Activity = "";
            switch (activityState)
            {
                case ActivityState.Online:
                    Activity = "Online";
                    break;
                case ActivityState.Playing:
                    Activity = content;
                    break;
            }

            //PresenceResponse response = await Networking.NetworkManager.tcpConnection.SendAsync<PresenceResponse>(new PresenceRequest(UserData.ID, Activity, friendIDs));
        }

        //public static void ShowNotification(NotifierType notifierType, string Name, string Activity, ImageSource Image)
        //{
        //    FancyBalloon balloon = new FancyBalloon();

        //    Windows.mainWindow.Dispatcher.Invoke(() =>
        //    {
        //        switch (notifierType)
        //        {
        //            case NotifierType.Activity:
        //                balloon.NameText = Name;
        //                balloon.ActivityText = Activity;
        //                PlaySound(SoundType.Activity, 0.05f);
        //                balloon.Avatar = UserData.AvatarIMG.Source;
        //                balloon.AvatarEllipse.StrokeThickness = 2;
        //                balloon.StatusIndicator.Visibility = Visibility.Visible;
        //                Windows.mainWindow.myNotifyIcon.ShowCustomBalloon(balloon, System.Windows.Controls.Primitives.PopupAnimation.Slide, 4000);
        //                break;
        //            case NotifierType.Achievement:
        //                balloon.NameText = Name;
        //                balloon.ActivityText = Activity;
        //                PlaySound(SoundType.Activity, 0.05f);
        //                balloon.Avatar = BitmapToImage(Properties.Resources.AchievementIcon);
        //                balloon.AvatarEllipse.StrokeThickness = 0;
        //                balloon.StatusIndicator.Visibility = Visibility.Hidden;
        //                Windows.mainWindow.myNotifyIcon.ShowCustomBalloon(balloon, System.Windows.Controls.Primitives.PopupAnimation.Slide, 4000);
        //                break;
        //        }
        //    });
        //}

        public static ImageSource BitmapToImage(System.Drawing.Bitmap bitmap)
        {
            var myImage = new Image();
            myImage.Source = ConvertBitmap(bitmap);
            return myImage.Source;
        }
        public static BitmapImage ConvertBitmap(System.Drawing.Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }

        public static void FadeIn(UIElement element, double from, double to, double time)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.Duration = TimeSpan.FromSeconds(time);
            doubleAnimation.From = from;
            doubleAnimation.To = to;
            element.BeginAnimation(UIElement.OpacityProperty, doubleAnimation);
        }

        public static void PopOut(UIElement element, double from, double to, double time)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.Duration = TimeSpan.FromSeconds(0.2d);
            doubleAnimation.From = 0d;
            doubleAnimation.To = 1d;
            element.BeginAnimation(ScaleTransform.ScaleXProperty, doubleAnimation);
            element.BeginAnimation(ScaleTransform.ScaleYProperty, doubleAnimation);
        }

        public static void PopIn(UIElement element, double from, double to, double time)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.Duration = TimeSpan.FromSeconds(0.2d);
            doubleAnimation.From = 1d;
            doubleAnimation.To = 0d;
            element.BeginAnimation(ScaleTransform.ScaleXProperty, doubleAnimation);
            element.BeginAnimation(ScaleTransform.ScaleYProperty, doubleAnimation);
        }

        public static BitmapImage Convert(System.Drawing.Image img)
        {
            using (var memory = new MemoryStream())
            {
                img.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }
        public static byte[] SaveImage(BitmapSource bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(ms);

                return ms.GetBuffer();
            }
        }

        static RestAPI rest = new RestAPI("https://beyondinfinitystudio.com/wp-json/wc/v2/", "ck_43c99a8946eed48af9514c0807baca365f840f5a", "cs_79ad18a115ed30f38be881c5767e7538131ce493");
        public static WCObject WC = new WCObject(rest);

        public static List<Product> StoreList { get; set; }
    }
    #endregion

    #region User Data Class
    public static class UserData
    {
        public static string ID { get; set; }
        public static string Username { get; set; }
        public static string Email { get; set; }
        public static string NickName { get; set; }
        public static string XP { get; set; }
        public static string Avatar { get; set; }
        public static string Library { get; set; }

        public static string flist { get; set; }
        public static List<Friends> Friends = new List<Friends>();
        //public static List<FriendListItem> friendListItems = new List<FriendListItem>();
        public static Image AvatarIMG { get; set; }

        public static string GetCurrentLevel()
        {
            int currentXP = int.Parse(XP);
            if (currentXP < 100)
                return "1";
            else if (currentXP < 500)
                return "2";
            else if (currentXP < 1000)
                return "3";
            else if (currentXP < 1500)
                return "4";
            else
                return "95";
        }
    }

    public class Friends
    {
        public string ID;
        public string Name;
        public string Avatar;
    }
    #endregion

    #region Data Classes
    public static class LibraryPageData
    {
        public static string Title { get; set; }
        public static string Description { get; set; }
    }

    public class LibraryList
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public string Thumbnail { get; set; }
    }

    public static class GamePageData
    {
        public static string GameTitle { get; set; }
        public static string GameDescription { get; set; }
        public static string GamePrice { get; set; }
    }

    public class StoreListData
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string Price { get; set; }
    }

    public class Response
    {
        [JsonProperty("status")]
        public string status;
        [JsonProperty("message")]
        public string message;
    }
    #endregion

    #region Program Windows
    public static class Windows
    {
        public static MainWindow mainWindow { get; set; }
        public static LauncherPage LauncherPage { get; set; }
        public static LibraryPage LibraryPage = new LibraryPage();
        public static StorePage StorePage { get; set; }
        public static PublishPage PublishPage = new PublishPage();
        //public static ProfilePage profilePage = new ProfilePage();
        //public static GameStorePage gameStorePage = new GameStorePage();
        //public static GameLibraryPage gameLibraryPage = new GameLibraryPage();

        public static void UnSelectAll()
        {
            //foreach(ListBoxItem button in mainWindow.LeftMenu.Items)
            //{
            //    button.IsSelected = false;
            //}
        }
    }
    #endregion

    #region Manage Launcher States
    public static class States
    {
        public enum State { Ready, Update, Playing, Updating }
        public static State CurrentState = State.Ready;

        public static void UpdateState(State state)
        {
            if (Windows.mainWindow != null)
            {
                CurrentState = state;

                switch (state)
                {
                    case State.Ready:
                        //Windows.mainWindow.Avatar.Stroke = new SolidColorBrush(Colors.Black);
                        break;
                    case State.Playing:
                        //Windows.mainWindow.Avatar.Stroke = new SolidColorBrush(Colors.Green);
                        break;
                    default:
                        //Windows.mainWindow.Avatar.Stroke = new SolidColorBrush(Colors.Black);
                        break;
                }
            }
        }
    }
    #endregion

    #region Utility Methods
    static class Utils
    {
        public static async void DelayedInvoke(Action function, int time)
        {
            await Task.Delay(time);
            function();
        }

        public static bool IsMyFriend(string ID)
        {
            if (ID != UserData.ID)
            {
                foreach (Friends friend in UserData.Friends)
                {
                    if (friend.ID == ID)
                        return true;
                    else
                        return false;
                }
                return IsMyFriend(ID);
            }
            else
                return false;
        }

        public static void CopyPropertiesTo(this object fromObject, object toObject)
        {
            PropertyInfo[] toObjectProperties = toObject.GetType().GetProperties();
            foreach (PropertyInfo propTo in toObjectProperties)
            {
                PropertyInfo propFrom = fromObject.GetType().GetProperty(propTo.Name);
                if (propFrom != null && propFrom.CanWrite)
                    propTo.SetValue(toObject, propFrom.GetValue(fromObject, null), null);
            }
        }

    }
    #endregion
}