using MessengerClient.ViewModel.ClientStartupWindow;
using Common.Objects.Common;
using MessengerClient.ViewModel.ClientChatWindow;
using Common.Net.Config;
using Common.Processing;
using System.IO.Packaging;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using System.Windows;
using System.Net;
using MessengerClient.LocalService;
using Common.Assets.Enum.Client;
using Common.Assets.Misc;

namespace Net.Transmition
{
    /// <summary>
    /// An instance that provides client with basic datalink operations.
    /// <br />
    /// Абстракция, которая предоставляет клиенту возможность проводить основные сетевые действия.
    /// </summary>
    public class ClientTransmitter : IDisposable
    {


        #region PROPERTIES - public & private Properties



        /// <summary>
        /// An instance that serves as a flag for client windows changing.
        /// <br />
        /// Объект, который служит для перемещения между окнами клиента.
        /// </summary>
        private static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();



        /// <summary>
        /// Provides client connections fo the authorization service;
        /// <br />
        /// Предоставляет клиенские подключения для сервиса авторизации;
        /// </summary>
        private TcpClient authorizationSocket;


        /// <summary>
        /// Provides client connections fo the messenger service;
        /// <br />
        /// Предоставляет клиенские подключения для сервиса месенжера;
        /// </summary>
        private TcpClient messengerSocket;


        ///<inheritdoc cref="Disposed"/>
        private bool _disposed;


        ///<inheritdoc cref="AuthorizationPacketReader"/>
        private PackageReader _authorizationPacketReader;


        ///<inheritdoc cref="MessengerPacketReader"/>
        private PackageReader _messengerPacketReader;



        /// <summary>
        /// 'True' - if this instance has already been disposed, otherwise 'false'.
        /// <br />
        /// "True", если от этого проекта уже избавились, иначе "false".
        /// </summary>
        public bool Disposed
        {
            get { return _disposed; }
        }


        /// <summary>
        /// An object that provides aid in reading authorizer networkstream data.
        /// <br />
        /// Объект, который помогает читать данные из сетевого стрима авторизатора.
        /// </summary>
        public PackageReader AuthorizationPacketReader
        {
            get { return _authorizationPacketReader; }
            set
            {
                _authorizationPacketReader = value;
            }
        }


        /// <summary>
        /// An object that provides aid in reading messenger networkstream data.
        /// <br />
        /// Объект, который помогает читать данные из сетевого стрима мессенжера.
        /// </summary>
        public PackageReader MessengerPacketReader
        {
            get { return _messengerPacketReader; }
            set
            {
                _messengerPacketReader = value;
            }
        }





        /// <summary>
        /// User connection Event;
        /// <br />
        /// Событие подключения пользователя;
        /// </summary>
        public event Action connectedEvent;

        /// <summary>
        /// Message recievment event;
        /// <br />
        /// Событие получения сообщения;
        /// </summary>
        public event Action msgReceivedEvent;

        /// <summary>
        /// When current user decides to delete their message.
        /// <br />
        /// Когда некий пользователь решает удалить своё сообщение.
        /// </summary>
        public event Action mesageDeletedEvent;

        /// <summary>
        /// Other user disconnection event;
        /// <br />
        /// Событие отключения другого пользователя;
        /// </summary>
        public event Action otherUserDisconnectEvent;

        /// <summary>
        /// Current user disconnection event.
        /// <br />
        /// Событие отключение текущего пользователя.
        /// </summary>
        public event Action currentUserDisconnectEvent;




        /// <summary>
        /// A delegate for transeffring output to other objects;
        /// <br />
        /// Делегат для передачи аутпута другим объектам;
        /// </summary>
        /// <param name="sOutputMessage">
        /// A _message that we want to see somewhere (в данном случае, в консоли сервера и в пользовательском клиенте);
        /// <br />
        /// Сообщение, которое мы хотим где-то увидеть (в данном случае, в консоли сервера и в пользовательском клиенте);
        /// </param>
        public delegate void PendOutputDelegate(string sOutputMessage);

        /// <inheritdoc cref="PendOutputDelegate"/>
        public event PendOutputDelegate SendOutput;



        #endregion PROPERTIES - public & private Properties





        #region API - public Behavior



        /// <summary>
        /// Conntect to the authorization service and proceed to authorization;
        /// <br />
        /// Подключиться к сервису авторизации и пройти процесс авторизации;
        /// </summary>
        /// <param name="user">
        /// User technical DTO containing their private data.
        /// <br />
        /// "Технический" DTO пользователя, содержащий его личную информацию.
        /// </param>
        /// <returns>
        /// 'True' - if authorization successful, otherwise 'false'.
        /// <br />
        /// "True" - есди авторизация успешна, иначе "false".
        /// </returns>
        public async Task<bool> ConnectAndAuthorize(UserClientTechnicalDTO user)
        {
            //Если клиент не подключен
            if (!authorizationSocket.Connected)
            {
                await authorizationSocket.ConnectAsync(NetworkConfigurator.ClientAuthorizerEndPoint);
            }

            _authorizationPacketReader = new(authorizationSocket.GetStream());

            var connectPacket = new PackageBuilder();

            connectPacket.WriteOpCode(1);

            connectPacket.WriteJsonMessage(JsonMessageFactory.GetJsonMessageSimplified("Client", "Authorizer", $"{user.Login}|{user.Password}"));

            authorizationSocket.Client.Send(connectPacket.GetPacketBytes());

            var result = JsonMessageFactory.GetUnserializedPackage(_authorizationPacketReader.ReadJsonMessage()).Message as string;

            if (result.Equals("Denied")) return false;
            else return true;
        }



        /// <summary>
        /// Connect to the messenger service and send a query for current user data.
        /// <br />
        /// Подключиться к сервису сообщений и запросить данные текущего пользователя.
        /// </summary>
        /// <param name="user">
        /// User technical DTO containing their private data.
        /// <br />
        /// "Технический" DTO пользователя, содержащий его личную информацию.
        /// </param>
        public bool ConnectAndSendLoginToService(UserClientTechnicalDTO user)
        {
            try
            {
                messengerSocket.Connect(NetworkConfigurator.ClientMessengerEndPoint);
            }
            catch (Exception exSocketObsolete)
            {
                try 
                { 
                    messengerSocket = new();
                    messengerSocket.Connect(NetworkConfigurator.ClientMessengerEndPoint);
                }
                catch (Exception exServiceDown)
                {
                    SendOutput("Server is down. Concider connecting later.");
                }
            }

            if (messengerSocket.Connected)
            {
                MessengerPacketReader = new(messengerSocket.GetStream());

                var connectPacket = new PackageBuilder();

                connectPacket.WriteJsonMessage(JsonMessageFactory.GetJsonMessageSimplified("Client", "Messenger", user.Login));

                messengerSocket.Client.Send(connectPacket.GetPacketBytes());

                return true;
            }

            return false;
        }



        /// <summary>
        /// Disconnect client from messenger service;
        /// <br />
        /// Отключить клиент от сервиса мессенжера;
        /// </summary>
        public void Disconnect()
        {
            if (authorizationSocket.Connected)
            {
                cancellationTokenSource.Cancel();

                messengerSocket.Close();

                currentUserDisconnectEvent?.Invoke();
            }
        }



        /// <summary>
        /// Send user's _message to the service;
        /// <br />
        /// Отправить сообщение пользователя на сервис;
        /// </summary>
        /// <param name="message">
        /// User's _message;
        /// <br />
        /// Сообщение пользователя;
        /// </param>
        public void SendMessageToServer(string assembledJsonMessage)
        {
            var messagePacket = new PackageBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteJsonMessage(assembledJsonMessage);
            try
            {
                messengerSocket.Client.Send(messagePacket.GetPacketBytes());
            }
            catch (Exception ex)
            {
                SendOutput.Invoke($"You haven't connected yet.\n\nException: {ex.Message}");
            }
        }



        /// <summary>
        /// Sign up new user based on user technical dto.
        /// <br />
        /// Зарегистрировать нового пользователя, основанного на техническом объекте.
        /// </summary>
        public bool RegisterNewUser(UserClientTechnicalDTO userData)
        {
            if (!authorizationSocket.Connected) authorizationSocket.Connect(NetworkConfigurator.ClientAuthorizerEndPoint);

            SendNewClientData(userData);

            PackageReader _authorizationPacketReader = new(authorizationSocket.GetStream());

            var resultRaw = _authorizationPacketReader.ReadJsonMessage();

            var result = JsonMessageFactory.GetUnserializedPackage(resultRaw);

            if (result.GetMessage().Equals("Denied")) return false;
            else return true;
        }



        /// <summary>
        /// Get the data sent by messenger in response for client data request after successful authorization.
        /// <br />
        /// Получить данные от мессенжера, в ответ на запрос пользователя после удачной авторизации.
        /// </summary>
        public (UserServerSideDTO userData, List<UserClientPublicDTO> memberList) GetResponseData()
        {
            UserServerSideDTO res = null;
            List<UserClientPublicDTO> list = null;
            var code = _messengerPacketReader.ReadByte();
            if (code == 12)
            {
                // reading user data;
                var msg = JsonMessageFactory.GetUnserializedPackage(_messengerPacketReader.ReadJsonMessage());
                res = JsonConvert.DeserializeObject(msg.Message as string, type: typeof(UserServerSideDTO)) as UserServerSideDTO;

                // reading list of users;
                var msgList = JsonMessageFactory.GetUnserializedPackage(_messengerPacketReader.ReadJsonMessage());
                list = JsonConvert.DeserializeObject<List<UserClientPublicDTO>>(msgList.Message as string);
            }
            return (res, list);
        }



        /// <summary>
        /// Send the message deletion request to the messenger.
        /// <br />
        /// Отправить запросс об удалении сообщения в мессенжер.
        /// </summary>
        public void SendMessageDeletionToServer(string assembledJsonMessage)
        {
            var messagePacket = new PackageBuilder();
            messagePacket.WriteOpCode(6);
            messagePacket.WriteJsonMessage(assembledJsonMessage);
            try
            {
                messengerSocket.Client.Send(messagePacket.GetPacketBytes());
            }
            catch (Exception ex)
            {
                SendOutput.Invoke($"You haven't connected yet.\n\nException: {ex.Message}");
            }
        }



        #endregion API - public Behavior





        #region LOGIC - internal Behavior



        /// <summary>
        /// Read the incomming packet. A packet is a specific _message, sent by ServiceHub to handle different actions;
        /// <br />
        /// Прочитать входящий пакет. Пакет - это специальное сообщение, отправленное объектом "ServiceHub", чтобы структурировать обработку разных событий;
        /// </summary>
        public async Task ReadPacketsAsync()
        {
            if (_messengerPacketReader is null) _messengerPacketReader = new(messengerSocket.GetStream());

            byte opCode = 77;
            while (true)
            {
                try
                {
                    await Task.Run(() => opCode = _messengerPacketReader.ReadByte());
                }
                catch (Exception e)
                {
                    File.WriteAllText(@"..\..\..\.log\client-log.txt", $"\n{StringAssets.DateFormat}\t{StringAssets.TimeMillisecondFormat} Exception at client transmitter, Unit 'ReadPacketsAsync' block 'try'.\nException: {e.Message}\n{e.StackTrace}\n");
                }
                finally
                {
                    if (opCode != (byte)EnumAssets.WaitingForMessage)
                    {
                        switch ((EnumAssets)opCode)
                        {
                            case EnumAssets.Unknown:
                                break;

                            case EnumAssets.NewContact:
                                connectedEvent?.Invoke(); // client connection;
                                break;

                            case EnumAssets.MessageRecieved:
                                msgReceivedEvent?.Invoke(); // message recieved;
                                break;

                            case EnumAssets.MessageDeletionRequest:
                                mesageDeletedEvent?.Invoke(); // client message deleted;
                                break;

                            case EnumAssets.ContactDisconnection:
                                otherUserDisconnectEvent?.Invoke(); // client disconnection;
                                break;

                            case EnumAssets.ServerShutdown:
                                Disconnect();
                                break;

                            default:
                                SendOutput.Invoke("Operation code out of [1,5,6,10]. This is a debug _message.\nproject: ReversedClient, class: ClientTransmitter, method: ReadPacketsAsync.");
                                break;
                        }
                    }
                }
            }
        }



        /// <summary>
        /// Send data of the client that wants to register.
        /// <br />
        /// Отправить даные клиента, который хочет зарегистрироваться.
        /// </summary>
        /// <param name="userData">
        /// New user data, packed in DTO.
        /// <br />
        /// Данные нового пользователя, упакованные в "DTO".
        /// </param>
        private void SendNewClientData(UserClientTechnicalDTO userData)
        {
            var messagePacket = new PackageBuilder();
            var signUpMessage = JsonMessageFactory.GetJsonMessageSimplified(sender: userData.PublicId, reciever: "Service", message: $"{userData.Login}|{userData.Password}");
            messagePacket.WriteOpCode(0);
            messagePacket.WriteJsonMessage(signUpMessage);
            try
            {
                authorizationSocket.Client.Send(messagePacket.GetPacketBytes());
            }
            catch (Exception ex)
            {
                SendOutput.Invoke($"You haven't connected yet.\n\nException: {ex.Message}");
            }
        }



        #endregion LOGIC - internal Behavior





        #region CONSTRUCTION - Object Lifetime



        /// <summary>
        /// Default constructor;
        /// <br />
        /// Конструктор по умолчанию;
        /// </summary>
        public ClientTransmitter()
        {
            _disposed = false;

            authorizationSocket = new TcpClient();

            messengerSocket = new TcpClient();
        }



        /// <summary>
        /// Flush the TcpClients and set the flag.
        /// <br />
        /// Освободить TcpClient'ы и установить флаг.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                authorizationSocket?.Close();

                messengerSocket?.Close();

                _disposed = true;
            }
        }



        #endregion CONSTRUCTION - Object Lifetime



    }
}
