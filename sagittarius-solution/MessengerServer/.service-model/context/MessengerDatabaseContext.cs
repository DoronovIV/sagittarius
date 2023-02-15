using MessengerService.Model.Configs;
using Microsoft.Extensions.Configuration;

namespace MessengerService.Model.Context
{
    public class MessengerDatabaseContext : DbContext
    {

        #region STATE



        public DbSet<User> Users { get; set; } = null!; 


        public DbSet<Message> Messages { get; set; } = null!;


        public DbSet<Chat> Chats { get; set; } = null!;



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
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
            modelBuilder.ApplyConfiguration(new ChatConfiguration());
        }



        #endregion OVERRIDES




        #region CONSTRUCTION



        /// <summary>
        /// Check existance of the d/b.
        /// <br />
        /// Проверить наличие б/д.
        /// </summary>  
        public bool CheckDatabase()
        {
            return Database.EnsureCreated();
        }



        /// <summary>
        /// Default constructor;
        /// <br />
        /// Конструктор по умолчанию;
        /// </summary>
        public MessengerDatabaseContext()
        {
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
        public MessengerDatabaseContext(DbContextOptions<MessengerDatabaseContext> options) : base(options)
        {
        }



        #endregion CONSTRUCTION
    }
}
