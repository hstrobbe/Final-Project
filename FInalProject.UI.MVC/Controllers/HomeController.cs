using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.UI.MVC.Models;
using System.Net.Mail;
using System.Net;


namespace FinalProject.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel cvm)
        {
            //when a class has validation attributes that validation should be checked before attempting to process any data.
            if (!ModelState.IsValid)
            {
                //send them back to the form by passing the object to the view, which returns the form with the orignial populated info.
                return View(cvm);
            }
            //only executes if the form object passes model validation

            //buildign the message. This is what we see when we recieve the email
            string Message = $"You have recieved an email from {cvm.Name} with a subject {cvm.Subject}. Please respond to {cvm.Email} with your response to the following message: <br/>{cvm.Message}";

            //MailMessage is what sends the email (using system.net.mail)
            MailMessage mm = new MailMessage(
               //From
               "admin@hannahstrobbe.com",
               //To is this assumes forwarding by the host (you@yourdomain.ext)
               "hannahstrobbe@outlook.com",//hard code till smarter asp works around the forwarding issue
                                           //subject
               cvm.Subject,
               //body
               Message
                );
            //MailMessage properties
            //Allow HTML formatting in the email (message has html in it)
            mm.IsBodyHtml = true;

            //if you want to mark emails with high proirity
            mm.Priority = MailPriority.High;//the default is normal

            //respond to the senders email instead of our own smtp client (webmail)
            mm.ReplyToList.Add(cvm.Email);

            //SMTP Client  this is the info from the host smartasp.net 
            //this will allow the email to actually be sent
            SmtpClient client = new SmtpClient("mail.hannahstrobbe.com");

            //client credentionals smartasp requires your username and password
            client.Credentials = new NetworkCredential("admin@hannahstrobbe.com", "GSTWm245.");

            //it is possible that the mail server is down or may have config issues. we want to incapsolate in a try catch
            try
            {
                //attempt to send email
                client.Send(mm);
            }
            catch (Exception ex)
            {
                ViewBag.CustomerMessage = $"we are sorry your request could not be completed at this time. Please try again later. Error Message: <br/> {ex.StackTrace}";

                //return the view with the entire message so the users can copy and paste for late
                return View(cvm);
            }

            //if all goes well we will return a view that displays a conformation to the end user that the email was sent
            return View("EmailConformation", cvm);
        }
        [HttpGet]
        public ActionResult Admin()
        {
            ViewBag.Message = "Your admin page.";

            return View();
        }
    }
}
