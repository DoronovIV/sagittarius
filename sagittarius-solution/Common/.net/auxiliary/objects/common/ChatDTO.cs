using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Objects.Common
{
    /// <summary>
    /// A simple representation of a server chat entity instance that needs to be sent to client. It was created after discovery of the newtonsoft recursive serialization issue.
    /// <br />
    /// Простое представление экземпляра серверной сущности чата, которое должно быть отправлено клиенту. Оно было создано после обнаружения проблемы рекурсивной сериализации у newtonsoft.
    /// </summary>
    public class ChatDTO
    {


        /// <summary>
        /// The list of the members.
        /// <br />
        /// Список участников.
        /// </summary>
        public List<string>? Members { get; set; }


        /// <summary>
        /// The list of the messages.
        /// <br />
        /// Список сообщений.
        /// </summary>
        public List<MessageDTO>? Messages { get; set; }


        /// <summary>
        /// Default constructor.
        /// <br />
        /// Конструктор по умолчанию.
        /// </summary>
        public ChatDTO()
        {
            Members = new List<string>();
            Messages = new List<MessageDTO>();
        }

    }
}
