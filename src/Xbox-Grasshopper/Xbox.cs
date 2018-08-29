using System;
using System.Collections.Generic;
using System.ComponentModel;

using Grasshopper.Kernel;

using J2i.Net.XInputWrapper;

namespace XboxGrasshopper
{
    public class Xbox : GH_Component
    {
        private string _message;

        private XboxController _controller;
        private int _prevId = -1, _currid = -1;

        private bool _padUp, _padDown, _padLeft, _padRight;
        private bool _buttonA, _buttonB, _buttonX, _buttonY;
        private bool _leftThumb, _rightThumb;
        private double _leftThumbX, _leftThumbY, _rightThumbX, _rightThumbY;
        private bool _leftShoulder, _rightShoulder;
        private double _leftTrigger, _rightTrigger;
        private bool _start, _select;

        public Xbox()
          : base("Xbox", "Xbox",
              "Listen to input from an Xbox game controller.",
              "XboxGrasshopper", "Xbox")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        public override Guid ComponentGuid => new Guid("3106204b-c72a-4d0f-b90a-4192999de5c3");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.xbox_controller_24;


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

            pManager.AddBooleanParameter("LeftThumbPressed", "LTP", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("LeftThumbX", "LTX", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("LeftThumbY", "LTY", "", GH_ParamAccess.item);

            pManager.AddBooleanParameter("RightThumbPressed", "RTP", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("RightThumbX", "RTX", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("RightThumbY", "RTY", "", GH_ParamAccess.item);

            pManager.AddBooleanParameter("LeftButton", "LB", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("LeftTrigger", "LT", "", GH_ParamAccess.item);

            pManager.AddBooleanParameter("RightButton", "RB", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("RightTrigger", "RT", "", GH_ParamAccess.item);

            pManager.AddBooleanParameter("Start", "St", "", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Select", "Sl", "", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int id = 0;
            bool autoUpdate = true;
            int millis = 33;

            if (!DA.GetData(0, ref id)) return;
            if (!DA.GetData(1, ref autoUpdate)) return;
            if (!DA.GetData(2, ref millis)) return;

            _currid = id;
            if (_currid != _prevId)
            {
                InitializeController();
            }

            // Some sanity
            if (millis < 10) millis = 10;

            DA.SetData(0, _message);
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

            DA.SetData(12, _rightThumb);
            DA.SetData(13, _rightThumbX);
            DA.SetData(14, _rightThumbY);

            DA.SetData(15, _leftShoulder);
            DA.SetData(16, _leftTrigger);

            DA.SetData(17, _rightShoulder);
            DA.SetData(18, _rightTrigger);

            DA.SetData(19, _start);
            DA.SetData(20, _select);

            if (autoUpdate)
            {
                this.OnPingDocument().ScheduleSolution(millis, doc =>
                {
                    this.ExpireSolution(false);
                });
            }
        }

        internal void InitializeController()
        {
            XboxController.StopPolling();

            try
            {
                _controller = XboxController.RetrieveController(_currid);
                _controller.StateChanged += _selectedController_StateChanged;
                XboxController.StartPolling();

                _message = "Connected to controller #" + _currid;
            }
            catch
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Cannot find controller with ID #" + _currid);
                _controller = null;

                _message = "Cannot find controller with ID #" + _currid;
            }
        }
        
        void _selectedController_StateChanged(object sender, XboxControllerStateChangedEventArgs e) => OnPropertyChanged("SelectedController");
        
        public void OnPropertyChanged(string name)
        {
            _padLeft = _controller.IsDPadLeftPressed;
            _padRight = _controller.IsDPadRightPressed;
            _padUp = _controller.IsDPadUpPressed;
            _padDown = _controller.IsDPadDownPressed;

            _buttonA = _controller.IsAPressed;
            _buttonB = _controller.IsBPressed;
            _buttonX = _controller.IsXPressed;
            _buttonY = _controller.IsYPressed;

            _leftThumb = _controller.IsLeftStickPressed;
            _leftThumbX = _controller.LeftThumbStick.X;
            _leftThumbY = _controller.LeftThumbStick.Y;

            _rightThumb = _controller.IsRightStickPressed;
            _rightThumbX = _controller.RightThumbStick.X;
            _rightThumbY = _controller.RightThumbStick.Y;

            _leftShoulder = _controller.IsLeftShoulderPressed;
            _leftTrigger = _controller.LeftTrigger;

            _rightShoulder = _controller.IsRightShoulderPressed;
            _rightTrigger = _controller.RightTrigger;

            _start = _controller.IsStartPressed;
            _select = _controller.IsBackPressed;
        }
    }
}
