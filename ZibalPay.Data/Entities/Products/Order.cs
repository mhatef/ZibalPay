using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZibalPay.Data.Entities.Users;

namespace ZibalPay.Data.Entities.Products
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int OrderSum { get; set; }
        public bool IsFinally { get; set; }
        public DateTime CreateDate { get; set; }

        #region Relations

        [ForeignKey("UserId")]
        public User User { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }


        #endregion
    }
}
