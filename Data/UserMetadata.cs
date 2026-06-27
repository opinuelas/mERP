using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace mERP.Data;

[ModelMetadataType(typeof(UserMetadata))]
public partial class User { }

public class UserMetadata
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Need to specify a user id")]
    public string? Usrid { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Need to specify a user name")]
    public string? Usrname { get; set; }

    [Required(ErrorMessage = "Need to select a role")]
    public int? Usrrole { get; set; }
}
