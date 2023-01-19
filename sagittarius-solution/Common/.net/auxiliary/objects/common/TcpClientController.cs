using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    /// <summary>
    /// An instance that helps disposing the tcp clients.
    /// <br />
    /// Объект, который помогает диспозить tcp клиентов.
    /// </summary>
    public class TcpClientController
    {


        /// <summary>
        /// The list of clients.
        /// <br />
        /// Массив клиентов.
        /// </summary>
        private List<TcpClient>? _clients;



        /// <summary>
        /// The concurrency safe list.
        /// <br />
        /// Потокобезопасный массив.
        /// </summary>
        private ArrayList _mainList;



        /// <summary>
        /// Default constructor.
        /// <br />
        /// Конструктор по умолчанию.
        /// </summary>
        public TcpClientController()
        {
            _clients = new ();
            _mainList = new (_clients);
        }



        /// <summary>
        /// Adds a TcpClient to the list.
        /// <br />
        /// Добавляет TcpClient в массив.
        /// </summary>
        public void RegisterClient(TcpClient? newClient)
        {
            if (newClient is not null)
            {
                if (!_mainList.Contains(newClient)) _mainList.Add(newClient);
            }

            else throw new NullReferenceException("[Manual] The client you have tried to add was null. Seek for its initialization or remove the triggering instruction.");
        }



        /// <summary>
        /// Close all the clients registered. Returns 'true' if there were exceptions during the process, otherwise 'false'.
        /// <br />
        /// Закрыть все зарегистрированные клиенты. Возвращает "true", если при этом были исключения, иначе "false".
        /// </summary>
        public bool TryDisposeAll()
        {
            bool isThereAnyException = default;

            try
            {
                foreach (TcpClient item in _mainList)
                {
                    item.Close();
                }

                isThereAnyException = false;
            }

            catch
            {
                isThereAnyException = true;
            }


            return isThereAnyException;
        }


    }
}
