using System.Net;
using System.Net.Sockets;
using Tools.Toolbox;

namespace Common.Dependencies.Factories
{
    public static class IpEndPointDataAccessFactory
    {


        #region LOCALHOST



        /// <summary>
        /// Get localhost authorizer-messenger point (port 7111).
        /// <br />
        /// Получить авторайзер-мессенжер локалхост point (порт 7111).
        /// </summary>
        public static IPEndPoint GetLocalhostAuthorizerMessengerPoint()
        {
            return new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7111);
        }


        /// <summary>
        /// Get localhost user-authorizer point (port 7222).
        /// <br />
        /// Получить пользователь-авторайзер локалхост point (порт 7222).
        /// </summary>
        public static IPEndPoint GetLocalhostClientAuthorizerPoint()
        {
            return new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7222);
        }


        /// <summary>
        /// Get localhost user-messenger point (port 7333).
        /// <br />
        /// Получить пользователь-месенжер локалхост point (порт 7333).
        /// </summary>
        public static IPEndPoint GetLocalhostMessengerClientPoint()
        {
            return new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7333);
        }



        #endregion LOCALHOST




        #region LOCAL



        /// <summary>
        /// Get local authorizer-messenger point (port 7111).
        /// <br />
        /// Получить авторайзер-мессенжер локал point (порт 7111).
        /// </summary>
        public static IPEndPoint GetLocalAuthorizerMessengerPoint()
        {
            return new IPEndPoint(IPAddress.Parse(Utilizer.GetLocalIPAddress()), 7111);
        }


        /// <summary>
        /// Get local user-authorizer point (port 7222).
        /// <br />
        /// Получить пользователь-авторайзер локал point (порт 7222).
        /// </summary>
        public static IPEndPoint GetLocalClientAuthorizerPoint()
        {
            return new IPEndPoint(IPAddress.Parse(Utilizer.GetLocalIPAddress()), 7222);
        }


        /// <summary>
        /// Get local user-messenger point (port 7333).
        /// <br />
        /// Получить пользователь-месенжер локал point (порт 7333).
        /// </summary>
        public static IPEndPoint GetLocalMessengerClientPoint()
        {
            return new IPEndPoint(IPAddress.Parse(Utilizer.GetLocalIPAddress()), 7333);
        }



        #endregion LOCAL




        #region IGOR



        /// <summary>
        /// Get authorizer-messenger point (port 7111).
        /// <br />
        /// Получить авторайзер-мессенжер point (порт 7111).
        /// </summary>
        public static IPEndPoint GetIgorAuthorizerMessengerPoint()
        {
            return new IPEndPoint(IPAddress.Parse("10.61.140.35"), 7111);
        }


        /// <summary>
        /// Get user-authorizer point (port 7222).
        /// <br />
        /// Получить пользователь-авторайзер point (порт 7222).
        /// </summary>
        public static IPEndPoint GetIgorClientAuthorizerPoint()
        {
            return new IPEndPoint(IPAddress.Parse("10.61.140.35"), 7222);
        }


        /// <summary>
        /// Get user-messenger point (port 7333).
        /// <br />
        /// Получить пользователь-месенжер point (порт 7333).
        /// </summary>
        public static IPEndPoint GetIgorMessengerClientPoint()
        {
            return new IPEndPoint(IPAddress.Parse("10.61.140.35"), 7333);
        }



        #endregion IGOR




        #region GEVORK



        /// <summary>
        /// Get authorizer-messenger point (port 7111).
        /// <br />
        /// Получить авторайзер-мессенжер point (порт 7111).
        /// </summary>
        public static IPEndPoint GetGevorkAuthorizerMessengerPoint()
        {
            return new IPEndPoint(IPAddress.Parse("10.61.140.37"), 7111);
        }


        /// <summary>
        /// Get user-authorizer point (port 7222).
        /// <br />
        /// Получить пользователь-авторайзер point (порт 7222).
        /// </summary>
        public static IPEndPoint GetGevorkClientAuthorizerPoint()
        {
            return new IPEndPoint(IPAddress.Parse("10.61.140.37"), 7222);
        }


        /// <summary>
        /// Get user-messenger point (port 7333).
        /// <br />
        /// Получить пользователь-месенжер point (порт 7333).
        /// </summary>
        public static IPEndPoint GetGevorkMessengerClientPoint()
        {
            return new IPEndPoint(IPAddress.Parse("10.61.140.37"), 7333);
        }



        #endregion GEVORK




    }
}
