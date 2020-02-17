using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FInalProject.UI.MVC.Models
{
    public class CourseCompletionEmailViewModel
    {
        

        public string Name { get; set; }

        public string Course { get; set; }
        
        public string Message { get; set; }
    }
}