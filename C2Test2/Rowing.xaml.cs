using C2Test2.Classes;
using Concept2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static Concept2.PMUSBInterface;

namespace C2Test2
{
    /// <summary>
    /// Interaction logic for Rowing.xaml
    /// </summary>
    ///
    
    public partial class Rowing : Window
    {
        BusinessLogic logic;
        bool isPlaying;
        int stop;

        public Rowing(ref BusinessLogic logic_)
        {
            isPlaying = false;
            InitializeComponent();
            this.logic = logic_;
            this.DataContext = logic;
            this.VideoControl.Play();
            this.VideoControl.Pause();
            this.VideoControl.Volume = 0;

            logic.Connect();
            logic.Distance = logic.getNewDistance();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(3000);
            timer.Tick += timer_Tick;
            timer.Start();
            //timer_Tick(sessionStart, new EventArgs());
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                uint newDistance = logic.getNewDistance();
                double diff = 0;

                if (newDistance > logic.Distance)
                {
                    stop = 0;
                    diff = (double)(newDistance - logic.Distance);
                    logic.Speed = (diff / 3) * 3.6 * logic.getSpeedLevelMultiplier();
                    logic.setSpeedRatio();
                    this.VideoControl.SpeedRatio = logic.SpeedRatio;
                    if (!isPlaying)
                    {
                        this.VideoControl.Play();
                        isPlaying = true;
                    }
                }
                else
                {
                    if (stop > 0 && stop <= 2)
                    {
                        logic.Speed = Math.Round(logic.Speed / 2, 1) * logic.getSpeedLevelMultiplier();
                    }
                    else if (stop == 3)
                    {
                        this.VideoControl.Pause();
                        logic.Speed = 0;
                        logic.SpeedRatio = 0;
                        isPlaying = false;
                    }
                    else if (stop == 4)
                    {
                        this.VideoControl.Stop();
                        this.VideoControl.Play();
                        this.VideoControl.Pause();
                        isPlaying = false;
                    }
                    stop++;
                }
                logic.Distance = newDistance;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt! Hibaüzenet: " + ex.Message);
                logic.Speed = 0;
                logic.SpeedRatio = 0;
                this.Close();
            }
        }

        private void btn_slowdown_Click(object sender, RoutedEventArgs e)
        {
            if (this.VideoControl.SpeedRatio - 0.2 >= 0)
            {
                double speedR = this.VideoControl.SpeedRatio;
                speedR -= 0.2;
                speedR = Math.Round(speedR, 1);
                this.VideoControl.SpeedRatio = speedR;
            }
        }

        private void btn_speedup_Click(object sender, RoutedEventArgs e)
        {
            double speedR = this.VideoControl.SpeedRatio;
            speedR += 0.2;
            speedR = Math.Round(speedR, 1);
            this.VideoControl.SpeedRatio = speedR;
            this.lbl_speedrate.Content = this.VideoControl.SpeedRatio;
        }
        
        
    }
}
