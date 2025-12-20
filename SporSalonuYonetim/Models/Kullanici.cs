using Microsoft.AspNetCore.Identity;

namespace SporSalonuYonetim.Models
{
    public class Kullanici : IdentityUser
    {
        public string? FullName { get; set; }
    }
}