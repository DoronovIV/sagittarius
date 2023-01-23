using MessengerService.Model.Enums;

namespace MessengerService.Model.Entities
{
    [Serializable]
    public class User
    {
        
        public int Id { get; set; }

        public string PublicId { get; set; } = null!;

        public string CurrentNickname { get; set; } = null!;

        public string Login { get; set; } = null!;

        public List<Chat>? ChatList { get; set; }

        public List<Message>? MessageList { get; set; }


        /// <summary>
        /// Default constructor.
        /// <br />
        /// Конструктор по умолчанию.
        /// </summary>
        public User()
        {
            ChatList = new();
            MessageList = new();
        }


        /// <summary>
        /// Message paranetrized constructor.
        /// 'role' - depends on creation context. 
        /// <br />
        /// Параметризованный конструктор по сообщению.
        /// "role" - зависит от контекста создания.
        /// </summary>
        public User(IMessage message, UserRoles role) : base()
        {
            switch (role)
            {
                case UserRoles.Sender:
                    CurrentNickname = PublicId = message.GetSender();
                    break;

                case UserRoles.Reciever:
                    CurrentNickname = PublicId = message.GetReciever();
                    break;
            }
        }
    }
}
