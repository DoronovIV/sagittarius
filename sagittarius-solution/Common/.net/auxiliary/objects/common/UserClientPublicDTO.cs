namespace Common.Objects.Common
{
    /// <summary>
    /// A simple representation of a server user entity instance that needs to be sent to client. This instance includes only the information that might be shown to the user.
    /// <br />
    /// Простое представление экземпляра серверной сущности пользователя, которое должно быть отправлено клиенту. Данный экземпляр включает в себя исключительно ту информацию, которая может быть продемонстрированна пользователю.
    /// </summary>
    public class UserClientPublicDTO
    {

        /// <summary>
        /// User name;
        /// <br />
        /// Имя пользователя;
        /// </summary>
        public string UserName { get; set; } = null!;


        /// <summary>
        /// User's unique id;
        /// <br />
        /// Уникальный идентификатор пользователя;
        /// </summary>
        public string PublicId { get; set; } = null!;



        /// <summary>
        /// Default constructor.
        /// <br />
        /// Конструктор по умолчанию.
        /// </summary>
        public UserClientPublicDTO()
        {

        }



        /// <summary>
        /// Default constructor.
        /// <br />
        /// Конструктор по умолчанию.
        /// </summary>
        public UserClientPublicDTO(string userName, string publicId)
        {
            UserName = userName;
            PublicId = publicId;
        }
    }
}
