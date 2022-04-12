using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BankAccounts.Models
{
    public class BankUser
    {
        [Key]
        public int UserId {get;set;}
        [Display(Name="First Name")]
        [MinLength(2)]
        [Required]
        public string FirstName {get;set;}
        [Required]
        [MinLength(2)]
        [Display(Name="Last Name")]
        public string LastName {get;set;}
        [Required]
        [EmailAddress]
        public string Email {get;set;}
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password {get;set;}
        public List<Transaction> Transactions {get;set;} = new List<Transaction>();
        [Compare("Password")]
        [DataType(DataType.Password)]
        [NotMapped]
        public string Confirm {get;set;}

        // Will need to .Include() transactions to use Balance!
        public double Balance
        {
            get { return Transactions.Sum(t => t.Amount); }
        }
        
    }
    public class LoginUser
    {
        [EmailAddress]
        [Required]
        [Display(Name="Email")]
        public string EmailAttempt {get;set;}
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        public string PasswordAttempt {get;set;}
    }

}