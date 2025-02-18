using System.ComponentModel.DataAnnotations;
namespace MVCDHProject.Models
{
    public class ResetPasswordModel
    {
        [Required]
        public string UserId {  get; set; }
        [Required]
        public string Token {  get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name ="Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="The Confirm Password should Mathch with Password")]
        public string ConfirmPassword {  get; set; }
    }
}
