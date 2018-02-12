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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace C2Test2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BusinessLogic logic;
        public MainWindow()
        {
            logic = new BusinessLogic();
            InitializeComponent();
        }

        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            PMUSBInterface.Initialize();
            var deviceNo = PMUSBInterface.DiscoverPMs(PMUSBInterface.PMtype.PM5_PRODUCT_NAME);
            string message = "";
            if (deviceNo > 0)
            {
                Rowing rowWindow = new Rowing(ref logic);
                rowWindow.ShowDialog();
            }
            else
            {
                message += "Nincs csatlakoztatva eszköz!";
                MessageBox.Show(message);
            }
}

        private void btn_options_Click(object sender, RoutedEventArgs e)
        {
            Options optionsWindow = new Options(ref logic);
            optionsWindow.ShowDialog();
        }

        private void btn_testconnection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PMUSBInterface.Initialize();
                var deviceNo = PMUSBInterface.DiscoverPMs(PMUSBInterface.PMtype.PM5_PRODUCT_NAME);
                string message = "";
                if (deviceNo > 0)
                {
                    string serialNo = PMUSBInterface.getSerialNumber(0);
                    message = "Eszközök csatlakoztatva: " + deviceNo.ToString() + "\n";
                    message += "Szériaszám: " + serialNo.ToString();
                }
                else
                {
                    message += "Nincs csatlakoztatva eszköz!";
                }
                MessageBox.Show(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt. Hibaüzenet: " + ex.Message);
            }
            


        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
