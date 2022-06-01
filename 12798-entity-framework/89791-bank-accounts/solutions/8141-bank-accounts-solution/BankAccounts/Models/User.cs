#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAccounts.Models;
public class User
{
    [Key]
    public int UserId { get; set; }
    [MinLength(2, ErrorMessage = "First name must be at least 2 characters.")]
    [Required]
    public string FirstName { get; set; }
    [Required]
    [MinLength(2, ErrorMessage = "Last name must be at least 2 characters.")]
    public string LastName { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }


    public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    [Compare("Password")]
    [DataType(DataType.Password)]
    [NotMapped]
    public string Confirm { get; set; } = null!;

    // Will need to .Include() transactions to use Balance!
    public double Balance
    {
        get { return Transactions.Sum(t => t.Amount); }
    }

}