using System.ComponentModel;

namespace AspNetCore.Authentication.Identity.DTOs;

public class SetPasswordModel
{
    [DefaultValue("aA123456!")]
    public string NewPassword { get; set; }

    //[DefaultValue("aA123456!")]
    //[Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    //public string ConfirmPassword { get; set; }
}
