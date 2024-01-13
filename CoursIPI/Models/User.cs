using System.ComponentModel.DataAnnotations;

namespace CoursIPI.Models;

public class User
{
    [Key] // Primary key
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

}
