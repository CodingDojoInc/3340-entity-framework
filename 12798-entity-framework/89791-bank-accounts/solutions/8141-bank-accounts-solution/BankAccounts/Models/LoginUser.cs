#pragma warning disable CS8618
public class LoginUser
{
    [EmailAddress]
    [Required]
    [Display(Name = "Email")]
    public string EmailAttempt { get; set; }
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string PasswordAttempt { get; set; }
}