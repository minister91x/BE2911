using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BeAspNet.DataaAccress.DataObject
{
    public class User
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter UserName.")]
        [MaxLength(10)]
        public string UserName { get; set; }
        public string FUllName { get; set; }
        public string UserAddress { get; set; }
    }
}
