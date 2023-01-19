using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tools.Formatting;

namespace MessengerService.LocalService
{
    /// <summary>
    /// An adapter for transfering user data from d/b to the client.
    /// <br />
    /// Адаптер для передачи пользовательскиз данных из б/д в клиент.
    /// </summary>
    public static class UserParser
    {


        #region API - public Contract



        /// <summary>
        /// Get the private dto from the user entity instance.
        /// <br />
        /// Получить приватную dto пользователя из entity типа "user".
        /// </summary>
        public static UserServerSideDTO ParseToDTO(User user)
        {
            UserServerSideDTO res = new();

            List<Chat> userSortedChatList = new();
            SortChats(ref userSortedChatList);

            res.CurrentNickname = user.CurrentNickname;
            res.CurrentPublicId = user.PublicId;
            res.ChatArray = new List<ChatDTO>();

            foreach (var chat in user.ChatList)
            {
                ChatDTO chatDto = new();
                chatDto.Members = new List<string>();
                chatDto.Messages = new List<MessageDTO>();

                ParseChatMembers(ref chatDto, chat);

                ParseChatMessages(ref chatDto, chat);

                res.ChatArray.Add(chatDto);
            }

            return res;
        }



        /// <summary>
        /// Get an instance of user entity from the user dto one.
        /// <br />
        /// Получить экземпляр entity типа "user" из соответствующей dto.
        /// </summary>
        public static User ParseFromDTO(UserServerSideDTO user)
        {
            User res = new();

            res.CurrentNickname = user.CurrentNickname;
            res.PublicId = user.CurrentPublicId;

            foreach (var chatDto in user.ChatArray)
            {
                Chat complexChat = new();

                foreach (string member in chatDto.Members)
                {
                    User tempUser = new();
                    tempUser.PublicId = member;
                    complexChat.UserList.Add(tempUser);
                }

                foreach (var message in chatDto.Messages)
                {
                    Message tempMessage = new();
                    tempMessage.Author = complexChat.UserList.First(m => m.PublicId.Equals(message.Sender));
                    tempMessage.Date = message.Date;
                    tempMessage.Time = StringDateTime.FromThreeToTwoSections(message.Time);

                    tempMessage.Chat = complexChat;

                    tempMessage.Contents = message.Contents;

                    complexChat.MessageList.Add(tempMessage);

                    if (res.PublicId.Equals(tempMessage.Author.PublicId)) res.MessageList.Add(tempMessage);
                }
                res.ChatList.Add(complexChat);
            }
            return res;
        }



        #endregion API - public Contract





        #region LOGIC



        /// <summary>
        /// Sort messages in a user's chat list.
        /// <br />
        /// Отсортировать сообщения в списке чатов пользователя.
        /// </summary>
        private static void SortChats(ref List<Chat> sortedChatList)
        {
            foreach (Chat chat in sortedChatList)
            {
                chat.MessageList.Sort((Message A, Message B) =>
                {
                    if (Int32.Parse(StringDateTime.RemoveSeparation(A.Date)) > Int32.Parse(StringDateTime.RemoveSeparation(B.Date))) return 1;
                    else if (Int32.Parse(StringDateTime.RemoveSeparation(A.Date)) < Int32.Parse(StringDateTime.RemoveSeparation(B.Date))) return -1;
                    else
                    {
                        if (Int32.Parse(StringDateTime.RemoveSeparation(A.Time)) > Int32.Parse(StringDateTime.RemoveSeparation(B.Time))) return 1;
                        else if (Int32.Parse(StringDateTime.RemoveSeparation(A.Time)) < Int32.Parse(StringDateTime.RemoveSeparation(B.Time))) return -1;
                        else return 0;
                    }
                });
            }
        }



        #endregion LOGIC






        #region SERIALIZATION



        /// <summary>
        /// Parse the member list of the chat.
        /// <br />
        /// Спарсить список участников чата.
        /// </summary>
        private static void ParseChatMembers(ref ChatDTO chatDto, Chat unparsedChat)
        {
            for (int j = 0, jSize = unparsedChat.UserList.Count; j < jSize; j++)
            {
                chatDto.Members.Add(unparsedChat.UserList[j].PublicId);
            }
        }



        /// <summary>
        /// Parse the message list of the chat.
        /// <br />
        /// Спарсить список сообщений чата.
        /// </summary>
        private static void ParseChatMessages(ref ChatDTO chatDto, Chat unparsedChat)
        {
            for (int j = 0, jSize = unparsedChat.MessageList.Count; j < jSize; j++)
            {
                var dto = new MessageDTO
                    (
                        sender: unparsedChat.MessageList[j].Author.PublicId,
                        contents: unparsedChat.MessageList[j].Contents,
                        date: unparsedChat.MessageList[j].Date,
                        time: unparsedChat.MessageList[j].Time
                    );

                chatDto.Messages.Add(dto);
            }
        }



        #endregion SERIALIZATION


    }
}
