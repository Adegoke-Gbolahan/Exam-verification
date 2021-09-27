using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace ExamVerification
{
    class connection
    {
        public static MySqlConnection myConnection = new MySqlConnection("User=root;password=;server=localhost;database=examverification;");
        public static Byte[] _template_tmp;
        public static Byte[] _template_tmp2;
        public static void DBcon()
        {
            myConnection.Open();
        }
        public static void DBclose()
        {
            myConnection.Close();
        }
        public static string PadZero(int number)
        {
            if (number < 10)
            {
                return "0" + number;
            }
            return number.ToString();
        }

        public static byte[] convertIMGtoByte(Image img)
        {
            Bitmap btmp = new Bitmap(img);
            MemoryStream mstream = new MemoryStream();
            btmp.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] ImageAsBytes = mstream.ToArray();
            btmp.Dispose();
            return ImageAsBytes;
        }
    }
}
