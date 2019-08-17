using System.ComponentModel.DataAnnotations;

namespace newProjectApp.API.DTOS
{
    public class UserRegisterDTO
    {
        [Required]
        public string username { get; set; }
        [StringLength(8,MinimumLength=4,ErrorMessage="لا يمكن ان يقل الباسورد عن اربعة وان لا يزيد عن ثمانية حروف")]
        public string password { get; set; }
    }
}