using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace BEEWiN.Models
{
    public class Mail
    {
        public string VerifyCode(string email)
        {
            //=======================================================
            // Email 驗證信
            var rand = new Random();
            var verifycode = rand.Next(100000, 999999).ToString();

            // sender = 寄信者，reciver = 收信者
            MailMessage message = new MailMessage("jincoco88912@gmail.com", email);
            // 支援 HTML 語法
            message.IsBodyHtml = true;
            // E-Mail 編碼
            message.BodyEncoding = System.Text.Encoding.UTF8;
            // E-Mail 主旨 subject = 主旨
            message.Subject = "BEEWiN會員信箱驗證信";
            // E-Mail 內容 msg = 內容
            message.Body = verifycode;
            // 設定 SMTP Server 和 Port，此範例 Gmail SMTP Server = smtp.gmail.com，Port = 587
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            // 設定 SMTP 帳號及密碼 smtpAccount = SMTP帳號，smtpPassword = SMTP密碼
            smtpClient.Credentials = new System.Net.NetworkCredential("jincoco88912@gmail.com", "qxasfkeoznoigjmq");
            // E-Mail 寄出
            smtpClient.EnableSsl = true;
            smtpClient.Send(message);

            return (verifycode);
        }
        public void MailMessage(string userEmail, string msgSubject, string msgBody)
        {
            //=======================================================
            // Email 驗證信
            //=======================================================
            // sender = 寄信者，reciver = 收信者
            MailMessage message = new MailMessage("jincoco88912@gmail.com", userEmail);
            // 支援 HTML 語法
            message.IsBodyHtml = true;
            // E-Mail 編碼
            message.BodyEncoding = System.Text.Encoding.UTF8;
            // E-Mail 主旨 subject = 主旨
            message.Subject = msgSubject;
            // E-Mail 內容 msg = 內容
            message.Body = msgBody;
            // 設定 SMTP Server 和 Port，此範例 Gmail SMTP Server = smtp.gmail.com，Port = 587
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            // 設定 SMTP 帳號及密碼 smtpAccount = SMTP帳號，smtpPassword = SMTP密碼
            smtpClient.Credentials = new System.Net.NetworkCredential("jincoco88912@gmail.com", "qxasfkeoznoigjmq");
            // E-Mail 寄出
            smtpClient.EnableSsl = true;
            smtpClient.Send(message);
        }
    }
}