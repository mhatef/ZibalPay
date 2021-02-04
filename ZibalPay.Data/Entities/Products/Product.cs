using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZibalPay.Data.Entities.Products
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(150, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} کارکتر باشد")]
        public string ProductTitle { get; set; }

        [Display(Name = "توضیحات محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(350, ErrorMessage = "مقدار {0} نمی تواند بیشتر از {1} کارکتر باشد")]
        public string ProductDescription { get; set; }

        [Display(Name = "تصویر محصول")]
        public string ProductImage { get; set; }

        [Display(Name = "قیمت محصول")]
        public int ProductPrice { get; set; }

        [Display(Name = "تاریخ ایجاد محصول")]
        public DateTime CreateDate { get; set; }
        
        #region Relations

        public List<OrderDetail> OrderDetails { get; set; }


        #endregion
    }
}
