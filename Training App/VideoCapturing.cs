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

namespace CameraCapture
{
    public partial class VideoCapturing : Form
    {
        private Capture capture;        // takes images from camera as image frames
        private bool captureInProgress; // checks if capturing is executing
        private Timer timer1;           // timer to take picture every 2 secs

        List<Bitmap> ExtFaces;          //represents the faces extracted during cameraCapture
        double countTimer;
        CascadeClassifier cascade;      //the face detector
        Rectangle[] faces;              // the rectangles of the detected photos
        int FaceNo = 0;

        //what this function does ?
        // open camera, take a capture 
        // when an image is grabbed (captured) add it to the capture object
        // then, process frame :
        // create a matrix object to save the picture
        // create Matrix to save the pictures in
        // put what camera captured into the matrix object : Mat  
        // convert frame obj to an image obj
        // if frames object contain images : 
        // loop at all images . detect faces and draw blue rectangles on detected faces 
        // Add the all extracted faces to : extfaces (arraylist) && count no of faces (INT counter)
        // DisplayThe recording by the camera in the imagebox
        
        public VideoCapturing()
        {
            InitializeComponent();
            cascade = new CascadeClassifier("haarcascade_frontalface_default.xml"); //this file contains the training 
            CvInvoke.UseOpenCL = false;
            try
            {
                capture = new Capture();
                ExtFaces = new List<Bitmap>();
                capture.ImageGrabbed += ProcessFrame;
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
        }

        private void ProcessFrame(object sender, EventArgs arg)
        {
            Mat frame = new Mat();      //Matrix to save the picture
            capture.Retrieve(frame, 0); //retrieve the picture to the matrinx
            Image<Bgr, byte> image = frame.ToImage<Bgr, byte>();
            FaceNo = 0;
            if (frame != null)
            {
                Image<Gray, byte> grayFrame = frame.ToImage<Gray, byte>(); // display the image in the imageBox
                faces = cascade.DetectMultiScale(grayFrame, 1.1, 2, new Size(30, 30));

                Bitmap BitmapInput = grayFrame.ToBitmap();
                Bitmap ExtractedFace;
                Graphics FaceCanvas;
                //countTable.Text = faces.Count().ToString();
                if (faces.Count() > 0)
                {
                    foreach (var face in faces)
                    {
                        image.Draw(face, new Bgr(Color.Blue), 1); // draw rectangles in the picture
                        ExtractedFace = new Bitmap(face.Width, face.Height);
                        FaceCanvas = Graphics.FromImage(ExtractedFace);
                        FaceCanvas.DrawImage(BitmapInput, 0, 0, face, GraphicsUnit.Pixel);
                        ExtFaces.Add(ExtractedFace);
                        FaceNo++;
                    }
                }
                imageBox1.Image = image; // display the image in the imageBox
            }
        }


        // stop button
        private void button1_Click(object sender, EventArgs e)
        {
            //haar = new HaarCascade("haarcascade_frontalface_default.xml");
            if (capture != null && button1.Text == "Start")
            {
                button1.Text = "Started";
                InitTimer();
            }
        }

        // timer time take a value here
        // start timer
        public void InitTimer()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000; // in miliseconds
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            countTimer++;
            if (countTimer >= 10)
            {
                timer1.Stop();
                MessageBox.Show(ExtFaces.Count + "");
            }
            else
                ProcessFrame(sender, e);
        }

        // Button : next
        // if clicked :
        // call CheckFrames constructor and send it the ExtFaces
        private void button2_Click(object sender, EventArgs e)
        {
            if (ExtFaces.Count == 0)
                MessageBox.Show("Please start video capturing", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                CheckFrames form = new CheckFrames(ExtFaces);
                form.Tag = this;
                form.Show(this);
                Hide();
            }
        }
    }
}