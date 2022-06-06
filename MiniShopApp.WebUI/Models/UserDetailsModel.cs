using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShopApp.WebUI.Models
{
    public class UserDetailsModel
    {
        public string UserId { get; set; }
        [Required(ErrorMessage ="First Name boş bırakılamaz")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name boş bırakılamaz")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "User Name boş bırakılamaz")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email boş bırakılamaz")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Lütfen geçerli bir email adresi giriniz")]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public IEnumerable<string> SelectedRoles { get; set; }
    }
}
