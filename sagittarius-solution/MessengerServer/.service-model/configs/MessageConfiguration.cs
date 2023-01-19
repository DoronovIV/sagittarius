namespace MessengerService.Model.Configs
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {

        public void Configure(EntityTypeBuilder<Message> messageBuilder)
        {
            messageBuilder.HasKey(m => m.Id);

            messageBuilder.Property(m => m.Date).IsRequired();
            messageBuilder.Property(m => m.Time).IsRequired();
            messageBuilder.Property(m => m.Contents).IsRequired();

            messageBuilder.HasOne(m => m.Author).WithMany(u => u.MessageList).HasForeignKey(m => m.AuthorId);
        }

    }
}
