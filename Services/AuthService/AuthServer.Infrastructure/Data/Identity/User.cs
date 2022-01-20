using Microsoft.AspNetCore.Identity;

namespace AuthServer.Infrastructure.Data.Identity
{
    public class User : IdentityUser
    {
        // Add additional profile data for application users by adding properties to this class
        public string Name { get; set; }
    }
}
