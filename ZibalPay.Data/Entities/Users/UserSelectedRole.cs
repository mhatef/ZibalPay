using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZibalPay.Data.Entities.Users
{
    public class UserSelectedRole
    {
        public UserSelectedRole()
        {
            
        }

        [Key]
        public int USR_Id { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }

        #region Relations

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }


        #endregion

    }
}
