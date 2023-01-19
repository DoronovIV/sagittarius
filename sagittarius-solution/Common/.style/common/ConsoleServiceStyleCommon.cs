using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Style.Common
{
    public static class ConsoleServiceStyleCommon
    {


        /// <summary>
        /// Get the spectre markup string containing greeting.
        /// <br />
        /// Получить строку типа Spectre.Markup, содержащую приветствие.
        /// </summary>
        public static string GetAuthorizerGreeting()
        {
            string tenSpacesTabChar = string.Empty;
            tenSpacesTabChar += "[black on white]   GTM+3  [/] ";
            return $"{tenSpacesTabChar}[skyblue2]Welcome to [/][purple_1]Authorizer[/][skyblue2] service. Listenning started.[/]\n";
        }


        /// <summary>
        /// Get the spectre markup string containing greeting.
        /// <br />
        /// Получить строку типа Spectre.Markup, содержащую приветствие.
        /// </summary>
        public static string GetMessengerGreeting()
        {
            string tenSpacesTabChar = string.Empty;
            tenSpacesTabChar += "[black on white]   GTM+3  [/] ";
            return $"{tenSpacesTabChar}[skyblue2]Welcome to [/][red]Messenger[/][skyblue2] service. Listenning started.[/]\n";
        }



        /// <summary>
        /// Get the spectre markup string containing current time in format of 'HH:mm:ss'.
        /// <br />
        /// Получить строку типа Spectre.Markup, содержащую текущее время в формате "HH:mm:ss".
        /// </summary>
        public static string GetCurrentTime()
        {
            return $"[black on white][[{DateTime.Now.ToString("HH:mm:ss")}]][/]";
        }



        /// <summary>
        /// Get the spectre markup string containing user connection message.
        /// <br />
        /// Получить строку типа Spectre.Markup, содержащую сообщение о подключении пользователя.
        /// </summary>
        public static string GetUserConnection(string userLogin)
        {
            return $"{ConsoleServiceStyleCommon.GetCurrentTime()} user has [underline]connected[/] with login [green]\"{userLogin}\"[/].\n";
        }



        /// <summary>
        /// Get the spectre markup string containing user disconnection message.
        /// <br />
        /// Получить строку типа Spectre.Markup, содержащую сообщение об дисконнекте пользователя.
        /// </summary>
        public static string GetUserDisconnection(string userLogin)
        {
            return $"{ConsoleServiceStyleCommon.GetCurrentTime()} user [green]\"{userLogin}\"[/] has [underline]disconnected[/].\n";
        }


    }
}
