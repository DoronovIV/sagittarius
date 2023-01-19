using Common.Style.Common;

namespace Common.Style.Authorizer
{
    /// <summary>
    /// A bunch of style and markup features using 'Spectre.Console' nuget.
    /// <br />
    /// Набор стилей и разметок, использующий нюгет "Spectre.Console".
    /// </summary>
    public static class ConsoleServiceStyle
    {

        /// <summary>
        /// Get the spectre markup string containing user registration and connection message.
        /// <br />
        /// Получить строку типа Spectre.Markup, содержащую сообщение о регистрации и подключении пользователя.
        /// </summary>
        public static string GetUserRegistrationStyle(string userLogin)
        {
            return $"{ConsoleServiceStyleCommon.GetCurrentTime()} user has [underline]registered[/] with login [green]\"{userLogin}\"[/].\n";
        }

    }
}
