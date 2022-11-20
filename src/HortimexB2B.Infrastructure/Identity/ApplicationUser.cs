using Microsoft.AspNetCore.Identity;

namespace HortimexB2B.Infrastructure.Identity
{
    public class ApplicationUser: IdentityUser
    {
        public int GraffitiId { get; set; }
    }
}
