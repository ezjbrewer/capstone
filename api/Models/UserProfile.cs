using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Sandwich.Models;
public class UserProfile
{
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }
    [NotMapped]
    public string UserName { get; set; }
    [NotMapped]
    public string Email { get; set; }
    [NotMapped]
    public string Address { get; set; }
    public DateTime CreateDateTime { get; set; }
    [NotMapped]
    public List<string> Roles { get; set; }
    public string IdentityUserId { get; set; }
    public IdentityUser IdentityUser { get; set; }
    public string FullName
    {
        get
        {
            return $"{FirstName} {LastName}";
        }
    }
}