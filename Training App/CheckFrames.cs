using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace CameraCapture
{
    public partial class CheckFrames : Form
    {
        List<Bitmap> ExtFaces; //represents chosen extfaces from all extfaces captured

        //checkFrames Constructor Takes as input list of extfaces
        //copy all extraced faces from camera video capture into :Extfaces arraylist
        public CheckFrames(List<Bitmap> extFaces)
        {
            InitializeComponent();
            ExtFaces = extFaces;
            pictureBox1.Image = new Bitmap(ExtFaces[0]);
        }

        int count = 1, count2 = 1;
        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked && count < ExtFaces.Count)
            {
                pictureBox1.Image = new Bitmap(ExtFaces[count]);
                    
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                String extFacesPath = configFile.AppSettings.Settings["ExtFacesPath"].Value;

                string path = @"ids.txt";
                string id = "";
                string tempPath = "";
                using (System.IO.StreamReader sr = System.IO.File.OpenText(path))
                {                        
                    id = sr.ReadLine();
                    tempPath = extFacesPath + id + "\\";                    
                }
                Debug.WriteLine("path is " + tempPath);
                if (!Directory.Exists(tempPath))
                {
                    //MessageBox.Show("path is " + tempPath, "My Application", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
                    Directory.CreateDirectory(tempPath);
                }
                
                ExtFaces[count - 1].Save(tempPath + id + "_" + count2 + ".bmp");
                count++;
                count2++;
                checkBox1.Checked = false;
            }
            else if (!checkBox1.Checked && count < ExtFaces.Count)
            {
                pictureBox1.Image = new Bitmap(ExtFaces[count]);
                count++;
            }
            if (count == ExtFaces.Count - 1)
            {
                FinishAddStudent fad = new FinishAddStudent();
                fad.Tag = this;
                fad.Show(this);
                Hide();
            }
        }
    }
}
