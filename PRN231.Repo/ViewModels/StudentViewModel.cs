using System.ComponentModel.DataAnnotations;
using PRN231.Repo.ValidationCustom;

namespace PRN231.Repo.ViewModels;

public class StudentViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [RegularExpression(@"^[A-Z][a-z]*(\s[A-Z][a-z]*)*$")]
    public string FullName { get; set; }
    
    [Required(ErrorMessage = "Date of birth is required")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{dd-mm-yyyy}", ApplyFormatInEditMode = true)]
    [AgeRange(18, 100, ErrorMessage = "Age must be between 18 and 100")]
    public DateTime? DateOfBirth { get; set; }
    
    public int? GroupId { get; set; }
}