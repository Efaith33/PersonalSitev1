using PersonalSite1.UI.MVC.Models;
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace PersonalSite1.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        public JsonResult ContactAjax(ContactViewModel cvm)
        {
            // You can make this whatever you want, it will be the body of the message sent
            string body = $"{cvm.Name} has sent you the following message: <br/>" + $"{cvm.Message} from the email address: {cvm.Email}";

            // Message Object
            MailMessage mm = new MailMessage
                (
                // FROM address - email must be on host - creds stored in Web.config
                ConfigurationManager.AppSettings["EmailUser"].ToString(),
                // To - email doesn't have to be on host - creds stored in Web.config
                ConfigurationManager.AppSettings["EmailTo"].ToString(),
                // Email subject
                cvm.Subject,
                // Body of the email
                body);

            // Allow HTML in email (That is our formatting with br tag above)
            mm.IsBodyHtml = true;
            // You can make the message be designated as high priority
            mm.Priority = MailPriority.High;
            // Reply to the Person who filled out the form, not your domain email
            mm.ReplyToList.Add(cvm.Email);

            // Configure the mail client - creds stored in web.config
            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["EmailClient"].ToString());
            // Configure the email credentials using values from web.config
            client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailUser"].ToString(), ConfigurationManager.AppSettings["EmailPass"].ToString());

            try
            {
                // Send Mail
                client.Send(mm);
            }
            catch (Exception ex)
            {
                // Log error in ViewBag to be seen by admins
                ViewBag.Message = ex.StackTrace;
                return Json(HttpStatusCode.BadRequest);
            }

            return Json(cvm);

        }

        public ActionResult Index()
        {
            return View();
        }
    }
}

        //    [HttpPost]
        //    public ActionResult Contact(ContactViewModel cvm)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return View(cvm);
        //        }

        //        string message = $"You have received an email from {cvm.Name} with a subject of {cvm.Subject}. Please respond to {cvm.Email} with your response to the following message: <br/>{cvm.Message}";

        //        MailMessage mm = new MailMessage(
        //            ConfigurationManager.AppSettings["EmailUser"].ToString(),
        //            ConfigurationManager.AppSettings["EmailTo"].ToString(),
        //            cvm.Subject,
        //            message);

        //        mm.IsBodyHtml = true;
        //        mm.Priority = MailPriority.High;
        //        mm.ReplyToList.Add(cvm.Email);


        //        SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["EmailClient"].ToString());

        //        client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailUser"].ToString(), 
        //            ConfigurationManager.AppSettings["EmailPass"].ToString());

        //        try
        //        {
        //            client.Send(mm);
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.CustomerMessage = $"We're sorry your request could not be completed at this time. Please try again later. Error Message: {ex.Message}<br/>{ex.StackTrace}";
        //            return View(cvm);
        //        }

        //        return View("EmailConfirmation", cvm);

        //    }
        //}
    