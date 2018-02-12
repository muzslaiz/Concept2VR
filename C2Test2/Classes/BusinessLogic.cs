using Concept2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Concept2.PMUSBInterface;

namespace C2Test2.Classes
{
    public class BusinessLogic : INotifyPropertyChanged
    {
        private double speed;
        public double Speed
        {
            get { return speed; }
            set
            {
                this.speed = value;
                this.OnPropertyChanged("speedString");
            }
        }
        public string speedString
        {
            get { return Speed.ToString() + " km/h"; }
        }


        private int speedLevel;
        public int SpeedLevel {
            get { return speedLevel; }
            set
            {
                speedLevel = value;
                this.OnPropertyChanged("speedLevelString");
            } 
        }
        public string speedLevelString
        {
            get { return speedLevel.ToString(); }
        }

        
        private double speedRatio;
        public double SpeedRatio
        {
            get { return speedRatio; }
            set
            {
                this.speedRatio = value;
                this.OnPropertyChanged("speedRatioString");
            }
        }
        public string speedRatioString
        {
            get { return speedRatio.ToString(); }
        }


        private uint distance;
        public uint Distance
        {
            get { return distance; }
            set
            {
                this.distance = value;
            }
        }

        public BusinessLogic()
        {
            speedLevel = 1;
            speedRatio = 0.3;
        }

        public void Connect()
        {
            PMUSBInterface.Initialize();
            var deviceNo = PMUSBInterface.DiscoverPMs(PMUSBInterface.PMtype.PM5_PRODUCT_NAME);
            string serialNo = PMUSBInterface.getSerialNumber(0);
            PMUSBInterface.InitializeProtocol(1000);
        }

        public void setSpeedRatio()
        {
            this.SpeedRatio = Math.Round(this.speed / Constants.droneVelocity,2) * 1.1;
        }

        public uint getNewDistance()
        {
            CSAFECommand command = new CSAFECommand(0);
            command.CommandsBytes.Add((uint)CSAFECommands.CSAFE_GETHORIZONTAL_CMD);
            var data = command.Execute();
            return PMUSBInterface.getMSB(data, 2);
        }

        public double getSpeedLevelMultiplier()
        {
            double ret = 0;
            switch (this.SpeedLevel)
            {
                case 1:
                    ret = 1.5;
                break;
                case 2:
                    ret = 2.3;
                break;
            }
            return ret;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
    }
}
