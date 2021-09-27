using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ExamVerification
{
    class sms
    {
        public static string JsonPost(string msg)
        {
            string username = "harbeeb11";
            string pass = "harbeeb11";

            string url = "https://account.kudisms.net/api/?username="+username+"&password="+pass+"&message="+msg+"&sender=Fedpoly&mobiles=09019473995,09033536548";
            try
            {
                string result = "";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();

                }

                return result;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
