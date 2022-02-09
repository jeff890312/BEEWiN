using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace BEEWiN.Models
{
    public class Notify
    {
        public void SendMessage(string sentmsg)
        {
            //=======================================================
            // LineNotify for youself
            // https.://qinoo.blogspot.com/2019/08/line-notify-c.html
            //=======================================================
            string[] adminLine = new string[] { "R8QKqjcLwWvAYL86x0strwW7K4JPsLs9bAyirm7sM3c" };
            foreach (var l in adminLine)
            {
                String line_notify_url = @"https://notify-api.line.me/api/notify";
                String msg = sentmsg;
                msg = @"message=" + HttpUtility.UrlEncode(msg);
                byte[] byteArray = Encoding.UTF8.GetBytes(msg);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(line_notify_url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("Authorization", "Bearer " + l);
                request.Timeout = 30000;

                // Get the request stream.  
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.  
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.  
                dataStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                int responseCode = (int)response.StatusCode;
                String responseMsg = (String)new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
        }
    }
}