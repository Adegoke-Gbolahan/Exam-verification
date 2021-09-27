using SecuGen.FDxSDKPro.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ExamVerification
{
    public partial class frmfinger : Form
    {
        public int trap = 0;
        private SGFingerPrintManager m_FPM;
        public int cnt, nk;
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
        public frmfinger()
        {
            InitializeComponent();
        }
        void mYmessage(string ssm)
        {
            lstInfo.Items.Add(ssm);
            if (yem != 0)
            {
                Scheck();
            }
        }
        private void frmfinger_Load(object sender, EventArgs e)
        {
            m_LedOn = true;

            m_RegMin1 = new Byte[400];
            m_RegMin2 = new Byte[400];
            m_VrfMin = new Byte[400];
            _template = new Byte[400];

            m_FPM = new SGFingerPrintManager();
            Enumerate_Finger();
        }
        private void Identify_Finger_Me()
        {
            Int32 iError;
            bool matched1 = false;
            SGFPMSecurityLevel secu_level;
            int secuL = 3;
            try
            {
                if ((_template != null) && (_template.Length > 0))
                {
                    if (cnt <= 2)
                    {
                        secu_level = (SGFPMSecurityLevel)secuL;

                        iError = m_FPM.MatchTemplate(m_RegMin1, _template, secu_level, ref matched1);
                        if (iError == (Int32)SGFPMError.ERROR_NONE)
                        {
                            if (matched1)
                            {
                                yem = 1;
                            }
                            else
                            {
                                msg = "Finger does not match! Index 1";
                                yem = 0;
                                mYmessage(msg);
                                return;
                            }
                        }
                        else
                        {
                            mYmessage(global.DisplayError("MatchTemplate()", iError));
                        }
                    }
                    else
                    {
                        secu_level = (SGFPMSecurityLevel)secuL;
                        iError = m_FPM.MatchTemplate(m_RegMin2, _template, secu_level, ref matched1);
                        if (iError == (Int32)SGFPMError.ERROR_NONE)
                        {
                            if (matched1)
                            {
                                yem = 1;
                            }
                            else
                            {
                                msg = "Finger does not match! Index 2";
                                yem = 0;
                                mYmessage(msg);
                                return;
                            }
                        }
                        else
                        {
                            mYmessage(global.DisplayError("MatchTemplate()", iError));
                        }
                    }
                }
            }
            catch (Exception sam)
            {
                MessageBox.Show(sam.Message);
            }
        }

        private void Identify_Sec()
        {
            Int32 iError;
            bool matched1 = false;
            SGFPMSecurityLevel secu_level;
            int secuL = 3;
            try
            {
                if ((_template != null) && (_template.Length > 0))
                {
                    secu_level = (SGFPMSecurityLevel)secuL;
                    iError = m_FPM.MatchTemplate(m_RegMin1, m_RegMin2, secu_level, ref matched1);
                    if (iError == (Int32)SGFPMError.ERROR_NONE)
                    {
                        if (matched1)
                        {
                            msg = "Finger already used! Index 1\n Put the second finger";
                            yem = 0;
                            mYmessage(msg);
                            return;
                        }
                        else
                        {
                            yem = 1;
                        }
                    }
                    else
                    {
                        mYmessage(global.DisplayError("MatchTemplate()", iError));
                    }
                }
            }
            catch (Exception sam)
            {
                MessageBox.Show(sam.Message);
            }
        }

        public void Scheck()
        {
            try
            {
                if (cnt != trap)
                {
                    switch (cnt)
                    {
                        case 1:
                            lstInfo.Items.Add("Fingerprint Read Successfully>>");
                            lstInfo.Items.Add("Put that finger again!");
                            break;
                        case 2:
                            lstInfo.Items.Add("Fingerprint Read Successfully>>");
                            lstInfo.Items.Add("Put the second finger!");
                            break;
                        case 3:
                            lstInfo.Items.Add("Fingerprint Read Successfully>>");
                            lstInfo.Items.Add("Put that second finger again!");
                            break;
                        case 4:
                            lstInfo.Items.Add("Fingerprint Read Successfully>>");
                            lstInfo.Items.Add("All finger captured successfully!!!");
                            //Identify();
                            MessageBox.Show("All finger captured successfully!!!");
                            PrepareTemplate();
                            this.Close();
                            break;
                    }
                    trap = cnt;
                }
            }
            catch { }
        }

        public void Save_Template()
        {
            try
            {
                if (m_RegMin1 == null || m_RegMin2 == null)
                {
                    MessageBox.Show("Error, Fingerprint not yet captured!");
                    return;
                }
                this.Hide();
                connection._template_tmp = m_RegMin1;
                connection._template_tmp2 = m_RegMin2;
                this.Close();
            }
            catch (Exception em)
            {
                MessageBox.Show(em.ToString());
            }
        }

        private void PrepareTemplate()
        {
            Int32 iError;
            Int32 match_score = 0;
            SGFPMSecurityLevel secu_level; //
            int secuR = 4;

            secu_level = (SGFPMSecurityLevel)secuR;

            iError = m_FPM.GetMatchingScore(m_RegMin1, m_RegMin2, ref match_score);

            if (iError == (Int32)SGFPMError.ERROR_NONE)
            {
                Save_Template();
            }
            else
            {
                mYmessage(global.DisplayError("GetMatchingScore()", iError));
            }
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
                mYmessage(enum_device);
            }
        }

        private void cmdStart_Click(object sender, EventArgs e)
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
                    cmdStart.Enabled = false;
                    cmdStop.Enabled = true;
                    msg = "**Fingerprint Initialize Successfull**";
                    mYmessage(msg);
                    cnt = 0;
                    nk = 0;
                }
                else
                {
                    mYmessage(global.DisplayError("OpenDevice() ", iError));
                    cmdStart.Enabled = true;
                }
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

            if (cnt == 4)
            {
                mYmessage("Fingerprint completed!!!");
                yem = 0;
                return;
            }

            fp_image = new Byte[m_ImageWidth * m_ImageHeight];
            img_qlty = 0;

            iError = m_FPM.GetImage(fp_image);
            m_FPM.GetImageQuality(m_ImageWidth, m_ImageHeight, fp_image, ref img_qlty);
            if (cnt < 2)
            {
                DrawImage(fp_image, pictureBox1);
            }
            else
            {
                DrawImage(fp_image, pictureBox2);
            }

            if (iError == (Int32)SGFPMError.ERROR_NONE)
            {
                //check the typeof finger it is
                if (cnt == 0)
                {
                    iError = m_FPM.CreateTemplate(fp_image, m_RegMin1);
                }
                else if (cnt == 1)
                {
                    iError = m_FPM.CreateTemplate(fp_image, _template);
                    Identify_Finger_Me();
                }

                if (cnt == 2)
                {
                    iError = m_FPM.CreateTemplate(fp_image, m_RegMin2);
                    Identify_Sec();
                }
                else if (cnt == 3)
                {
                    iError = m_FPM.CreateTemplate(fp_image, _template);
                    Identify_Finger_Me();
                }
                if (yem == 0) { return; }

                if (iError == (Int32)SGFPMError.ERROR_NONE)
                {
                    string imgQ = "";
                    if (img_qlty < 50)
                    {
                        imgQ = "Low";
                        mYmessage("Finger Quality: " + imgQ);
                        return;
                    }
                    else if (img_qlty >= 50 && img_qlty <= 70)
                    {
                        imgQ = "Medium";
                        cnt++;
                    }
                    else if (img_qlty > 70)
                    {
                        imgQ = "High";
                        cnt++;
                    }
                    mYmessage("Finger Quality: " + imgQ);
                }
                else
                {
                    mYmessage(global.DisplayError("CreateTemplate()", iError));
                }
            }
            else
            {
                mYmessage(global.DisplayError("GetImage()", iError));
            }
        }

        private void cmdStop_Click(object sender, EventArgs e)
        {
            lstInfo.Items.Clear();
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            cmdStart.Enabled = true;
            cmdStop.Enabled = false;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
