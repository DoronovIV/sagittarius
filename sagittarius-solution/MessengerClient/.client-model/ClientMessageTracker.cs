using Common.Objects.Common;
using Newtonsoft.Json;
using MessengerClient.ViewModel.ClientChatWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace MessengerClient.Model
{
    /// <summary>
    /// An object that manages messages in client dto object.
    /// <br />
    /// Объект, который занимается управлением сообщений в DTO клиента.
    /// </summary>
    public static class ClientMessageTracker
    {



        #region API



        /// <summary>
        /// Add message to the dto.
        /// <br />
        /// Добавить сообщение в dto.
        /// </summary>
        public static void AddMessage(IMessage newMessage, ref UserServerSideDTO UserTracked)
        {
            if (!IsMessageAlreadyPresent(newMessage, ref UserTracked))
            {
                lock (ClientMessengerWindowViewModel.synchronizer)
                {
                    MessageDTO dto = new(newMessage.GetSender(), newMessage.GetMessage() as string, newMessage.GetDate(), newMessage.GetTime());

                    ChatDTO chatDto = GetChatWithMessage(newMessage, ref UserTracked);

                    if (chatDto is null)
                    {
                        chatDto = new();

                        chatDto.Members.Add(newMessage.GetSender());
                        chatDto.Members.Add(newMessage.GetReciever());

                        UserTracked.ChatArray.Add(chatDto);
                    }

                    chatDto.Messages.Add(dto);
                }
            }
        }


        /// <summary>
        /// Delete message from dto.
        /// <br />
        /// Удалить сообщение из dto.
        /// </summary>
        public static void DeleteMessage(IMessage deletionMessage, ref UserServerSideDTO UserTracked)
        {
            lock (ClientMessengerWindowViewModel.synchronizer)
            {
                ChatDTO? chatInWhichToDelete = null;
                MessageDTO? messageToDelete = null;

                foreach (var chat in UserTracked.ChatArray)
                {
                    foreach (var message in chat.Messages)
                    {
                        IMessage tempMessage = JsonConvert.DeserializeObject<JsonMessagePackage>(JsonMessageFactory.GetJsonMessage(message.Sender, "n/a", message.Date, message.Time, message.Contents));

                        if (MessageParser.IsMessageIdenticalToAnotherOne(tempMessage, deletionMessage))
                        {
                            messageToDelete = message;
                            chatInWhichToDelete = chat;
                            break;
                        }
                    }
                }
                if (messageToDelete is not null) chatInWhichToDelete?.Messages?.Remove(messageToDelete);
            }
        }






        #endregion API





        #region LOGIC



        /// <summary>
        /// 'True' - if message already present in dto, otherwise 'false'.
        /// <br />
        /// "True" - если сообщение присутствует в dto, иначе "false".
        /// </summary>
        private static bool IsMessageAlreadyPresent(IMessage message, ref UserServerSideDTO UserTracked)
        {
            bool bRes = false;

            lock (ClientMessengerWindowViewModel.synchronizer)
            {
                if (IsChatAlreadyPresent(message, ref UserTracked))
                {
                    UserTracked.ChatArray.ForEach((chat) =>
                    {
                        chat.Messages.ForEach((msg) =>
                        {
                            IMessage tempMessage = JsonConvert.DeserializeObject<JsonMessagePackage>(JsonMessageFactory.GetJsonMessage(msg.Sender, "n/a", msg.Date, msg.Time, msg.Contents));

                            if (MessageParser.IsMessageIdenticalToAnotherOne(tempMessage, message))
                            {
                                bRes = true;
                            }
                        });
                    });
                }
            }

            return bRes;
        }


        /// <summary>
        /// 'True' - if chat of the message already present in dto, otherwise 'false'.
        /// <br />
        /// "True" - если чат сообщения присутствует в dto, иначе "false".
        /// </summary>
        private static bool IsChatAlreadyPresent(IMessage newMessage, ref UserServerSideDTO UserTracked)
        {
            return GetChatWithMessage(newMessage, ref UserTracked) is not null;
        }




        /// <summary>
        /// Get the chat containing the passed message.
        /// <br />
        /// Получить чат, содержащий переданное сообщение.
        /// </summary>
        private static ChatDTO? GetChatWithMessage(IMessage message, ref UserServerSideDTO UserTracked)
        {
            ChatDTO? chatResult = null;

            bool doAllMembersMatch = default;

            foreach (var chat in UserTracked.ChatArray)
            {
                doAllMembersMatch = false;
                foreach (var member in chat.Members)
                {
                    if (!(member.Equals(message.GetSender()) || member.Equals(message.GetReciever())))
                    {
                        doAllMembersMatch = false;
                        break;
                    }
                    else doAllMembersMatch = true;
                }

                if (doAllMembersMatch)
                {
                    chatResult = chat;
                    break;
                }
            }

            return chatResult;
        }



        #endregion LOGIC


    }
}
