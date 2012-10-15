using System;
using System.Net;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Text;

namespace LanIpCalculator
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
            this.Calculate();
        }

        private void OnClear(object sender, EventArgs e)
        {
            IP.Text = string.Empty;
            MaskLength.Text = string.Empty;
            Result.Text = string.Empty;
        }

        private void Calculate()
        {
            StringBuilder builder = new StringBuilder();

            IPAddress ipAdr;
            int maskLenght;
            if (IPAddress.TryParse(IP.Text, out ipAdr) && int.TryParse(MaskLength.Text, out maskLenght) && maskLenght < 31)
            {
                var snm = SubnetMask.CreateByNetBitLength(maskLenght);
                int maxHostAmount = (2 << (31 - maskLenght)) - 2;

                builder.Append(string.Format("Host IP: {0} \n", IP.Text));
                builder.Append(string.Format("Subnet mask: {0} \n", snm));
                builder.Append(string.Format("Network IP: {0} \n", ipAdr.GetNetworkAddress(snm)));
                builder.Append(string.Format("Network broadcast IP: {0} \n", ipAdr.GetBroadcastAddress(snm)));
                builder.Append(string.Format("Max hosts amount: {0} \n", maxHostAmount));
                builder.Append(string.Format("IP range: {0} - {1} \n", ipAdr.GetFirstSubnetAddress(snm), ipAdr.GetLastSubnetAddress(snm)));
            }
            else
            {
                builder.Append("Ups, some unknown error has been occured :)");
            }

            Result.Text = builder.ToString();
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            this.Calculate();
        }
    }
}