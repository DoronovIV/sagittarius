using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Objects.Common
{
    /// <summary>
    /// A simple representation of a server message entity instance that needs to be sent to client. It was created after discovery of the newtonsoft recursive serialization issue.
    /// <br />
    /// Простое представление экземпляра серверной сущности сообщения, которое должно быть отправлено клиенту. Оно было создано после обнаружения проблемы рекурсивной сериализации у newtonsoft.
    /// </summary>
    public class MessageDTO
    {
        public string Sender { get; set; }

        public string Contents { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }


        /// <summary>
        /// Default constructor.
        /// <br />
        /// Конструктор по умолчанию.
        /// </summary>
        public MessageDTO()
        {

        }


        public MessageDTO(string sender, string contents, string date, string time)
        {
            Sender = sender;
            Contents = contents;
            Date = date;
            Time = time;
        }

    }
}
