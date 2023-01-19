using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tools.Formatting;

namespace Common.Objects.Common
{
    /// <summary>
    /// A simple representation of a server user entity instance that needs to be sent to client. This instance includes the information that might NOT be shown to the user.
    /// <br />
    /// Простое представление экземпляра серверной сущности пользователя, которое должно быть отправлено клиенту. Данный экземпляр включает в себя информацию, которая НЕ может быть продемонстрированна пользователю.
    /// </summary>
    public class UserServerSideDTO
    {

        public string CurrentNickname { get; set; }

        public string CurrentPublicId { get; set; }

        public List<ChatDTO>? ChatArray { get; set; }


        /// <summary>
        /// Default constructor.
        /// <br />
        /// Конструктор по умолчанию.
        /// </summary>
        public UserServerSideDTO()
        {
            ChatArray= new();
        }



        /// <summary>
        /// Sort the list of the messages that comes from the database DbSet.
        /// <br />
        /// Отсортировать список сообщений, который приходит из DbSet'а б/д.
        /// </summary>
        public void SortMessageList()
        {
            foreach (ChatDTO chat in ChatArray)
            {
                chat.Messages.Sort((MessageDTO A, MessageDTO B) =>
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

    }
}
