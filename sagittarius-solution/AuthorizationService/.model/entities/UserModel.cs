namespace AuthorizationServiceProject.Model.Entities
{
    public class UserModel
    {

        public int Id { get; set; }


        public string Login { get; set; } = null!;


        public string PasswordHash { get; set; } = null!;



        /// <summary>
        /// Default constructor.
        /// <br />
        /// Конструктор по умолчанию.
        /// </summary>
        public UserModel()
        {

        }

    }
}
