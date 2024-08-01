using Microsoft.AspNetCore.Identity;

namespace StoreRopa.Data.utils
{
    public class ApplicationUser : IdentityUser
    {
        public Int32 Id {  get; set; } 
        public string FullName { get; set; }
    }
}
