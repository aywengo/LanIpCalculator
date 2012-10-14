using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Text;

namespace PhoneApp2
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCalculate(object sender, RoutedEventArgs e)
        {
            StringBuilder builder = new StringBuilder();

            IPAddress ipAdr;
            int maskLenght;
            if (IPAddress.TryParse(IP.Text, out ipAdr) && int.TryParse(MaskLength.Text, out maskLenght) && maskLenght < 31)
            {
                var snm = SubnetMask.CreateByNetBitLength(maskLenght);
                int maxHostAmount = (2 << (31 - maskLenght)) - 2;

                builder.Append(string.Format("Your IP: {0} \n", IP.Text));
                builder.Append(string.Format("Subnet mask: {0} \n", snm));
                builder.Append(string.Format("Network IP: {0} \n", ipAdr.GetNetworkAddress(snm)));
                builder.Append(string.Format("Network broadcast IP: {0} \n",  ipAdr.GetBroadcastAddress(snm)));
                builder.Append(string.Format("Max hosts amount: {0} \n", maxHostAmount));
            }
            else
            {
                builder.Append("Ups, some unknown error has been occured :)");                
            }

            Result.Text = builder.ToString();
        }

        private void OnClear(object sender, EventArgs e)
        {
            IP.Text = string.Empty;
            MaskLength.Text = string.Empty;
            Result.Text = string.Empty;
        }
    }
}