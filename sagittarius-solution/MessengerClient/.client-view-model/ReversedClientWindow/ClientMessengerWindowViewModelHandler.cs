using Common.Assets.Misc;
using Common.Objects.Common;
using Newtonsoft.Json;
using MessengerClient.LocalService;
using MessengerClient.Model;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Windows.Interop;

namespace MessengerClient.ViewModel.ClientChatWindow
{
    public partial class ClientMessengerWindowViewModel
    {



        #region Transmition handlers



        /// Connection related


        /// <summary>
        /// Connect to service.
        /// <br />
        /// Подключиться к сервису.
        /// </summary>
        private void ConnectToService()
        {
            _serviceTransmitter.ConnectAndAuthorize(CurrentUserTechnicalDTO);
        }


        /// <summary>
        /// Connect new user;
        /// <br />
        /// Подключить нового пользователя;
        /// </summary>
        public void ConnectUser()
        {
            JsonMessagePackage userNameAndIdPackage;

            userNameAndIdPackage = JsonMessageFactory.GetUnserializedPackage(_serviceTransmitter.MessengerPacketReader.ReadJsonMessage());


            // create new user instance;
            var user = new UserClientPublicDTO()
            {
                UserName = userNameAndIdPackage.Message as string,
                PublicId = userNameAndIdPackage.Sender,
            };

            /*
             
           [!] In case there's no such user in collection we add them manualy;
            To prevent data duplication;
            
             */

            MessengerChat newChat;
            if (!DefaultCommonMemberList.Any(x => x.PublicId == user.PublicId))
            {
                if (user.PublicId != _currentUserModel.PublicId)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        DefaultCommonMemberList.Add(user);
                    });
                }
            }
        }


        /// <summary>
        /// Remove a user from the client list;
        /// <br />
        /// Удалить пользователя из списка клиентов;
        /// </summary>
        private void RemoveUser()
        {
            var msg = _serviceTransmitter.MessengerPacketReader.ReadJsonMessage();
            var deserializedMessage = JsonConvert.DeserializeObject<JsonMessagePackage>(msg);
            var uid = deserializedMessage.GetMessage();
            var user = DefaultCommonMemberList.Where(x => x.PublicId.Equals(uid)).FirstOrDefault();

            // foreach (var user in )
            Application.Current.Dispatcher.Invoke(() => DefaultCommonMemberList.Remove(user));   // removing disconnected user;
        }


        /// <summary>
        /// Make all actions needed for the ui side.
        /// <br />
        /// Выполнить все необходимые со стороные UI действия.
        /// </summary>
        private void DisconnectFromService()
        {
            AlreadyDisconnected = true;
            WpfWindowsManager.MoveFromChatToLogin(string.Empty);
        }




        /// Message transmittion


        /// <summary>
        /// Send a _message to the service;
        /// <br />
        /// Is needed to nullify the chatWithDeletedMessage _message field after sending;
        /// <br />
        /// <br />
        /// Отправить сообщение на сервис;
        /// <br />
        /// Необходимо, чтобы стереть сообщение после отправкиж
        /// </summary>
        private void SendMessage()
        {
            try
            {
                if (Message != string.Empty)
                {
                    UserClientPublicDTO currentAddressee = null;
                    if (SelectedOnlineMember is not null) currentAddressee = SelectedOnlineMember;
                    else if (ActiveChat is not null) currentAddressee = ActiveChat.Addressee;
                    else throw new NullReferenceException("[Custom] No target was selected.");

                    var jsonMessagePackage = new JsonMessagePackage(
                        sender: _currentUserModel.PublicId,
                        reciever: currentAddressee.PublicId,
                        date: DateTime.Now.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                        time: DateTime.Now.ToString("HH:mm:ss:fff"),
                        message: Message);

                    var serializedJsonMessage = JsonMessageFactory.GetSerializedMessage(jsonMessagePackage);

                    _serviceTransmitter.SendMessageToServer(serializedJsonMessage); // transmitter;

                    var someChat = DefaultCommonChatList.FirstOrDefault(c => (c.Addressee.PublicId.Equals(currentAddressee.PublicId)));
                    if (someChat is null)
                    {
                        someChat = new(addresser: CurrentUserModel, addressee: SelectedOnlineMember);
                        DefaultCommonChatList.Add(someChat);
                    }
                    someChat.AddOutgoingMessage(Message + " ✓");
                    ActiveChat = someChat;

                    if (acceptedUserData is null) acceptedUserData = new();

                    ClientMessageTracker.AddMessage(jsonMessagePackage, ref acceptedUserData); // tracker;

                    var aaaaaa = acceptedUserData;

                    Message = string.Empty;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Please, choose a contact.", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }


        /// <summary>
        /// Recieve user _message;
        /// <br />
        /// Получить сообщение от пользователя;
        /// </summary>
        private void RecieveMessage()
        {
            IMessage msg = JsonMessageFactory.GetUnserializedPackage(_serviceTransmitter.MessengerPacketReader.ReadJsonMessage());
            IMessage msgCopy = msg;

            try
            {
                // if the _message was sent to us from other user
                if (!_currentUserModel.PublicId.Equals(msg.GetSender()))
                {
                    var someChat = DefaultCommonChatList.Where(c => c.Addressee.PublicId == msg.GetSender()).FirstOrDefault();
                    if (someChat is null)
                    {
                        someChat = new(addressee: DefaultCommonMemberList.First(u => u.PublicId == msg.GetSender()), addresser: CurrentUserModel);
                        Application.Current.Dispatcher.Invoke(() => DefaultCommonChatList.Add(someChat));
                    }
                    Application.Current.Dispatcher.Invoke(() => someChat.AddIncommingMessage(msgCopy.GetMessage() as string));


                    if (ActiveChat is null || !someChat.Addressee.PublicId.Equals(ActiveChat.Addressee.PublicId))
                    {
                        var addresseeCopy = someChat.Addressee;
                        addresseeCopy.UserName = ChatParser.FromReadToUnread(someChat.Addressee.UserName);
                        someChat.Addressee = addresseeCopy;
                        SystemSounds.Exclamation.Play();
                        OnPropertyChanged(nameof(ChatList));
                    }
                }
                // if we sent this _message
                else
                {
                    VisualizeOutgoingMessage(msgCopy);
                }

                ClientMessageTracker.AddMessage(msgCopy, ref acceptedUserData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Message collection changing exception: {ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Show exception output _message.
        /// <br />
        /// Показать вывод сообщения ошибки/исключения.
        /// </summary>
        private void ShowErrorMessage(string sMessage)
        {
            MessageBox.Show(sMessage, "Exception", MessageBoxButton.OK, MessageBoxImage.Information);
        }



        /// Message Deletion


        /// <summary>
        /// Delete a jsonMessage that service sent here for deletion.
        /// <br />
        /// Удалить сообщение, которое сервис прислал сюда для удаления.
        /// </summary>
        private void DeleteCurrentClientMessageAfterServiceRespond()
        {
            var msg = JsonMessageFactory.GetUnserializedPackage(_serviceTransmitter.MessengerPacketReader.ReadJsonMessage());
            try
            {
                MessageEraser eraser = new(msg, DefaultCommonChatList);
                eraser.DeleteMessage();
                ClientMessageTracker.DeleteMessage(msg, ref acceptedUserData);
                DefaultCommonChatList = eraser.ChatList;
                OnPropertyChanged(nameof(DefaultCommonChatList));

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message} (transmitter, VM-handler)", "Unexpected exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Tell the messenger service that used would like to delete the selected jsonMessage.
        /// <br />
        /// Сообщить сервису сообщений, что пользователь хотел бы удалить выбранное сообщение.
        /// </summary>
        private void InitiateMessageDeletion()
        {
            if (SelectedMessage is not null)
            {
                if (SelectedMessage.Contains(" ✓✓"))
                {

                    // retrieve jsonMessage we want to delete
                    var messageContentString = ClientMessageAdapter.FromListViewChatToPackage(SelectedMessage);

                    ChatDTO chatWithDeletedMessage = acceptedUserData.ChatArray.Where(c => c.Members.ToList().Contains(ActiveChat.Addressee.PublicId)).First();

                    var MessageIndex = ActiveChat.MessageList.IndexOf(SelectedMessage);


                    // get full jsonMessage by the chat we got
                    MessageDTO deletedMessageDto = chatWithDeletedMessage.Messages.ElementAt(MessageIndex);

                    var messageTime = deletedMessageDto.Time;
                    
                    // make a jsonMessage to server with full info
                    var pack = JsonMessageFactory.GetJsonMessage
                    (
                        sender: ActiveChat.Addresser.PublicId,
                        reciever: ActiveChat.Addressee.PublicId,
                        date: deletedMessageDto.Date,
                        time: deletedMessageDto.Time,
                        message: messageContentString
                    );

                    // send deletion request
                    _serviceTransmitter.SendMessageDeletionToServer(pack);
                }
            }
        }



        #endregion Transmition handlers





        #region LOGIC



        /// <summary>
        /// Add or sign out and outhoing messageContentString sent by client on recieving from service.
        /// <br />
        /// Добавить или отметить исходящее сообщение, отправленное клиентом, при получении его от сервиса.
        /// </summary>
        private void VisualizeOutgoingMessage(IMessage msg)
        {
            var someChat = DefaultCommonChatList.FirstOrDefault(c => c.Addressee.PublicId.Equals(msg.GetReciever()));
            string newMessage = string.Empty;
            string oldMessage = string.Empty;


            oldMessage = someChat.MessageList.Select(m => m).Where(m => (m.Contains(msg.GetSender()) && m.Contains(msg.GetMessage() as string) && !m.Contains("✓✓"))).FirstOrDefault();
            if (oldMessage is not null)
            {
                newMessage = oldMessage + "✓";
            }
            else
                if (msg.GetSender().Equals(acceptedUserData.CurrentPublicId))
                    Application.Current.Dispatcher.Invoke(() => someChat.AddOutgoingMessage((msg.GetMessage() as string) + " ✓✓"));
                else Application.Current.Dispatcher.Invoke(() => someChat.AddIncommingMessage(msg.GetMessage() as string));


            ObservableCollection<string> newMessageList = new();
            foreach (string message in someChat.MessageList)
            {
                if (!message.Equals(oldMessage))
                {
                    newMessageList.Add(message);
                }
                else
                {
                    newMessageList.Add(newMessage);
                }
            }
            someChat.MessageList = newMessageList;

            OnPropertyChanged(nameof(ActiveChat));
        }



        /// <summary>
        /// Start reading packets.
        /// <br />
        /// Начать читать пакеты.
        /// </summary>
        public async Task StartListenningAsync()
        {
            await ServiceTransmitter.ReadPacketsAsync();
        }



        #endregion LOGIC






        #region HANDLERS



        /// <summary>
        /// Handle the logout button click.
        /// <br />
        /// Обработать клик по кнопке "Выход".
        /// </summary>
        private void OnLogoutButtonPressed()
        {
            var vRes = MessageBox.Show("Are you sure you want to log off?", "Logging Off", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (vRes == MessageBoxResult.Yes)
            {
                WpfWindowsManager.MoveFromChatToLogin(string.Empty);
            }
        }



        #endregion HANDLERS




    }
}
