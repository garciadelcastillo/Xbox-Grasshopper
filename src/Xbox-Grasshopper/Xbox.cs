using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Windows.Threading;

using Grasshopper.Kernel;
using Rhino.Geometry;

using J2i.Net.XInputWrapper;

namespace XboxGrasshopper
{
    public class Xbox : GH_Component
    {
        private XboxController _controller;

        private bool _padUp, _padDown, _padLeft, _padRight;
        private bool _buttonA, _buttonB, _buttonX, _buttonY;
        private bool _leftThumb;
        private double _leftThumbX, _leftThumbY;


        public Xbox()
          : base("Xbox", "Xbox",
              "Listen to input from an Xbox game controller.",
              "XboxGrasshopper", "Xbox")
        {
            _controller = XboxController.RetrieveController(0);
            _controller.StateChanged += _selectedController_StateChanged;
            XboxController.StartPolling();
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        public override Guid ComponentGuid => new Guid("3106204b-c72a-4d0f-b90a-4192999de5c3");
        protected override System.Drawing.Bitmap Icon => null;


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("ID", "ID", "Id number of the controller", GH_ParamAccess.item, 0);
            pManager.AddBooleanParameter("Autoupdate", "AUTO", "Keep listening while connection alive? The alternative is connecting a timer to this component.", GH_ParamAccess.item, true);
            pManager.AddIntegerParameter("Interval", "Int", "Refresh interval in milliseconds.", GH_ParamAccess.item, 33);
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("msg", "msg", "Status messages", GH_ParamAccess.item);

            pManager.AddBooleanParameter("PadLeft", "Left", "", GH_ParamAccess.item);
            pManager.AddBooleanParameter("PadRight", "Right", "", GH_ParamAccess.item);
            pManager.AddBooleanParameter("PadUp", "Up", "", GH_ParamAccess.item);
            pManager.AddBooleanParameter("PadDown", "Down", "", GH_ParamAccess.item);

            pManager.AddBooleanParameter("A", "A", "", GH_ParamAccess.item);
            pManager.AddBooleanParameter("B", "B", "", GH_ParamAccess.item);
            pManager.AddBooleanParameter("X", "X", "", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Y", "Y", "", GH_ParamAccess.item);

            pManager.AddBooleanParameter("LeftThumb", "LT", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("LeftThumbX", "LTX", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("LeftThumbY", "LTY", "", GH_ParamAccess.item);
            //pManager.AddNumberParameter("RightThumbX", "RTX", "", GH_ParamAccess.item);
            //pManager.AddNumberParameter("RightThumbY", "RTY", "", GH_ParamAccess.item);

            //pManager.AddBooleanParameter("LeftButton", "A", "", GH_ParamAccess.item);
            //pManager.AddNumberParameter("LeftThumbY", "LTY", "", GH_ParamAccess.item);
            
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int id = 0;
            bool auto = true;
            int interval = 33;

            if (!DA.GetData(0, ref id)) return;
            if (!DA.GetData(1, ref auto)) return;
            if (!DA.GetData(2, ref interval)) return;

            DA.SetData(0, "");

            DA.SetData(1, _padLeft);
            DA.SetData(2, _padRight);
            DA.SetData(3, _padUp);
            DA.SetData(4, _padDown);

            DA.SetData(5, _buttonA);
            DA.SetData(6, _buttonB);
            DA.SetData(7, _buttonX);
            DA.SetData(8, _buttonY);

            DA.SetData(9, _leftThumb);
            DA.SetData(10, _leftThumbX);
            DA.SetData(11, _leftThumbY);

            
            // Otherwise, back to reguar autoupdate
            if (auto)
            {
                this.OnPingDocument().ScheduleSolution(interval, doc =>
                {
                    this.ExpireSolution(false);
                });
            }
        }


        void _selectedController_StateChanged(object sender, XboxControllerStateChangedEventArgs e)
        {
            OnPropertyChanged("SelectedController");

            //Console.WriteLine($"{it++} prop changed {sender} {e.PreviousInputState.Gamepad} {e.CurrentInputState.Gamepad}");
        }


        public void OnPropertyChanged(string name)
        {
            //if (PropertyChanged != null)
            //{
            //    System.Action a = () => { PropertyChanged(this, new PropertyChangedEventArgs(name)); };
            //    Dispatcher.BeginInvoke(a, null);
            //    //Console.WriteLine($"Changed name: {name} {SelectedController.RightTrigger}");
            //}

            _padUp = _controller.IsDPadUpPressed;
            _padDown = _controller.IsDPadDownPressed;
            _padLeft = _controller.IsDPadLeftPressed;
            _padRight = _controller.IsDPadRightPressed;

            _buttonA = _controller.IsAPressed;
            _buttonB = _controller.IsBPressed;
            _buttonX = _controller.IsXPressed;
            _buttonY = _controller.IsYPressed;

            _leftThumb = _controller.IsLeftStickPressed;
            _leftThumbX = _controller.LeftThumbStick.X;
            _leftThumbY = _controller.LeftThumbStick.Y;

        }

        //public event PropertyChangedEventHandler PropertyChanged;
    }
}
