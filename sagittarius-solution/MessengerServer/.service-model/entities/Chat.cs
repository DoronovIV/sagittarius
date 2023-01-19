namespace MessengerService.Model.Entities
{
    public class Chat
    {

        public int Id { get; set; }


        public List<User> UserList { get; set; } = null!;


        public List<Message>? MessageList { get; set; }



        /// <summary>
        /// Default constructor.
        /// <br />
        /// Конструктор по умолчанию.
        /// </summary>
        public Chat()
        {
            UserList = new();
            MessageList = new();
        }
    }
}
