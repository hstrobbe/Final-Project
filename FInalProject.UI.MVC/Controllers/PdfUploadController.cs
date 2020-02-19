using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
namespace FInalProject.UI.MVC.Controllers
{
    public class PdfUploadController : Controller
    {
        // GET: Upload  
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult PdfUploadFile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PdfUploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Content/PDF/"), _FileName);
                    file.SaveAs(_path);
                }
                ViewBag.Message = "File Uploaded Successfully!!";
                return View();
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return View();
            }
        }
    }
}  
