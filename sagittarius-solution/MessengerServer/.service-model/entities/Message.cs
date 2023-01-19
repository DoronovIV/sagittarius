using Tools.Formatting;
using Tools.Toolbox;

namespace MessengerService.Model.Entities
{
    public class Message : IMessage
    {

        public int Id { get; set; }


        /// <summary>
        /// Contents of the message.
        /// <br />
        /// Содержимое сообщения.
        /// </summary>
        public string Contents { get; set; }


        /// <summary>
        /// Db id of the Author of the message.
        /// <br />
        /// ID б/д автора сообщения.
        /// </summary>
        public int AuthorId { get; set; }


        /// <summary>
        /// Date of the message sending.
        /// <br />
        /// Дата отправки сообщения.
        /// </summary>
        public string Date { get; set; } = null!;


        /// <summary>
        /// Time of the message sending.
        /// <br />
        /// Время отправки сообщения.
        /// </summary>
        public string Time { get; set; } = null!;


        /// <summary>
        /// A reference to the Author of the message.
        /// <br />
        /// Ссылка на автора сообщения.
        /// </summary>
        public User Author { get; set; } = null!;


        /// <summary>
        /// A reference to the chat of the message.
        /// <br />
        /// Ссылка на чат сообщения.
        /// </summary>
        public Chat Chat { get; set; } = null!;


        /// <summary>
        /// Default constructor.
        /// <br />
        /// Конструктор по умолчанию.
        /// </summary>
        public Message()
        {

        }



        #region IMessage


        public string GetSender()
        {
            return Author.PublicId;
        }


        public string GetReciever()
        {
            throw new NotImplementedException();
        }


        public string GetDate()
        {
            return Date;
        }


        public string GetTime()
        {
            return Time;
        }


        public object GetMessage()
        {
            return Contents;
        }


        #endregion IMessage

    }
}
