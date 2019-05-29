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
using Microsoft.AspNet.Identity;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;

namespace GamePlan.Controllers
{
    public class EmailNotifyController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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
            EmailNotifyViewModel model2 = new EmailNotifyViewModel { ToEmail = User.Identity.GetUserName() };
            

            try
            {
                // Verification  
                if (ModelState.IsValid)
                {
                    var allEvents = await GetUserEvents();
                    var eventsWithReminders = allEvents.Where(e => e.EmailNotification == true).ToList();

                    // Initialization.  
                    string emailMsg = "Dear " + model2.ToEmail + ", <br /><br /> Here is your daily schedule<br><br />";


                    string emailSubject = EmailInfo.EMAIL_SUBJECT_DEFAULT + " Reminder";

                    foreach (var item in eventsWithReminders)
                    {
                        emailMsg += item.Description.ToString();
                        emailMsg += "<br><br />";
                    }

                    // Sending Email.  
                    await this.SendEmailAsync(model2.ToEmail, emailMsg, emailSubject);


                    // Info.  
                    return this.Json(new { EnableSuccess = true, SuccessTitle = "Success", SuccessMsg = "Notification has been sent successfully! to '" + model2.ToEmail + "' Check your email." });
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

        public async Task<List<Event>> GetUserEvents()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49757/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/Events");
                response.EnsureSuccessStatusCode();
                string data = await response.Content.ReadAsStringAsync();
                var jsonResults = JsonConvert.DeserializeObject<IEnumerable<Event>>(data).ToList();

                return jsonResults;
            }
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