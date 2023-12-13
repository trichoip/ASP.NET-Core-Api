using System.ComponentModel;

namespace AspNetCore.Authentication.Identity.DTOs;

public class DeletePersonalDataModel
{
    [DefaultValue("aA123456!")]
    public string? Password { get; set; }
}
