using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Resources;
using System.Net;
using GamePlan.Models;

namespace GamePlan.Controllers
{
    public class EmailNotifyController : Controller
    {
        #region Index view method.  

        #region Get: /EmailNotify/Index method.  

        // <summary>  
        // Get: /EmailNotify/Index method.  
        // </summary>          
        // <returns>Return index view</returns>  
        public ActionResult Index()
        {
            try
            {
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }

            // Info.  
            return this.View();
        }

        #endregion

        #region POST: /EmailNotify/Index  

        // <summary>  
        // POST: /EmailNotify/Index  
        // </summary>  
        // <param name="model">Model parameter</param>  
        // <returns>Return - Response information</returns>  
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(EmailNotifyViewModel model)
        {
            try
            {
                // Verification  
                if (ModelState.IsValid)
                {
                    // Initialization.  
                    string emailMsg = "Dear " + model.ToEmail + ", <br /><br /> Thist is test <b style='color: red'> Notification </b> <br /><br /> Thanks & Regards, <br />Ryan Kunz";
                    string emailSubject = EmailInfo.EMAIL_SUBJECT_DEFAULT + " Test";

                    // Sending Email.  
                    await this.SendEmailAsync(model.ToEmail, emailMsg, emailSubject);


                    // Info.  
                    return this.Json(new { EnableSuccess = true, SuccessTitle = "Success", SuccessMsg = "Notification has been sent successfully! to '" + model.ToEmail + "' Check your email." });
                }
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);

                // Info  
                return this.Json(new { EnableError = true, ErrorTitle = "Error", ErrorMsg = ex.Message });
            }

            // Info  
            return this.Json(new { EnableError = true, ErrorTitle = "Error", ErrorMsg = "Something goes wrong, please try again later" });
        }

        #endregion

        #endregion

        #region Helper  

        #region Send Email method.  

        // <summary>  
        //  Send Email method.  
        // </summary>  
        // <param name="email">Email parameter</param>  
        // <param name="msg">Message parameter</param>  
        // <param name="subject">Subject parameter</param>  
        // <returns>Return await task</returns>  
        public async Task<bool> SendEmailAsync(string email, string msg, string subject = "")
        {
            // Initialization.  
            bool isSend = false;

            try
            {
                // Initialization.  
                var body = msg;
                var message = new MailMessage();

                // Settings.  
                message.To.Add(new MailAddress(email));
                message.From = new MailAddress(EmailInfo.FROM_EMAIL_ACCOUNT);
                message.Subject = !string.IsNullOrEmpty(subject) ? subject : EmailInfo.EMAIL_SUBJECT_DEFAULT;
                message.Body = body;
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    // Settings.  
                    var credential = new NetworkCredential
                    {
                        UserName = EmailInfo.FROM_EMAIL_ACCOUNT,
                        Password = EmailInfo.FROM_EMAIL_PASSWORD
                    };

                    // Settings.  
                    smtp.Credentials = credential;
                    smtp.Host = EmailInfo.SMTP_HOST_GMAIL;
                    smtp.Port = Convert.ToInt32(EmailInfo.SMTP_PORT_GMAIL);
                    smtp.EnableSsl = true;

                    // Sending  
                    await smtp.SendMailAsync(message);

                    // Settings.  
                    isSend = true;
                }
            }
            catch (Exception ex)
            {
                // Info  
                throw ex;
            }

            // info.  
            return isSend;
        }

        #endregion

        #endregion
    }
}