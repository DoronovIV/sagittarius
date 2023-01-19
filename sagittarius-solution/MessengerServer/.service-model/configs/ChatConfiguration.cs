namespace MessengerService.Model.Configs
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {

        public void Configure(EntityTypeBuilder<Chat> chatBuilder)
        {
            chatBuilder.HasKey(x => x.Id);

            chatBuilder.HasMany(c => c.UserList).WithMany(u => u.ChatList);
            chatBuilder.HasMany(c => c.MessageList).WithOne(m => m.Chat);
        }

    }
}
