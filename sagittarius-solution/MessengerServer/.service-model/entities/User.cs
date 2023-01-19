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
    }
}
