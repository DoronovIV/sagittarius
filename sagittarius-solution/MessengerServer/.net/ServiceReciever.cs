using System.Net.Sockets;
using System;
using Spectre.Console;
using MessengerService.Controls;

namespace MessengerService.Datalink
{
    /// <summary>
    /// A client reference used by server to manipulate with client data;
    /// <br />
    /// Ссылка на клиента, используемая сёрвером для обработки пользовательских данных;
    /// </summary>
    public class ServiceReciever
    {


        #region PROPERTIES - State of an Object



        /// <summary>
        /// An object that helps reading/writing messages;
        /// <br />
        /// Объект, который предоставляет помощь в чтении/записи сообщений;
        /// </summary>
        private volatile PackageReader packetReader;


        /// <inheritdoc cref="ClientSocket"/>
        private volatile TcpClient _clientSocket;


        /// <inheritdoc cref="CurrentUser"/>
        private volatile User _currentUSer;






        /// <summary>
        /// Current user info.
        /// <br />
        /// Информация о текущем пользователе.
        /// </summary>
        public User CurrentUser 
        {
            get => _currentUSer;
            set => _currentUSer = value;
        }


        /// <summary>
        /// Client's connection socket;
        /// <br />
        /// Сокет подключения клиента;
        /// </summary>
        public TcpClient ClientSocket { get => _clientSocket; set => _clientSocket = value; }



        #endregion PROPERTIES - State of an Object




        #region API - public Contract


        public delegate void MessageTypeDelegate(JsonMessagePackage recievedMessage);

        public event MessageTypeDelegate MessageRecievedEvent;

        public event MessageTypeDelegate MessageDeletedEvent;


        public delegate void UserTypeDelegate(User userData);

        public event UserTypeDelegate UserDisconnected;


        #endregion API - public Contract




        #region LOGIC - private Behavior



        /// <summary>
        /// Listen to clients;
        /// <br />
        /// Прослушивать клиентов;
        /// </summary>
        private void Process()
        {
            object debugInfo = null;
            while (true)
            {

                try
                {
                    var opCode = packetReader.ReadByte();
                    switch ((EnumAssets)opCode)
                    {
                        // text message recieved;
                        case EnumAssets.MessageReceipt:

                            var textMessage = packetReader.ReadJsonMessage();
                            MessageRecievedEvent.Invoke(JsonMessageFactory.GetUnserializedPackage(textMessage));
                            AnsiConsole.Write(new Markup(ConsoleServiceStyle.GetClientMessageReceiptStyle(JsonMessageFactory.GetUnserializedPackage(textMessage))));

                            break;

                        // text message deletion;
                        case EnumAssets.MessageDeletionRequest:

                            var textMessageForDeletion = packetReader.ReadJsonMessage();
                            debugInfo = textMessageForDeletion;
                            try
                            {
                                MessageDeletedEvent.Invoke(JsonMessageFactory.GetUnserializedPackage(textMessageForDeletion));
                                AnsiConsole.Write(new Markup(ConsoleServiceStyle.GetClientMessageDeletionStyle(JsonMessageFactory.GetUnserializedPackage(textMessageForDeletion))));
                            }
                            catch (InvalidDataException e)
                            {
                                AnsiConsole.Write(new Markup($"[black on white][[{StringAssets.TimeSecondFormat}]] [/][red on white]Error 501. {e.GetType().Name} on message deletion.[/]"));
                            }

                            break;


                        default:

                            break;
                    }
                }
                catch (Exception ex)
                {
                    var errorState = debugInfo;
                    AnsiConsole.Write(new Markup(ConsoleServiceStyleCommon.GetUserDisconnection(CurrentUser.Login)));
                    UserDisconnected.Invoke(CurrentUser);
                    ClientSocket.Close(); // if this block is invoked, we can see that the client has disconnected and then we need to invoke the disconnection procedure;
                    break;
                }
            }
        }



        /// <summary>
        /// Put 'Process' method in a separate task.
        /// <br />
        /// Положить метод "Process" в отдельный task.
        /// </summary>
        public async Task ProcessAsync()
        {
            await Task.Run(() => Process());
        }



        #endregion LOGIC - private Behavior




        #region CONSTRUCTION - Object Lifetime



        /// <summary>
        /// Parametrised constructor;
        /// <br />
        /// Конструктор с параметрами;
        /// </summary>
        /// <param name="client">
        /// Client's socket;
        /// <br />
        /// Сокет клиента;
        /// </param>
        /// <param name="serviceHub">
        /// An instance of the 'ServiceController';
        /// <br />
        /// Экземпляр объекта класса "ServiceController";
        /// </param>
        public ServiceReciever(TcpClient client)
        {
            Application.tcpClientController.RegisterClient(client);
            ClientSocket = client;
            CurrentUser = new();
            packetReader = new PackageReader(ClientSocket.GetStream());
        }



        #endregion CONSTRUCTION - Object Lifetime


    }
}
