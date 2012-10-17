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
                var lanIP = new LanIpAddress(ipAdr, maskLenght);

                builder.Append(string.Format(Resource.ResultHostIP, lanIP.IPadress));
                builder.Append(string.Format("\n"));
                builder.Append(string.Format(Resource.ResultSubnetMask, lanIP.SubNetMask));
                builder.Append(string.Format("\n"));
                builder.Append(string.Format(Resource.ResultNetworkIP, lanIP.NetworkIP));
                builder.Append(string.Format("\n"));
                builder.Append(string.Format(Resource.ResultNetworkBroadcastIP, lanIP.BroadastNetworkIP));
                builder.Append(string.Format("\n"));
                builder.Append(string.Format(Resource.ResultMaxHostsAmount, lanIP.MaxHostAmount));
                builder.Append(string.Format("\n"));
                builder.Append(string.Format(Resource.ResultIPRange, lanIP.FirstSubnetIP, lanIP.LastSubnetIP));
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