//using System;
//using System.Threading.Tasks;
//using System.Net;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Diagnostics;
//using TestServerClientPackets.ExamplePacketsTwo;
//using Network;

//namespace GSBFLauncher.CustomClasses
//{
//    public class NetworkManager
//    {
//        private readonly string serverIP = "193.123.66.18";
//        private readonly int serverPort = 1234;
//        public TcpConnection tcpConnection;

//        #region Connect to Server
//        public void StartConnection()
//        {
//            ConnectionResult connectionResult = ConnectionResult.Timeout;

//            tcpConnection = ConnectionFactory.CreateSecureTcpConnection(serverIP, serverPort, out connectionResult);
//            if (connectionResult != ConnectionResult.Connected) return;

//            tcpConnection.RegisterStaticPacketHandler<CalculationRequest>(calculationReceived);
//            tcpConnection.RegisterStaticPacketHandler<NotifierRequest>(NotifReceived);

//            //string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
//            //var externalIp = System.Net.IPAddress.Parse(externalIpString);

//            string IPAddress = "0";
//            tcpConnection.Send(new RegisterUserRequest(int.Parse(UserData.ID), UserData.NickName, IPAddress, "Online", UserData.Avatar));

//            //StatusResponse response = await tcpConnection.SendAsync<StatusResponse>(new StatusRequest(IPAddress));
//            //Debug.WriteLine(response.Result);
//        }
//        #endregion

//        #region Ping response receive method
//        private static void calculationReceived(CalculationRequest packet, Connection connection)
//        {
//            connection.Send(new CalculationResponse(packet.X, packet));
//            Debug.WriteLine("Ping responded");
//        }
//        #endregion

//        #region Notification Request receive method
//        private void NotifReceived(NotifierRequest packet, Connection connection)
//        {
//            //foreach (FriendListItem friend in UserData.friendListItems)
//            //{
//            //    if (friend.friendID == packet.UserID)
//            //    {
//            //        string NotificationText = $"started playing {packet.Activity}";
//            //        Windows.mainWindow.ShowNotification(friend.NickName, NotificationText, friend.avatarSource);
//            //    }
//            //}
//        }
//        #endregion

//        public void CloseConnection()
//        {
//            tcpConnection.Close(Network.Enums.CloseReason.ClientClosed);
//        }

//        public async Task<bool> CheckOnline(string userID)
//        {
//            StatusResponse response = await tcpConnection.SendAsync<StatusResponse>(new StatusRequest(userID));
//            Debug.WriteLine(response.Status);
//            return response.Status;
//        }

//        public void CreateGameSession()
//        {
//            tcpConnection.Send(new GameSessionRequest(int.Parse(UserData.ID)));
//        }
//    }
//}
