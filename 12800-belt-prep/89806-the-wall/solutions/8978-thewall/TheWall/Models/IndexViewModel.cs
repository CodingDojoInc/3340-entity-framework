#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace TheWall.Models;
public class IndexViewModel
{
    public Message NewMessage {get;set;}
    public Comment NewComment {get;set;}
}