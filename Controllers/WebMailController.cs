using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using APIMail.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIMail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebMailController : ControllerBase
    {

        [HttpPost("SendMail")]
        public JsonResponse<string> Contact(EmailFormModel model)
        {
            JsonResponse<string> jsonResponse = new JsonResponse<string>();
            try
            {
                if (ModelState.IsValid)
                {
                    var fromAddress = new MailAddress("info@advancetranscription.com",model.FromName);
                    var toAddress = new MailAddress("info@advancetranscription.com");
                    const string fromPassword = "Advance@#$2020";
                    string subject = model.Subject;
                    string body = model.Message;

                    var smtp = new SmtpClient
                    {
                        Host = "mail.advancetranscription.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                        Timeout = 20000
                    };
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(message);
                        jsonResponse.data = "Mail Sent Successully";
                        jsonResponse.status = new ServiceStatus(200);
                    }
                }
            }
            catch (Exception ex)
            {
                jsonResponse.status = new ServiceStatus(500, ex.Message);
            }
            return jsonResponse;
        }

    }
}
