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
        public async Task ListenToClientConnections()
        {
            try
            {
                AnsiConsole.Write(new Markup(ConsoleServiceStyleCommon.GetAuthorizerGreeting()));

                ServiceReciever newClient;

                await TryConnectToMessengerAsync();

                clientListener.Start();

                while (true)
                {
                    var clientSocket = await clientListener.AcceptTcpClientAsync();

                    newClient = new(clientSocket, this);

                    UserList.Add(newClient);

                    // each user is handled in their own task
                    Task.Run(() => newClient.ProcessAsync());
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
        public async Task SendClientResponse(ServiceReciever client, bool checkResult)
        {
            await TryConnectToMessengerAsync();
            PackageBuilder builder = new PackageBuilder();
            string package;

            // 'true' - access granted. else 'false'
            if (checkResult && messengerServiceSocket.Connected) package = JsonMessageFactory.GetJsonMessageSimplified("Authorizer", "Client", "Granted");
            else package = JsonMessageFactory.GetJsonMessageSimplified("Authorizer", "Client", "Denied");

            builder.WriteJsonMessage(package);

            client.ClientSocket.Client.Send(builder.GetPacketBytes());
        }



        /// <summary>
        /// Try send user login to the messenger service.
        /// <br />
        /// Попытаться отправить логин пользователя на сервис сообщений.
        /// </summary>
        public async Task<bool> TrySendLoginToService(ServiceReciever user)
        {
            await TryConnectToMessengerAsync();

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
        public async Task<bool> TryAddNewUser(UserClientTechnicalDTO user)
        {
            await TryConnectToMessengerAsync();

            if (messengerServiceSocket.Connected)
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
            else
            {
                AnsiConsole.Write(new Markup($"{ConsoleServiceStyleCommon.GetCurrentTime()} Client registration failed due to negative [red on black]Messenger[/] status.\n"));
                return false;
            }
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





        #region LOGIC



        /// <summary>
        /// Try renew connection to the messenger.
        /// <br />
        /// Попытаться обновить подключение к мессенжеру.
        /// </summary>
        private async Task TryConnectToMessengerAsync()
        {
            try
            {
                if (!messengerServiceSocket.Connected)
                {
                    AnsiConsole.Write(new Markup($"{ConsoleServiceStyleCommon.GetCurrentTime()} [red on black]Messenger[/] [yellow on black]server is down. Renewing connection...[/]\n"));
                    messengerServiceSocket = new();
                    await messengerServiceSocket.ConnectAsync(NetworkConfigurator.AuthorizerMessengerEndPoint, new CancellationTokenSource(NetworkConfigurator.ConnectionTimeoutValue).Token);
                    AnsiConsole.Write(new Markup($"{ConsoleServiceStyleCommon.GetCurrentTime()} [yellow on green]Connection renewed.[/]\n"));
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.Write(new Markup($"{ConsoleServiceStyleCommon.GetCurrentTime()} [yellow on red]Failed to renew Messenger conection.[/]\n"));
            }
        }



        #endregion LOGIC






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
