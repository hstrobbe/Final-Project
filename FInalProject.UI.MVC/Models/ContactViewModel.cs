using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.UI.MVC.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "***Name is required***")]
        //the above validation sets name to required and customizes and error message that will populate in the validation message for name

        public string Name { get; set; }

        [Required(ErrorMessage = "***Email is required***")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Subject { get; set; }

        [Required(ErrorMessage = "***Message is required***")]
        [UIHint("MultilineText")]//provides a text are element rather than an input
        public string Message { get; set; }
    }
}