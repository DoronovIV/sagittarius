using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Packages
{
    /// <summary>
    /// A protocol that consolidates entities that carry info about sender id, reciever id, date, time and contents of the message.
    /// <br />
    /// Протокол, который объединяет сущности, которые содержат информацию об id отправителя, id получателя, дате, времени и содержимом сообщения.
    /// </summary>
    public interface IMessage
    {

        public string GetSender();

        public string GetReciever();

        public string GetDate();

        public string GetTime();

        public object GetMessage();


    }
}
