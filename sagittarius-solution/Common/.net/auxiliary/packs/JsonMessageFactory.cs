using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Assets.Misc;

namespace Common.Packages
{
    /// <summary>
    /// An instance to simplify working with json messages.
    /// <br />
    /// Объект для упрощения работы с json сообщениями.
    /// </summary>
    public static class JsonMessageFactory
    {


        /// <summary>
        /// Get a message without specific date and time properties in cases when one does not need it.
        /// <br />
        /// Получить сообщение без конкретной даты и времени, в случае, когда некто не нуждается в этом.
        /// </summary>
        public static string GetJsonMessageSimplified(string sender, string reciever, object? message)
        {
            return GetJsonMessage(sender, reciever, StringAssets.NonAccessable, StringAssets.NonAccessable, message);
        }


        /// <summary>
        /// Get fully initialized serialized json message. In case of serialization issues, throws an InvalidDataException.
        /// <br />
        /// Получить полностью проинициализированное json сообщение. В случае проблем с сериализацией, бросает InvalidDataException.
        /// </summary>
        public static string GetJsonMessage(string sender, string reciever, string date, string time, object? message)
        {
            string sRes = string.Empty;


            JsonMessagePackage package = new(sender, reciever, date, time, message);

            sRes = JsonConvert.SerializeObject(package);

            // json parsing issues check
            if (sRes.Equals(string.Empty))
            {
                throw new InvalidDataException($"[Custom] something went wrong. Result was {sRes}.");
            }

            return sRes;
        }


        /// <summary>
        /// Serialize initialized json message.
        /// <br />
        /// Сериализовать инициализированное json сообщение.
        /// </summary>
        public static string GetSerializedMessage(JsonMessagePackage unserializedMessage)
        {
            return GetJsonMessage
            (
                sender: unserializedMessage.Sender, 
                reciever: unserializedMessage.Reciever, 
                date: unserializedMessage.Date,
                time: unserializedMessage.Time,
                message: unserializedMessage.Message
            );
        }


        /// <summary>
        /// Deserialize raw json message into the packaged one.
        /// <br />
        /// Десериализовать сырое json сообщение в json пакет.
        /// </summary>
        public static JsonMessagePackage GetUnserializedPackage(string serializedJsonString)
        {
            JsonMessagePackage jmpRes = JsonConvert.DeserializeObject<JsonMessagePackage>(serializedJsonString, new JsonSerializerSettings() { DateFormatString = StringAssets.DateFormat });

            return jmpRes;
        }


    }
}
