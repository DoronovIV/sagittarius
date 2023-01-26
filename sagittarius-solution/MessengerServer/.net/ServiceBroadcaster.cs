namespace MessengerService.Datalink
{
    /// <summary>
    /// A service-instance for the 'ServiceController' that handles all its broadcasting.
    /// <br />
    /// Помощник класса "ServiceController", который занимается рассылками.
    /// </summary>
    public class ServiceBroadcaster
    {


        /// <summary>
        /// A reference to the service controller instance.
        /// <br />
        /// Ссылка на экземпляр объекта класса "ServiceController".
        /// </summary>
        private ServiceController controllerReference;





        #region API - public  Contract
        


        /// <summary>
        /// Broadcast a notification message to all users about the new userData connection;
        /// <br />
        /// Транслировать уведомление для всех пользователей о подключении нового пользователя;
        /// </summary>
        public void BroadcastConnection()
        {
            var broadcastPacket = new PackageBuilder();

            ValidateControllerUserList();
            foreach (var user in controllerReference.UserList)
            {
                foreach (var usr in controllerReference.UserList)
                {
                    // code '1' means new user has connected;
                    broadcastPacket.WriteOpCode(1);
                    broadcastPacket.WriteJsonMessage(JsonMessageFactory.GetJsonMessageSimplified(sender: usr.CurrentUser.PublicId, reciever: "Messenger", message: usr.CurrentUser.CurrentNickname));

                    user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
                }
            }
        }



        /// <summary>
        /// Send message to all users. Mostly use to deliver one userData's message to all other ones;
        /// <br />
        /// Отправить сообщение всем пользователям. В основном, используется, чтобы переслать сообщение одного пользователя всем остальным;
        /// </summary>
        /// <param name="message"></param>
        public void BroadcastMessage(JsonMessagePackage package)
        {
            var msgPacket = new PackageBuilder();
            msgPacket.WriteOpCode(5);
            //msgPacket.WritePackageLength(package);
            msgPacket.WriteJsonMessage(JsonMessageFactory.GetSerializedMessage(package));

            ValidateControllerUserList();
            foreach (var user in controllerReference.UserList)
            {
                if (user.CurrentUser.PublicId.Equals(package.Reciever) || user.CurrentUser.PublicId.Equals(package.Sender))
                {
                    user.ClientSocket.Client.Send(msgPacket.GetPacketBytes(), SocketFlags.Partial);
                }
            }
        }


        /// <summary>
        /// Broadcast a notification message to all users about some userData disconnection;
        /// <br />
        /// Транслировать уведомление для всех пользователей о том, что один из пользователей отключился;
        /// </summary>
        /// <param name="uid">
        /// id of the userData in order to find and delete him from the userData list;
        /// <br />
        /// id пользователя, чтобы найти его и удалить из списка пользователей;
        /// </param>
        public void BroadcastDisconnect(User userData)
        {
            // write notification message;
            var broadcastPacket = new PackageBuilder();
            broadcastPacket.WriteOpCode(10);    // on userData disconnection, _service recieves the code-10 operation and broadcasts the "disconnect message";  
            broadcastPacket.WriteJsonMessage(JsonMessageFactory.GetJsonMessageSimplified(userData.PublicId, "Everyone", userData.PublicId)); // it also sends disconnected userData id;

            ValidateControllerUserList();
            // send notification to everyone;
            foreach (var user in controllerReference.UserList)
            {
                user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes(), SocketFlags.Partial);
            }
        }



        /// <summary>
        /// Broadcast notification about one user's message deletion.
        /// <br />
        /// Распространить уведомление о том, что один пользователь удалил своё сообщение.
        /// </summary>
        public void BroadcastMessageDeletion(JsonMessagePackage message)
        {
            PackageBuilder builder = new();
            builder.WriteOpCode(6);
            builder.WriteJsonMessage(JsonMessageFactory.GetSerializedMessage(message));
            var specificChatUsers = controllerReference.UserList.Where(u => u.CurrentUser.PublicId.Equals(message.Sender) || u.CurrentUser.PublicId.Equals(message.Reciever));

            ValidateControllerUserList();
            foreach (var user in specificChatUsers)
            {
                user.ClientSocket.Client.Send(builder.GetPacketBytes());
            }
        }


        /// <summary>
        /// Broadcast service shutdown message to the users and authorizer _service.
        /// <br />
        /// Транслировать выключение сервиса пользователям и сервису авторизации.
        /// </summary>
        public void BroadcastShutdown()
        {
            PackageBuilder builder = new();
            builder.WriteOpCode(byte.MaxValue);
            ValidateControllerUserList();
            foreach (var user in controllerReference.UserList)
            {
                user.ClientSocket.Client.Send(builder.GetPacketBytes());
            }
        }



        #endregion API - public  Contract






        #region LOGIC



        /// <summary>
        /// Check the list of users and remove the disconnected ones.
        /// <br />
        /// Проверить список пользователей и удалить неподключённых.
        /// </summary>
        private void ValidateControllerUserList()
        {
            List<ServiceReciever> newList = new();
            foreach (var user in controllerReference.UserList)
            {
                if (user.ClientSocket.Client is not null)
                    newList.Add(user);
            }
            controllerReference.UserList = newList;
        }



        #endregion LOGIC






        /// <summary>
        /// Default constructor.
        /// <br />
        /// Конструктор по умолчанию.
        /// </summary>
        public ServiceBroadcaster(ServiceController controllerInstance)
        {
            controllerReference = controllerInstance;
        }


    }
}
