﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Http;
using System.Net.Mail;


namespace AnotherJiraRestClient
{
   public class sendGrid
    {
        public static async Task Execute(string From,string mailTo,string Msg,string html)
        {
            try
            {
                
                var client = new SendGridClient("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");
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(From,"SLA Alert!"),
                    Subject = "SLA Has Been Breached...",
                    PlainTextContent = Msg,
                    HtmlContent = html,
                };
                msg.AddTo(new EmailAddress(mailTo,"santhosh kumar"));
                msg.AddCc(new EmailAddress("deepaksingh.mohanta-ext@jci.com", "Deepak"));
                var response = await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {

            }
        }
    }
}