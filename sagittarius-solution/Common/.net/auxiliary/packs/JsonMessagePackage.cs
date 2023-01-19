using Newtonsoft.Json;
using Tools.Formatting;

namespace Common.Packages
{
    /// <summary>
    /// The main text message protocol.
    /// <br />
    /// Основной протокол текстовых сообщений.
    /// </summary>
    public class JsonMessagePackage : IMessage
    {


        /// <inheritdoc cref="Sender"/>
        protected string _sender;


        /// <inheritdoc cref="Reciever"/>
        protected string _reciever;


        /// <inheritdoc cref="Date"/>
        protected string _date;


        /// <inheritdoc cref="Time"/>
        protected string _time;


        /// <inheritdoc cref="Message"/>
        protected object? _message;



        /// <summary>
        /// Sender nickname.
        /// <br />
        /// Никнейм отправителя.
        /// </summary>
        public string Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }


        /// <summary>
        /// Reciever nickname.
        /// <br />
        /// Никнейм получателя.
        /// </summary>
        public string Reciever
        {
            get { return _reciever; }
            set { _reciever = value; }
        }


        /// <summary>
        /// The message sending date.
        /// <br />
        /// Дата отправки сообщения.
        /// </summary>
        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }


        /// <summary>
        /// The message sending time.
        /// <br />
        /// Время отправки сообщения.
        /// </summary>
        public string Time
        {
            get { return _time; }
            set { _time = value; }
        }


        /// <summary>
        /// A text message string.
        /// <br />
        /// Строка текстового(?) сообщения.
        /// </summary>
        public virtual object? Message
        {
            get { return _message; }
            set { _message = value; }
        }




        /// <summary>
        /// Parametrized constructor.
        /// <br />
        /// Параметризованный конструктор.
        /// </summary>
        public JsonMessagePackage(string sender, string reciever, string date, string time, object? message)
        {
            Sender = sender;
            Reciever = reciever;
            Date = date;
            Time = time;
            Message = message;
        }



        /// <summary>
        /// Default constructor.
        /// <br />
        /// Конструктор по умолчанию.
        /// </summary>
        public JsonMessagePackage()
        {

        }







        #region IMessage


        public string GetSender()
        {
            return _sender;
        }


        public string GetReciever()
        {
            return _reciever;
        }


        public string GetDate()
        {
            return _date;
        }


        public string GetTime()
        {
            return _time;
        }


        public object GetMessage()
        {
            return _message;
        }


        #endregion IMessage


    }
}
