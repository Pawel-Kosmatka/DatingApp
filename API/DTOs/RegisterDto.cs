using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public record RegisterDto([Required] string UserName, [Required] string Password);
}