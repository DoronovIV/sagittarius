using AuthorizationServiceProject.Model.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationServiceProject.Model.Configs
{
    public class UserModelConfiguration : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> userModelBuilder)
        {
            userModelBuilder.HasKey(u => u.Id);

            userModelBuilder.Property(u => u.Login).IsRequired();
            userModelBuilder.Property(u => u.PasswordHash).IsRequired();
        }
    }
}
