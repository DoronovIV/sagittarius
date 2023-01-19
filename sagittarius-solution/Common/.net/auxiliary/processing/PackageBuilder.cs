using Common.Packages;
using System.Text;

namespace Common.Processing
{
    /// <summary>
    /// A service that forms packages for communication.
    /// <br />
    /// Сервис, который формирует пакеты для коммуникации.
    /// </summary>
    public class PackageBuilder
    {


        #region PROPERTIES


        /// <summary>
        /// A reference to the stream instance (in this case, client stream);
        /// <br />
        /// Ссылка на экземпляр стрима (в данном случае, стим клиента);
        /// </summary>
        MemoryStream _memoryStream;


        #endregion PROPERTIES





        #region API - public Contract


        /// <summary>
        /// Write operation code to the stream;
        /// <br />
        /// Передать код операции в стрим;
        /// </summary>
        /// <param name="opCode">
        /// Operation code;
        /// <br />
        /// Код операции;
        /// </param>
        public void WriteOpCode(byte opCode)
        {
            _memoryStream.WriteByte(opCode);
        }



        /// <summary>
        /// Write raw json message into byte array to travel the network.
        /// <br />
        /// Записать сырое json сообщения в массив байтов для передачи по сети.
        /// </summary>
        public void WriteJsonMessage(string jsonMessage)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(jsonMessage);

            _memoryStream.Write(BitConverter.GetBytes(bytes.Length));
            _memoryStream.Write(bytes);
        }


        /// <summary>
        /// Write serialized data;
        /// <br />
        /// Записать сериализованные данные;
        /// </summary>
        /// <param name="span">
        /// Binary data;
        /// <br />
        /// Бинарные данные;
        /// </param>
        public void WriteByteSpan(Span<byte> span)
        {
            _memoryStream.Write(BitConverter.GetBytes(span.Length));
            _memoryStream.Write(span);
        }


        /// <summary>
        /// Get all packet bytes including code, message length and the message itself;
        /// <br />
        /// Получить байты пакета, включая код, длину сообщения и само сообщение;
        /// </summary>
        /// <returns>
        /// Packet in a byte array;
        /// <br />
        /// Содержимое пакета в виде массова байтов;
        /// </returns>
        public byte[] GetPacketBytes()
        {
            return _memoryStream.ToArray();
        }


        #endregion API - public Contract





        #region CONSTRUCTION


        /// <summary>
        /// Default constructor;
        /// <br />
        /// Конструктор по умолчанию;
        /// </summary>
        public PackageBuilder()
        {
            _memoryStream = new MemoryStream();
        }


        #endregion CONSTRUCTION


    }
}
