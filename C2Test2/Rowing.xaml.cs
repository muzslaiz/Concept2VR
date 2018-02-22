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
        bool finished;
        int stop;
        int finishedCounter;
        DispatcherTimer timer;

        public Rowing(ref BusinessLogic logic_)
        {
            isPlaying = false;
            finished = false;
            stop = 0;
            finishedCounter = 0;

            InitializeComponent();
            this.logic = logic_;
            this.DataContext = logic;

            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Topmost = true;

            this.VideoControl.Play();
            this.VideoControl.Pause();
            this.VideoControl.Volume = 0;
            this.VideoControl.ScrubbingEnabled = true;
            
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);

            logic.Connect();
            logic.Distance = logic.getNewDistance();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(3000);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                //Is finished check
                if (logic.WorkedDistance >= Constants.RowDistance && isPlaying)
                {
                    finished = true;
                    if (finishedCounter > 3)
                    {
                        this.Reset();
                    }
                    finishedCounter++;
                }

                //Speedcheck
                if (!finished)
                {
                    uint newDistance = logic.getNewDistance();
                    double diff = 0;

                    if (newDistance > logic.Distance)
                    {
                        stop = 0;
                        diff = (double)(newDistance - logic.Distance);
                        logic.Speed = (diff / 3) * 3.6 * logic.getSpeedLevelMultiplier();
                        logic.setSpeedRatio();
                        this.VideoControl.SpeedRatio = logic.SpeedRatio <= 3 ? logic.SpeedRatio : 3;
                        if (!isPlaying)
                        {
                            this.VideoControl.Play();
                            isPlaying = true;
                        }
                    }
                    else
                    {
                        double nextSpeed = Math.Round(logic.Speed / 1.3, 1);
                        if (nextSpeed >= 5)
                        {
                            logic.Speed = nextSpeed;
                            logic.setSpeedRatio();
                            this.VideoControl.SpeedRatio = logic.SpeedRatio <= 3 ? logic.SpeedRatio : 3;
                        }
                        else
                        {
                            if (isPlaying)
                            {
                                this.VideoControl.Pause();
                                logic.Speed = 0;
                                logic.SpeedRatio = 0;
                                isPlaying = false;
                            }
                            if (stop >= 1)
                            {
                                TimeSpan ts = new TimeSpan(0, 0, 0, 0, 0);
                                this.VideoControl.Position = ts;
                                isPlaying = false;
                            }
                            stop++;
                        }
                    }
                    logic.Distance = newDistance;
                    var prtg = (this.VideoControl.Position.TotalSeconds / this.VideoControl.NaturalDuration.TimeSpan.TotalSeconds);
                    logic.WorkedDistance = prtg * Constants.RowDistance;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt! Hibaüzenet: " + ex.Message);
                logic.Speed = 0;
                logic.SpeedRatio = 0;
                this.Close();
            }
        }

        private void Reset()
        {
            this.VideoControl.Pause();
            logic.Speed = 0;
            logic.SpeedRatio = 0;
            logic.WorkedDistance = 0;
            isPlaying = false;
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, 0);
            this.VideoControl.Position = ts;
            isPlaying = false;
            finished = false;
        }

        private void btn_speedUp_Click_1(object sender, RoutedEventArgs e)
        {
            logic.tDistance += 5;
        }

        private void btn_progress_Click(object sender, RoutedEventArgs e)
        {
            var prg = this.VideoControl.Position.TotalSeconds;
            var percentage = (this.VideoControl.Position.TotalSeconds / this.VideoControl.NaturalDuration.TimeSpan.TotalSeconds);
            MessageBox.Show(percentage.ToString());
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Reset();
            timer.Stop();
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
    }
}
