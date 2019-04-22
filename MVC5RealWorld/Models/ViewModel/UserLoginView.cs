﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC5RealWorld.Models.ViewModel
{
    public class UserLoginView
    {
        [Key]
        public int SYSUserID { get; set; }

        [Required(ErrorMessage = "Campo obrigatorio")]
        [Display(Name = "Login ID")]
        public string LoginName { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
