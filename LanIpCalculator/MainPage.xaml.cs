using System;
using System.Net;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Text;
using LanIpCalculator.Assets;

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
            bool hasErrors = false;

            IPAddress ipAdr;            
            if (!IPAddress.TryParse(IP.Text, out ipAdr))
            {
                builder.Append(string.Format(Resource.WarningIP));
                builder.Append(string.Format("\n"));
                hasErrors = true;
            }

            int maskLenght = string.IsNullOrWhiteSpace(MaskLength.Text) ? 0 : int.Parse(MaskLength.Text);
            if (maskLenght > 30 || maskLenght < 1)
            {
                builder.Append(string.Format(Resource.WarningSubnetMask));
                builder.Append(string.Format("\n"));
                hasErrors = true;
            }
            
            if (!hasErrors)
            {
                var snm = SubnetMask.CreateByNetBitLength(maskLenght);
                int maxHostAmount = (2 << (31 - maskLenght)) - 2;

                builder.Append(string.Format(Resource.ResultHostIP, IP.Text));
                builder.Append(string.Format("\n"));
                builder.Append(string.Format(Resource.ResultSubnetMask, snm));
                builder.Append(string.Format("\n"));
                builder.Append(string.Format(Resource.ResultNetworkIP, ipAdr.GetNetworkAddress(snm)));
                builder.Append(string.Format("\n"));
                builder.Append(string.Format(Resource.ResultNetworkBroadcastIP, ipAdr.GetBroadcastAddress(snm)));
                builder.Append(string.Format("\n"));
                builder.Append(string.Format(Resource.ResultMaxHostsAmount, maxHostAmount));
                builder.Append(string.Format("\n"));
                builder.Append(string.Format(Resource.ResultIPRange, ipAdr.GetFirstSubnetAddress(snm), ipAdr.GetLastSubnetAddress(snm)));
                builder.Append(string.Format("\n"));
            };

            Result.Text = builder.ToString();
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            this.Calculate();
            CalcButton.Focus();
        }
    }
}