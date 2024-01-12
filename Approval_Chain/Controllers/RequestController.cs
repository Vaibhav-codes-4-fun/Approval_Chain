using Approval_Chain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Reflection.Emit;

public class RequestController : Controller
{
    public object ViewState { get; private set; }

    public ActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public ActionResult ProcessRequest(RequestModel request)
    {
        // Process the request (save to database, etc.)

        // Send email to the chain of managers
        SendApprovalEmail(request);

        // You may redirect to a confirmation page or display a message
        return Content("Request submitted successfully!");
    }

    
     private void SendApprovalEmail(RequestModel request)
    {
    // SMTP configuration (replace with your email server details)
         var smtpClient = new SmtpClient("smtp-relay.brevo.com")
            {
               Port = 587,
               Credentials = new NetworkCredential("vaibhav.upasani24@gmail.com", "xsmtpsib-5bfec43a40a131f1875f2204942dced0de7305964c7d13745addea32de1c7a0f-np6HV8WGvX31Igwt"),
               EnableSsl = true,
            };

    // Split the input email addresses into an array
         var approverEmails = request.ApproverEmail.Split(',');

    // Loop through each email address and send an email
         
        // Email content
            var mailMessage = new MailMessage
             {
                From = new MailAddress(request.EmployeeEmail),
                Subject = "Employee Request Approval",
                IsBodyHtml = true,
             };
            var acceptLink = "http://yourwebsite.com/accept?requestID=123";
            var denyLink = "http://yourwebsite.com/deny?requestID=123";
            var body = $@"<p>Topic: {request.Topic}</p>
                  <p>Approver Email: {request.ApproverEmail}</p>
                  <p>Click the buttons below to respond:</p>
                  <a href=""{acceptLink}""><button style=""background-color: #4CAF50; color: white;"">Accept</button></a>
                  <a href=""{denyLink}""><button style=""background-color: #f44336; color: white;"">Deny</button></a>";
            mailMessage.Body = body;
        foreach (var approverEmail in approverEmails)
        {
            // Add the current manager's email address to the recipients
            mailMessage.To.Add(approverEmail.Trim());
         }
        // Send the email
        smtpClient.Send(mailMessage);
         
     }

}
