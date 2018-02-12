using C2Test2.Classes;
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

namespace C2Test2
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        public BusinessLogic bLogic;
        public Options(ref BusinessLogic logic)
        {
            bLogic = logic;
            InitializeComponent();
            this.DataContext = bLogic;
        }

        private void sld_speedlevel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = (Slider)sender;
            if (slider.IsInitialized)
            {
                bLogic.SpeedLevel = (int)e.NewValue;
            }
        }
    }
}
