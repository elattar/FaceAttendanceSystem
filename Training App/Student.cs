using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraCapture
{
    class Student
    {
        private string name;
        private string id;
        private string email;

        public void setName(string name)
        {
            this.name = name;
        }
        public string getName()
        {
            return this.name;
        }
        public void setID(string id)
        {
            this.id = id;
        }
        public string getID()
        {
            return this.id;
        }
        public void setEmail(string email)
        {
            this.email = email;
        }
        public string getEmail()
        {
            return this.email;
        }
    }
}
