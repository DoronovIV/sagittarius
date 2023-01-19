using AuthorizationServiceProject.Model.Entities;
using AuthorizationServiceProject.Model.Configs;
using Microsoft.Extensions.Configuration;

namespace AuthorizationServiceProject.Model.Context
{
    public class AuthorizationDatabaseContext : DbContext
    {


        #region STATE


        public DbSet<UserModel> Users { get; set; } = null;


        #endregion STATE




        #region OVERRIDES



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());

            builder.AddJsonFile(".config/appsettings.json");

            var config = builder.Build();
            string _connectionString = config.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(_connectionString);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserModelConfiguration());
        }



        #endregion OVERRIDES





        #region CONSTRUCTION



        /// <summary>
        /// Default constructor;
        /// <br />
        /// Конструктор по умолчанию;
        /// </summary>
        public AuthorizationDatabaseContext()
        {
            Database.EnsureCreated();
        }


        /// <summary>
        /// Constructor with connection options configurations (in this case).
        /// <br />
        /// Конструктор с конфигурацией опций подключения (в данном случае).
        /// </summary>
        /// <param name="options">
        /// Connection options instance.
        /// <br />
        /// Экземпляр опций подключения.
        /// </param>
        public AuthorizationDatabaseContext(DbContextOptions<AuthorizationDatabaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }



        #endregion CONSTRUCTION


    }
}
