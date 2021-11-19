using LetsJump.DataAccess.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.ComponentModel.DataAnnotations;
using LetsJump.DataAccess.Data;

namespace LetsJump.DataAccess.Tools
{
    public class Messaging : DataCredential
    {
        public MessageType messageType { get; set; } = MessageType.Verification;
        public string EmailOrPhone { get; set; }
        public string UniqueString { get; set; }

        public void SendMessage(UserLogin user)
        {
            #region Configuring phone and message body
            var MessageBody = "";
            switch (messageType)
            {
                case MessageType.Verification:
                    MessageBody = "Thank you for registering with " + CompanyName + ". Please verify your account by clicking on this link:";
                    break;
                case MessageType.SignUp_Success:
                    MessageBody = "Thank you for registering with " + CompanyName + ". " + CompanyMotto;
                    break;
                case MessageType.Password_RequestReset:
                    MessageBody = "We received your request to reset your password. Please use this code to reset your password: " + UniqueString + ". Best wishes, Team " + CompanyName;
                    break;
            }

            bool IsEmail = new EmailAddressAttribute().IsValid(user.Email);
            if (!IsEmail)
                EmailOrPhone = EmailOrPhone.Length! < 11 && EmailOrPhone.Length == 11 ? "+88" + EmailOrPhone : EmailOrPhone;
            #endregion

            (IsEmail ? new Action<string, string>(SendEmail) : SendSMS)(MessageBody, EmailOrPhone);
        }

        private void SendEmail(string MessageBody, string Email)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(CompanyName, SMTP_Username));
                message.To.Add(new MailboxAddress("", Email));
                message.Subject = "How you doin'?";

                message.Body = new TextPart("plain")
                {
                    Text = MessageBody
                };

                using (var client = new SmtpClient())
                {
                    client.Connect(SMTP_Host, SMTP_Port, false);
                    client.Authenticate(SMTP_Username, SMTP_Password);

                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex) { }
        }

        private void SendSMS(string MessageBody, string Phone)
        {
            var client = new RestClient("https://api.sms.to/sms/");
            var request = new RestRequest("send");
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new { message = MessageBody, to = Phone, sender_id = CompanyName, callback_url = "https://sms.to/callback/handler" });
            request.AddHeader("content-type", "application/json");
            request.AddHeader("Authorization", "Bearer " + SMS_to_API_Key);
            var response = client.Post(request);
            var content = response.Content;
        }

        public enum MessageType
        {
            Verification,
            SignUp_Success,
            Password_RequestReset
        }
    }
}
