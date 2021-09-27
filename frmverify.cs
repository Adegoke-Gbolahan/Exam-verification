using MySql.Data.MySqlClient;
using SecuGen.FDxSDKPro.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExamVerification
{
    public partial class frmverify : Form
    {
        private SGFingerPrintManager m_FPM;
        public int cnt, nk;
        public int trap = 0;
        public int yem = 1;
        public string msg = "";
        private bool m_LedOn = false;
        private Int32 m_ImageWidth;
        private Int32 m_ImageHeight;
        private Byte[] m_RegMin1;
        private Byte[] m_RegMin2;
        private Byte[] m_VrfMin;
        private Byte[] _template;
        private SGFPMDeviceList[] m_DevList; // Used for EnumerateDevice
        public frmverify()
        {
            InitializeComponent();
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void frmverify_Load(object sender, EventArgs e)
        {
            m_LedOn = true;

            m_RegMin1 = new Byte[400];
            m_RegMin2 = new Byte[400];
            m_VrfMin = new Byte[400];
            _template = new Byte[400];

            m_FPM = new SGFingerPrintManager();
            Enumerate_Finger();
            InitailizeFinger();
        }
        private void Enumerate_Finger()
        {
            Int32 iError;
            string enum_device = "";

            // Enumerate Device
            iError = m_FPM.EnumerateDevice();

            // Get enumeration info into SGFPMDeviceList
            m_DevList = new SGFPMDeviceList[m_FPM.NumberOfDevice];

            for (int i = 0; i < m_FPM.NumberOfDevice; i++)
            {
                m_DevList[i] = new SGFPMDeviceList();
                m_FPM.GetEnumDeviceInfo(i, m_DevList[i]);
                enum_device = m_DevList[i].DevName.ToString() + " : " + m_DevList[i].DevID;
                //mYmessage(enum_device);
            }

            if (enum_device == "")
            {
                // Add Auto Selection
                enum_device = "Auto Selection";
                //mYmessage(enum_device);
            }
        }

        private void Auto_Detect_Finger()
        {
            m_FPM.EnableAutoOnEvent(true, (int)this.Handle);
        }

        private void GetMyDeviceInfo()
        {
            SGFPMDeviceInfoParam pInfo = new SGFPMDeviceInfoParam();
            Int32 iError = m_FPM.GetDeviceInfo(pInfo);

            if (iError == (Int32)SGFPMError.ERROR_NONE)
            {
                m_ImageWidth = pInfo.ImageWidth;
                m_ImageHeight = pInfo.ImageHeight;

                ASCIIEncoding encoding = new ASCIIEncoding();
            }
        }

        private void DrawImage(Byte[] imgData, PictureBox picBox)
        {
            int colorval;
            Bitmap bmp = new Bitmap(m_ImageWidth, m_ImageHeight);
            picBox.Image = (Image)bmp;

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    colorval = (int)imgData[(j * m_ImageWidth) + i];
                    bmp.SetPixel(i, j, Color.FromArgb(colorval, colorval, colorval));
                }
            }
            picBox.Refresh();
        }

        protected override void WndProc(ref Message message)
        {
            if (message.Msg == (int)SGFPMMessages.DEV_AUTOONEVENT)
            {
                if (message.WParam.ToInt32() == (Int32)SGFPMAutoOnEvent.FINGER_ON)
                {
                    //mYmessage("Finger is on scanner");
                    CaptureFingerTemplate();
                }
                else if (message.WParam.ToInt32() == (Int32)SGFPMAutoOnEvent.FINGER_OFF)
                {
                    //mYmessage("Finger removed from scanner");
                }
            }
            base.WndProc(ref message);
        }

        private void CaptureFingerTemplate()
        {
            Int32 iError;
            Byte[] fp_image;
            Int32 img_qlty;

            fp_image = new Byte[m_ImageWidth * m_ImageHeight];
            img_qlty = 0;

            iError = m_FPM.GetImage(fp_image);
            m_FPM.GetImageQuality(m_ImageWidth, m_ImageHeight, fp_image, ref img_qlty);

            DrawImage(fp_image, picFinger);

            if (iError == (Int32)SGFPMError.ERROR_NONE)
            {
                iError = m_FPM.CreateTemplate(fp_image, m_VrfMin);

                if (iError == (Int32)SGFPMError.ERROR_NONE)
                {
                    string imgQ;
                        imgQ = "";
                    if (img_qlty < 50)
                    {
                        imgQ = "Low";
                        return;
                    }
                    else if (img_qlty >= 50 && img_qlty <= 70)
                    {
                        imgQ = "Medium";
                    }
                    else if (img_qlty > 70)
                    {
                        imgQ = "High";
                    }
                    Retrieve();
                }
                else
                {
                    //mYmessage(global.DisplayError("CreateTemplate()", iError));
                }
            }
            else
            {
                //mYmessage(global.DisplayError("GetImage()", iError));
            }
        }
        private void Retrieve()
        {
            Int32 iError;
            bool matched1 = false;
            bool matched2 = false;
            SGFPMSecurityLevel secu_level;
            int secuL = 3;
            try
            {
                if ((m_VrfMin != null) && (m_VrfMin.Length > 0))
                {
                    frmfinish hab = new frmfinish();
                    connection.DBcon();
                    MySqlDataReader myReader;
                    string sql = "SELECT * FROM student_data";
                    MySqlCommand cmd = new MySqlCommand(sql, connection.myConnection);
                    myReader = cmd.ExecuteReader();
                    while (myReader.Read())
                    {
                        m_RegMin1 = (Byte[])myReader["finger1"];
                        m_RegMin2 = (Byte[])myReader["finger2"];
                        hab.lblname.Text = myReader["name"].ToString();
                        hab.lblmatric.Text = myReader["matric_no"].ToString();
                        hab.lblphone.Text = myReader["phone_no"].ToString();
                        hab.lblgender.Text = myReader["gender"].ToString();
                        hab.lblstate.Text = myReader["state"].ToString();
                        hab.lbldepartment.Text = myReader["department"].ToString();
                        hab.lbllevel.Text = myReader["level"].ToString();
                        byte[] px = (byte[])(myReader["image"]);
                        MemoryStream ms = new MemoryStream(px);
                        hab.imgpic.Image = System.Drawing.Image.FromStream(ms);
                        ms.SetLength(0);
                        secu_level = (SGFPMSecurityLevel)secuL;
                        iError = m_FPM.MatchTemplate(m_RegMin1, m_VrfMin, secu_level, ref matched1);
                        iError = m_FPM.MatchTemplate(m_RegMin2, m_VrfMin, secu_level, ref matched2);
                        if (iError == (Int32)SGFPMError.ERROR_NONE)
                        {
                            if (matched1 || matched2)
                            {
                                connection.DBclose();
                                myReader.Close();
                                this.Hide();
                                //hab.timer1.Enabled = true;
                                hab.ShowDialog();
                                this.Close();
                                return;
                            }
                        }
                    }
                    lblmsg.Text = "Invalid Student!";
                    lblmsg.Visible = true;
                    connection.DBclose();
                    myReader.Close();
                }
            }
            catch (Exception samm)
            {
                connection.myConnection.Close();
                MessageBox.Show(samm.Message);
            }
        }

        public void InitailizeFinger()
        {
            if (m_FPM.NumberOfDevice == 0)
            {
                return;
            }
            Int32 iError;
            SGFPMDeviceName device_name;
            Int32 device_id;

            Int32 numberOfDevices = 2;
            Int32 deviceSelected = 0;
            Boolean autoSelection = (deviceSelected == (numberOfDevices - 1));  // Last index

            if (autoSelection)
            {
                // Order of search: Hamster IV(HFDU04) -> Plus(HFDU03) -> III (HFDU02)
                device_name = SGFPMDeviceName.DEV_AUTO;
                device_id = (Int32)(SGFPMPortAddr.USB_AUTO_DETECT);
            }
            else
            {
                device_name = m_DevList[deviceSelected].DevName;
                device_id = m_DevList[deviceSelected].DevID;
            }

            iError = m_FPM.Init(device_name);
            iError = m_FPM.OpenDevice(device_id);

            if (iError == (Int32)SGFPMError.ERROR_NONE)
            {
                GetMyDeviceInfo();
                // FDU03, FDU04 only
                if ((device_name == SGFPMDeviceName.DEV_FDU03) || (device_name == SGFPMDeviceName.DEV_FDU04))
                {
                    Auto_Detect_Finger();
                }
                else
                {
                    lblmsg.Text = global.DisplayError("OpenDevice() ", iError);
                    lblmsg.Visible = true;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblplace.Visible = !lblplace.Visible;
        }
    }
}
