using Microsoft.AspNetCore.Identity;
namespace CA_2.Models;

// Inherit from identity user so I dont have to write password, and hashing
public class ApplicationUser : IdentityUser
{
    
}