using Common.Objects.Common;
using Newtonsoft.Json;
using MessengerClient.Model;
using System.Windows.Interop;

namespace MessengerClient.LocalService
{
    public class MessageEraser
    {


        #region STATE



        /// <inheritdoc cref="Message"/>
        private IMessage? _message;


        /// <inheritdoc cref="ChatList"/>
        private ObservableCollection<MessengerChat>? _chatList;




        /// <summary>
        /// A message for deletion.
        /// <br />
        /// Сообщение к удалению.
        /// </summary>
        public IMessage? Message 
        {
            get { return _message; }
            set { _message = value; }
        }


        /// <summary>
        /// A chat to delete the message in.
        /// <br />
        /// Чат, в котором удаляется сообщение.
        /// </summary>
        public ObservableCollection<MessengerChat>? ChatList
        {
            get { return _chatList; }
            set { _chatList = value; }
        }



        #endregion STATE





        #region API


        /// <summary>
        /// Delete a message in a list of chats.
        /// <br />
        /// Удалить сообщение в массиве чатов.
        /// </summary>
        public void DeleteMessage(IMessage message, ObservableCollection<MessengerChat> chatList, UserServerSideDTO userData)
        {
            Message = message;
            ChatList = chatList;
            DeleteMessage();
        }



        /// <summary>
        /// Delete the message from the list of chats, according to properties.
        /// <br />
        /// Удалить сообщения из массива чатов, в соответствии со свойствами.
        /// </summary>
        public void DeleteMessage()
        {
            if (Message is not null && ChatList is not null)
            {
                MessengerChat someChat = null;
                string deletedChatMessage = string.Empty;
                bool breakFlag = false;

                foreach (var chat in ChatList)
                {
                    foreach (var message in chat.MessageList)
                    {
                        string messageToCompareForDebug = string.Empty;
                        if (Message.GetSender().Equals(chat.Addresser.PublicId)) messageToCompareForDebug = ClientMessageAdapter.FromPackageToChatListViewCurrentUser(Message);
                        else messageToCompareForDebug = ClientMessageAdapter.FromPackageToChatListViewOtherUser(Message);

                        if (messageToCompareForDebug.Equals(message))
                        {
                            someChat = chat;
                            deletedChatMessage = message;
                            breakFlag = true;
                            break;
                        }
                    }
                    if (breakFlag) break;
                }


                MessengerChat someChatCopy = new(addresser: someChat.Addresser, addressee: someChat.Addressee);
                if (someChat is not null)
                {
                    someChat.MessageList.Remove(deletedChatMessage);
                }
            }
            else throw new NullReferenceException("[Custom] The Message and DefaultCommonChatList properties have not been set. You should use them or constructor.");
        }



        #endregion API





        #region CONSTRUCTION



        /// <summary>
        /// Default constructor.
        /// <br />
        /// Конструктор по умолчанию.
        /// </summary>
        public MessageEraser() 
        {
            _message = null;
            _chatList= null;
        }


        /// <summary>
        /// Parametrized constructor.
        /// <br />
        /// Параметризованный конструктор.
        /// </summary>
        public MessageEraser(IMessage message, ObservableCollection<MessengerChat> chatList)
        {
            _message = message;
            _chatList = chatList;
        }



        #endregion CONSTRUCTION


    }
}
