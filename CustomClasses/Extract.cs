//using ICSharpCode.SharpZipLib.Zip;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Loot.StaticClasses;
//using IWshRuntimeLibrary;

//namespace InfinityLauncher {
//    class Extract {

//        private GameLibraryPage _application;

//        public Extract(GameLibraryPage application) {
//            _application = application;
//        }

//        public void Run(string Name) {
//            BackgroundWorker bgw = new BackgroundWorker {
//                WorkerReportsProgress = true
//            };

//            bgw.DoWork += new DoWorkEventHandler(
//                delegate(object o, DoWorkEventArgs args) {
//                    BackgroundWorker bw = o as BackgroundWorker;
//                    FastZip fastZip = new FastZip();
//                    fastZip.ExtractZip(URLs.GAME_DOWNLOAD_PATH(Name), URLs.GAME_PATH(Name), null);
//            });
            
//            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
//            delegate(object o, RunWorkerCompletedEventArgs args) {
//                _application.SetLauncherReady();
//                CreateShortcut();
//            });

//            bgw.RunWorkerAsync();
//        }

//        private void CreateShortcut()
//        {
//            object shDesktop = (object)"Desktop";
//            WshShell shell = new WshShell();
//            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\" + _application.Title + ".lnk";
//            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
//            shortcut.Description = "New shortcut for a Notepad";
//            shortcut.Hotkey = "Ctrl+Shift+N";
//            shortcut.TargetPath = URLs.GAME_EXECUTABLE_PATH(_application.Title);
//            shortcut.Save();
//        }
//    }
//}
