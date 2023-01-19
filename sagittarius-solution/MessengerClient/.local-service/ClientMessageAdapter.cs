namespace MessengerClient.Model
{
    /// <summary>
    /// An adapter for message package - list view item data flow.
    /// <br />
    /// Адаптер для обмена данными между пакетами сообщений и элементами контрола list view.
    /// </summary>
    public static class ClientMessageAdapter
    {

        /// <summary>
        /// Parse message from list view controll format to it's actuall raw content w/o metadata (author, time).
        /// <br />
        /// Спарсить сообщение из формата списка к виду строки самого сообщения без метаданных (автор, время).
        /// </summary>
        public static string FromListViewChatToPackage(string chatMessage)
        {
            string sRes = string.Empty;
            int nColonIndex = 0;

            for (int i = 0, iSize = chatMessage.Length - 3; i < iSize; i++)
            {
                if (chatMessage[i].Equals(':') && chatMessage[i + 1].Equals(' '))
                {
                    nColonIndex = i + 1;
                    break;
                }
            }

            for (int i = 0, iSize = chatMessage.Length - 3; i < iSize; i++)
            {
                if (i > nColonIndex)
                    sRes += chatMessage[i];
            }
            return sRes;
        }



        /// <summary>
        /// Parse message from package to the list view controll format adding metadata (for the client window owner).
        /// <br />
        /// Спарсить сообщение из пакета в формат контрола list view, добавив метаданные (для владельца окна клиента).
        /// </summary>
        public static string FromPackageToChatListViewCurrentUser(IMessage packageMessage)
        {
            return $"[{StringDateTime.FromThreeToTwoSections(packageMessage.GetTime())}] " + $"{packageMessage.GetSender()}: " + packageMessage.GetMessage() + " ✓✓";
        }


        /// <summary>
        /// Parse message from package to the list view controll format adding metadata (for the client window owner adressee).
        /// <br />
        /// Спарсить сообщение из пакета в формат контрола list view, добавив метаданные (для собеседника владельца окна клиента).
        /// </summary>
        public static string FromPackageToChatListViewOtherUser(IMessage packageMessage)
        {
            return $"[{StringDateTime.FromThreeToTwoSections(packageMessage.GetTime())}] " + $"{packageMessage.GetSender()}: " + packageMessage.GetMessage();
        }

    }
}
