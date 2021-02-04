using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ZibalPay.Data.Entities.Products;

namespace ZibalPay.Data.Entities.Users
{
    public class User
    {
        public User()
        {
            
        }

        [Key]
        public int UserId { get; set; }

        [Display(Name = "نام کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} کارکتر باشد")]
        public string UserFirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} کارکتر باشد")]
        public string UserLastName { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string UserPassword { get; set; }

        [Display(Name = "جنسیت کاربر")]
        // true is female
        public bool UserGender { get; set; }

        [Display(Name = "شماره موبایل کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "شماره موبایل وارد شده صحیح نمی باشد")]
        public string UserPhoneNumber { get; set; }

        [Display(Name = "ایمیل کاربر")]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }

        [Display(Name = "آیا کاربر حذف شده")]
        public bool IsDelete { get; set; }

        [Display(Name = "آیا کاربر فعال است")]
        public bool IsActive { get; set; }


        #region Relations

        public List<UserSelectedRole> UserSelectedRoles { get; set; }
        public List<Order> Orders { get; set; }


        #endregion
    }
}
