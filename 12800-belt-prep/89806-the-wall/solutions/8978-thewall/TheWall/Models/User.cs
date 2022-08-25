#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TheWall.Models;
public class User
{
    [Key]
    public int UserId { get; set; }
    
    [Required(ErrorMessage = "First name is required!")]
    public string First_Name { get; set; } 

    [Required(ErrorMessage = "Last name is required!")]
    public string Last_Name { get; set; } 

    [Required(ErrorMessage = "Email is required!")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required!")]
    [MinLength(8)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<Message> MyMessages {get;set;} = new List<Message>();
    public List<Comment> MyComments {get;set;} = new List<Comment>();

    [NotMapped]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string Confirm { get; set; }
}