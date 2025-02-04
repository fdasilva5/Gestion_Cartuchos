using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public string Usuario { get; set; }
    public string NombreCompleto { get; set; }
}
