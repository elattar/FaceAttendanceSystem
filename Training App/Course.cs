using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace CameraCapture
{
    class Course
    {
        private string courseName;
        private string courseCode;

        public void setCourseName(string courseName)
        {
            this.courseName = courseName;
        }
        public string getCourseName()
        {
            return this.courseName;
        }
        public void setcourseCode(string courseCode)
        {
            this.courseCode = courseCode;
        }
        public string getcourseCode()
        {
            return this.courseCode;
        }
        
        public string[] getCourses()
        {
            /****/
            string[] lines = File.ReadAllLines(@"courses.txt");
            /*
            //hna baftrd en course fl path dh :
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            String coursesPath = configFile.AppSettings.Settings["coursesPath"].Value;
            */
            /****/
            return lines;
        }
         
    }
}
