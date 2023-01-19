using Common.Objects.Common;
using MessengerClient.Model;
using System.Net.NetworkInformation;

namespace MessengerClient.LocalService
{

    /// <summary>
    /// A service for parsing 'DefaultCommonChatList' property of a 'User' object into a view-model 'DefaultCommonChatList' one.
    /// <br />
    /// Сервис для парсинга свойства "DefaultCommonChatList" объекта класса "User" в свойство "DefaultCommonChatList" вью-модели.
    /// </summary>
    public static class ChatParser
    {

        /// <summary>
        /// Parse chat list of a user into an view-model observable collection.
        /// <br />
        /// Спарсить список чатов пользователя в ObservableCollection вью-модели.
        /// </summary>
        public static void FillChats(UserServerSideDTO user, ref ObservableCollection<MessengerChat> ChatList)
        { 

            if (user.ChatArray is not null && user.ChatArray.Count != 0)
            {
                user.SortMessageList();

                foreach (ChatDTO chat in user.ChatArray)
                {
                    var userRef = chat.Members.Select(u => u).Where(u => !u.Equals(user.CurrentPublicId)).FirstOrDefault();
                    var chatRef = new MessengerChat(addresser: new(user.CurrentNickname, user.CurrentPublicId), addressee: new(userRef, userRef));

                    foreach (var message in chat.Messages)
                    {

                        if (message.Sender.Equals(user.CurrentPublicId)) chatRef.AddCheckedOutgoingMessage(message);
                        else chatRef.AddIncommingMessage(message);
                    }
                    
                    ChatList.Add(chatRef);
                }
            }
        }



        /// <summary>
        /// Clear notification sign from the chat title string.
        /// <br />
        /// Очистить знак оповещения со строки заголовка чата.
        /// </summary>
        public static string FromUnreadToRead(string addresseeUsername)
        {
            string res = string.Empty;

            for (int i = 0, iSize = addresseeUsername.Length; i < iSize; i++)
            {
                if (i > 1)
                {
                    res += addresseeUsername[i];
                }
            }

            return res;
        }



        /// <summary>
        /// Add visual notification sign to the chat title (which is represented by the addressee username). [!] If the name already contains the sign, the method returns passed string.
        /// <br />
        /// Добавить значок визуального оповещения в заголовок чата (который представляет собой юзернейм адресата). [!] Если имя уже содержит символ, метод возвращает переданную строку.
        /// </summary>
        public static string FromReadToUnread(string addresseeUsername)
        {
            string res = string.Empty;

            string visualNotoficationSymbol = "▪";

            if (addresseeUsername.Contains(visualNotoficationSymbol)) res = addresseeUsername;
            else
            {
                res = visualNotoficationSymbol + " " + addresseeUsername;
            }

            return res;
        }

    }
}
