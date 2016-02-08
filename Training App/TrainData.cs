using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.Util;
using System.IO;
using Emgu.CV.CvEnum;
using System.Configuration;

namespace CameraCapture
{
    public partial class TrainData : Form
    {
        private CascadeClassifier cascade;         //the face detector
        private FaceRecognizer fr1 ;
        private FaceRecognizer fr2 ;
        private FaceRecognizer fr3 ;

        public TrainData()
        {
            InitializeComponent();
            fr1 = new EigenFaceRecognizer(80, double.PositiveInfinity);//The recognitoion object
            fr2 = new FisherFaceRecognizer(-1, 3100);//The recognitoion object
            fr3 = new LBPHFaceRecognizer(1, 8, 8, 8, 100);//50
            cascade = new CascadeClassifier("haarcascade_frontalface_default.xml"); //this file contains the training 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Train();
        }

        public void Train()
        {
            string path = @"courses.txt";
            string path2 = @"enrollement.txt";
            
            // loop on courses file getting each course and loop on each course getting enrolled students in
            string[] linesCoursesFile = System.IO.File.ReadAllLines(path);

            foreach (string courseLine in linesCoursesFile)
            {
                string[] courseSplitter = courseLine.Split(':');
                string courseCode = courseSplitter[0].Trim();

                List<Image<Gray, byte>> images = new List<Image<Gray, byte>>();
                List<int> ids = new List<int>();
                List<int> idsTrainned = new List<int>();

                string[] linesEnrollementFile = System.IO.File.ReadAllLines(path2);
                
                foreach (string line in linesEnrollementFile)
                {   
                    string[] splitter = line.Split(',');
                    
                    if (splitter[1].Trim().Equals(courseCode))
                    {
                        ids.Add(Int32.Parse(splitter[0].Trim()));
                    }
                }
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                
                for(int i=0 ; i<ids.Count ; i++)
                {
                    String extFacesPath = configFile.AppSettings.Settings["ExtFacesPath"].Value + "\\" + ids[i];
                    DirectoryInfo dInfo = new DirectoryInfo(extFacesPath);

                    var allImages = dInfo.GetFiles("*.bmp"); //get from this directory all files contain ".bmp"
                    foreach (var image in allImages)
                    {
                        string photoPath = extFacesPath + "\\" + image;
                        Image<Gray, byte> img = new Image<Gray, byte>(photoPath).Resize(200, 200, Inter.Cubic);
                        img._EqualizeHist();
                        img.Save(photoPath);
                        images.Add(img);
                        idsTrainned.Add( ids[i] );
                    }
                }
                
                string h1Path = configFile.AppSettings.Settings["h1FilePath"].Value + "\\" + "h1_" + courseCode;
                string h2Path = configFile.AppSettings.Settings["h2FilePath"].Value + "\\" + "h2_" + courseCode;
                string h3Path = configFile.AppSettings.Settings["h3FilePath"].Value + "\\" + "h3_" + courseCode;
                
                if(images.Count > 0)
                {
                    fr1.Train(images.ToArray(), idsTrainned.ToArray());//this line is self explanatory
                    fr1.Save(h1Path);//saving the trainig 
                    //fr2.Train(images.ToArray(), idsTrainned.ToArray());//this line is self explanatory
                    //fr2.Save(h2Path);//saving the trainig 
                    fr3.Train(images.ToArray(), idsTrainned.ToArray());
                    fr3.Save(h3Path);//saving the trainig
                }
            }

            FinishAddStudent fas = new FinishAddStudent();
            fas.Tag = this;
            fas.Show(this);
            Hide();
        }
    }
}