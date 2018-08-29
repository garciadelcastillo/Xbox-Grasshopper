using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper.Kernel;

namespace XboxGrasshopper.Utils
{
    // https://www.grasshopper3d.com/forum/topics/add-an-icon-image-to-grasshopper-toolbar-tabs
    public class TabSetup : GH_AssemblyPriority
    {
        public override GH_LoadingInstruction PriorityLoad()
        {
            //throw new NotImplementedException();
            var server = Grasshopper.Instances.ComponentServer;

            server.AddCategoryShortName("XboxGrasshopper", "Xbox");
            server.AddCategorySymbolName("XboxGrasshopper", 'X');
            server.AddCategoryIcon("XboxGrasshopper", Properties.Resources.xbox_controller_16);

            return GH_LoadingInstruction.Proceed;
        }

    }
}
