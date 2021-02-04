using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZibalPay.Core.DTOs.User
{
    public class EditUserVM
    {
        public int UserId { get; set; }

        [Display(Name = "نام کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} کارکتر باشد")]
        public string UserFirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} کارکتر باشد")]
        public string UserLastName { get; set; }

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

    }

    public class LoginVm
    {
        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "شماره موبایل وارد شده صحیح نمی باشد")]
        public string UserPhoneNumber { get; set; }

        [Display(Name = "رمزعبور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }

    }

    public class LoginUserVm
    {
        public int UserId { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }

    }

    public class ChangeUserPasswordVm
    {
        public int UserId { get; set; }

        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(Name = "تکرار رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string RePassword { get; set; }

    }
}
