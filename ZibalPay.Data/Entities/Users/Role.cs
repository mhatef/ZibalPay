using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZibalPay.Data.Entities.Users
{
    public class Role
    {
        public Role()
        {
            
        }

        [Key]
        public int RoleId { get; set; }

        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} کارکتر باشد")]
        public string RoleTitle { get; set; }

        #region Relations

        public List<UserSelectedRole> UserSelectedRoles { get; set; }


        #endregion

    }
}
