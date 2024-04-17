using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;

namespace EmailSender;

public static class Function
{
    [FunctionName("SendEmail")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");



        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        string name = data?.name;
        string email = data?.email;
        string comment = data?.comment;

        string responseMessage = $"Hello, {name}. your comment : '{comment}' successfully received";

        var client = new SmtpClient("smtp.gmail.com", 587);
        client.EnableSsl = true;

        client.Credentials = new NetworkCredential(
            "ami65143@gmail.com",
            "api_key");

        var message = new MailMessage()
        {
            Subject = "no-reply",
            Body = responseMessage
        };

        message.From = new MailAddress("ami65143@gmail.com", "no-reply");
        message.To.Add(new MailAddress(email));

        client.Send(message);


        return new OkResult();
    }
}
