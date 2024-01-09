using Microsoft.AspNetCore.Identity;

namespace AuthJwt.Infrastructure.Model;

public class CustomIdentityUser : IdentityUser<int>
{
    public virtual ICollection<IdentityUserRole<int>> UserRoles { get; set; }
}
