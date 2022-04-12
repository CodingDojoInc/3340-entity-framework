using System;
using System.ComponentModel.DataAnnotations;

namespace BankAccounts.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId {get;set;}
        public int UserId {get;set;}
        [Required]
        [Display(Name="Diposit/Withdraw")]
        public double Amount {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;

    }
}