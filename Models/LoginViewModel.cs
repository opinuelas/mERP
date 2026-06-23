using System.ComponentModel.DataAnnotations;

namespace mERP.Models;

public class LoginViewModel
{
    [Required]
    [Display(Name = "User ID")]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}
