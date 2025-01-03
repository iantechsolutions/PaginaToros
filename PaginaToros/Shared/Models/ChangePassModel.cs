﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaginaToros.Shared.Models
{
    public class ChangePassModel
    {
        [Display(Name = "Nombre de usuario"), Required(ErrorMessage = Utilities.MSGREQUIRED)]
        public string UserName { get; set; }

        [Display(Name = "Password actual"), Required(ErrorMessage = Utilities.MSGREQUIRED)]
        public string CurrentPass { get; set; }

        [Display(Name = "Nuevo Password"), Required(ErrorMessage = Utilities.MSGREQUIRED), MinLength(6, ErrorMessage = Utilities.MSGSTRINGlENGSHORT)]
        public string NewPass { get; set; }

        [Display(Name = "Confirmación Password"), Required(ErrorMessage = Utilities.MSGREQUIRED)]
        public string ConfirmPass { get; set; }
    }
}
