#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TheWall.Models;
public class LogUser
{
    [EmailAddress]
    public string LogEmail { get; set; }
    [DataType(DataType.Password)]
    public string LogPassword { get; set; }
}