using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace LanIpCalculator
{
    public class LanIpAddress
    {
        public IPAddress IPadress { get; private set; }
        public int MaskLegth { get; private set; }

        public IPAddress SubNetMask
        {
            get
            {
                int hostPartLength = 32 - MaskLegth;
                int netPartLength = MaskLegth;

                if (netPartLength < 2)
                    throw new ArgumentException("Number of hosts is to large for IPv4");

                Byte[] binaryMask = new byte[4];

                for (int i = 0; i < 4; i++)
                {
                    if (i * 8 + 8 <= netPartLength)
                        binaryMask[i] = (byte)255;
                    else if (i * 8 > netPartLength)
                        binaryMask[i] = (byte)0;
                    else
                    {
                        int oneLength = netPartLength - i * 8;
                        string binaryDigit =
                            String.Empty.PadLeft(oneLength, '1').PadRight(8, '0');
                        binaryMask[i] = Convert.ToByte(binaryDigit, 2);
                    }
                }
                return new IPAddress(binaryMask);
                
                //return SubnetMask.CreateByNetBitLength(MaskLegth);
            }
        }

        public int MaxHostAmount
        {
            get
            {
                return (2 << (31 - MaskLegth)) - 2;
            }
        }

        public IPAddress NetworkIP
        {
            get
            {
                byte[] ipAdressBytes = IPadress.GetAddressBytes();
                byte[] subnetMaskBytes = SubNetMask.GetAddressBytes();

                if (ipAdressBytes.Length != subnetMaskBytes.Length)
                    throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

                byte[] networkAddress = new byte[ipAdressBytes.Length];
                for (int i = 0; i < networkAddress.Length; i++)
                {
                    networkAddress[i] = (byte)(ipAdressBytes[i] & (subnetMaskBytes[i]));
                }
                return new IPAddress(networkAddress);
            }
        }

        public IPAddress BroadastNetworkIP
        {
            get
            {
                byte[] ipAdressBytes = IPadress.GetAddressBytes();
                byte[] subnetMaskBytes = SubNetMask.GetAddressBytes();

                if (ipAdressBytes.Length != subnetMaskBytes.Length)
                    throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

                byte[] broadcastAddress = new byte[ipAdressBytes.Length];
                for (int i = 0; i < broadcastAddress.Length; i++)
                {
                    broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
                }
                return new IPAddress(broadcastAddress);
            }
        }

        public IPAddress FirstSubnetIP
        {
            get
            {
                var networkAddress = IPadress.GetNetworkAddress(SubNetMask);

                byte[] ipAdressBytes = networkAddress.GetAddressBytes();
                byte[] subnetMaskBytes = SubNetMask.GetAddressBytes();

                byte[] firstNetworkAddress = new byte[ipAdressBytes.Length];
                for (int i = 0; i < firstNetworkAddress.Length; i++)
                {
                    firstNetworkAddress[i] =
                        subnetMaskBytes[i] == 255
                        ? (byte)ipAdressBytes[i]
                        : (byte)(++ipAdressBytes[i]);
                }
                return new IPAddress(firstNetworkAddress);
            }
        }

        public IPAddress LastSubnetIP
        {
            get
            {
                var broadcastAddress = IPadress.GetBroadcastAddress(SubNetMask);

                byte[] ipAdressBytes = broadcastAddress.GetAddressBytes();
                byte[] subnetMaskBytes = SubNetMask.GetAddressBytes();

                byte[] latestNetworkAddress = new byte[ipAdressBytes.Length];
                for (int i = 0; i < latestNetworkAddress.Length; i++)
                {
                    latestNetworkAddress[i] =
                        subnetMaskBytes[i] == 255
                        ? (byte)ipAdressBytes[i]
                        : (byte)(--ipAdressBytes[i]);
                }
                return new IPAddress(latestNetworkAddress);
            }
        }


        public LanIpAddress(IPAddress iPadress, int maskLegth)
        {
            IPadress = iPadress;
            MaskLegth = maskLegth;
        }
    }
}
