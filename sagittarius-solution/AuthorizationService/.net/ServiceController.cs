using AuthorizationServiceProject.Net;
using Toolbox.Flags;
using AuthorizationServiceProject.Model.Context;
using AuthorizationServiceProject.Model.Entities;

namespace AuthorizationServiceProject.Net
{
    /// <summary>
    /// An instance that assosiates all reciever as well as talks to the db.
    /// <br />
    /// Объект, который хранит всех ресиверов и общается с б/д.
    /// </summary>
    public class ServiceController
    {


        #region STATE



        /// <inheritdoc cref="UserList">
        private List<ServiceReciever> _userList;


        /// <summary>
        /// An instance to accept incomming connections.
        /// <br />
        /// Сущность для приёма входящих подключений.
        /// </summary>
        private TcpListener clientListener;


        /// <summary>
        /// A messenger service socket.
        /// <br />
        /// Сокет сервиса сообщений.
        /// </summary>
        private TcpClient messengerServiceSocket;





        /// <summary>
        /// A list of users.
        /// <br />
        /// Лист пользователей.
        /// </summary>
        public List<ServiceReciever> UserList
        { 
            get { return _userList; } 
            set
            {
                _userList = value;
            }
        }



        #endregion STATE






        #region API - public Contract



        /// <summary>
        /// Run client incomming connection listenning loop.
        /// <br />
        /// Запустить цикл прослушивания входящий подключений.
        /// </summary>
        public void ListenToClientConnections()
        {
            try
            {
                AnsiConsole.Write(new Markup(ConsoleServiceStyleCommon.GetAuthorizerGreeting()));

                ServiceReciever newClient;

                clientListener.Start();

                try
                {
                    if (!messengerServiceSocket.Connected) messengerServiceSocket.Connect(NetworkConfigurator.AuthorizerMessengerEndPoint);
                }
                catch (Exception ex)
                {
                    AnsiConsole.Write(new Markup($"{ConsoleServiceStyleCommon.GetCurrentTime()} [yellow]Messenger service is currently down.[/]\n"));
                }

                while (true)
                {
                    newClient = new(clientListener.AcceptTcpClient(), this);

                    UserList.Add(newClient);

                    newClient.ProcessAsync();
                }
            }
            catch (Exception ex) 
            {
                AnsiConsole.Write(new Markup("[red on white]Unexpected Exception on client listenning.[/] " + ex.Message + "\n"));
            }
        }



        /// <summary>
        /// Check user authorization data.
        /// <br />
        /// Проверить данные пользователя.
        /// </summary>
        public bool CheckAuthorizationData(UserClientTechnicalDTO pair)
        {
            bool bRes = false;
            using (AuthorizationDatabaseContext context = new())
            {
                foreach (var el in context.Users)
                {
                    if (el.Login.Equals(pair.Login))
                    {
                        if (el.PasswordHash.Equals(pair.Password))
                        {
                            bRes = true;
                            break;
                        }
                    }
                }
            }
            return bRes;
        }



        /// <summary>
        /// Sends the response of the client registration result.
        /// <br />
        /// Отправить клиенту ответ в виде результата регистрации клиента.
        /// </summary>
        public void SendClientResponse(ServiceReciever client, bool checkResult)
        {
            PackageBuilder builder = new PackageBuilder();
            string package;

            // 'true' - access granted. else 'false'
            if (checkResult) package = JsonMessageFactory.GetJsonMessageSimplified("Authorizer", "Client", "Granted");
            else package = JsonMessageFactory.GetJsonMessageSimplified("Authorizer", "Client", "Denied");

            builder.WriteJsonMessage(package);

            client.ClientSocket.Client.Send(builder.GetPacketBytes());
        }



        /// <summary>
        /// Try send user login to the messenger service.
        /// <br />
        /// Попытаться отправить логин пользователя на сервис сообщений.
        /// </summary>
        public bool TrySendLoginToService(ServiceReciever user)
        {
            try
            {
                if (!messengerServiceSocket.Connected) messengerServiceSocket.Connect(NetworkConfigurator.AuthorizerMessengerEndPoint);
            }
            catch (Exception ex)
            {
                AnsiConsole.Write(new Markup($"{ConsoleServiceStyleCommon.GetCurrentTime()} [yellow]Messenger service was down. Trying to renew connection...[/]\n"));
                try
                {
                    messengerServiceSocket = new();
                    messengerServiceSocket.Connect(NetworkConfigurator.AuthorizerMessengerEndPoint);
                    AnsiConsole.Write(new Markup($"{ConsoleServiceStyleCommon.GetCurrentTime()} [yellow on green]Connection renewed.[/]\n"));
                }
                catch (Exception inex)
                {
                    AnsiConsole.Write(new Markup($"{ConsoleServiceStyleCommon.GetCurrentTime()} [white on red]Messenger service is still down. Client data was not sent.[/]\n"));
                    return false;
                }
            }

            if (messengerServiceSocket.Connected)
            {
                PackageBuilder builder = new();
                builder.WriteJsonMessage(JsonMessageFactory.GetJsonMessageSimplified("Authorizer", "Messenger", $"{user.CurrentUser.Login}|{user.CurrentUser.PublicId}"));
                messengerServiceSocket.Client.Send(builder.GetPacketBytes());
                return true;
            }

            return false;
        }







        /// <summary>
        /// Try add new user to the database.
        /// <br />
        /// Попытаться добавить нового пользователя в б/д.
        /// </summary>
        /// <returns>
        /// 'True' - if operation was successful, if user already present at the db - 'false'.
        /// <br />
        /// "True" - если процесс прошёл успешно, если пользователь уже есть в базе -  "false".
        /// </returns>
        public bool TryAddNewUser(UserClientTechnicalDTO user)
        {
            bool doesContain = IsUserPresentInDatabase(user);
            using (AuthorizationDatabaseContext context = new())
            {
                if (!doesContain)
                {
                    UserModel newUser = new();
                    newUser.Login = user.Login;
                    newUser.PasswordHash = user.Password;
                    context.Users.Add(newUser);

                    context.SaveChanges();
                }
            }
            return !doesContain;
        }



        /// <summary>
        /// Check if passed user information is contained in database and is correct.
        /// <br />
        /// Проверить, если переданная информация о пользователе лежит в б/д и записана корректно.
        /// </summary>
        /// <returns>
        /// 'True' - if data is correct, otherwise 'false'.
        /// <br />
        /// "True" - если данные совпадают, иначе "false".
        /// </returns>
        public bool IsUserPresentInDatabase(UserClientTechnicalDTO user)
        {
            bool doesContain = false;
            using (AuthorizationDatabaseContext context = new())
            {
                foreach (var item in context.Users)
                {
                    if (item.Login.Equals(user.Login) && item.PasswordHash.Equals(user.Password))
                    {
                        doesContain = true;
                        break;
                    }
                }
            }
            return doesContain;
        }



        #endregion API - public Contract







        #region CONSTRUCTION



        /// <summary>
        /// Default constructor.
        /// <br />
        /// Конструктор по умолчанию.
        /// </summary>
        public ServiceController()
        {
            _userList = new();
            clientListener = new ( new IPEndPoint(IPAddress.Any, NetworkConfigurator.ClientAuthorizerPort));
            messengerServiceSocket = new();

            //TrySeedAdmins();
        }


        #endregion CONSTRUCTION


    }
}
