using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace XboxGrasshopper
{
    public class Xbox : GH_Component
    {
        public Xbox()
          : base("Xbox", "Xbox",
              "Listen to input from an Xbox game controller.",
              "XboxGrasshopper", "Xbox")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        public override Guid ComponentGuid => new Guid("3106204b-c72a-4d0f-b90a-4192999de5c3");
        protected override System.Drawing.Bitmap Icon => null;


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("in", "in", "", GH_ParamAccess.item);
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("out", "out", "", GH_ParamAccess.item);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string input = "";

            if (!DA.GetData(0, ref input)) return;

            DA.SetData(0, input);
        }



    }
}
