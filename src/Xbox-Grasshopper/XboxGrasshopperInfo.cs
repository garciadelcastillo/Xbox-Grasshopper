using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace XboxGrasshopper
{
    public class XboxGrasshopper : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "XboxGrasshopper";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("19d283ff-00ea-4eba-8b96-5691f04b96fc");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
