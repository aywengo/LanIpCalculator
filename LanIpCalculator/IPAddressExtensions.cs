using System;
using System.Net;

namespace LanIpCalculator
{
    public static class IPAddressExtensions
    {
        public static IPAddress GetBroadcastAddress(this IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }
            return new IPAddress(broadcastAddress);
        }

        public static IPAddress GetLastSubnetAddress(this IPAddress address, IPAddress subnetMask)
        {
            var broadcastAddress = address.GetBroadcastAddress(subnetMask);

            byte[] ipAdressBytes = broadcastAddress.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

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

        public static IPAddress GetNetworkAddress(this IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] networkAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < networkAddress.Length; i++)
            {
                networkAddress[i] = (byte)(ipAdressBytes[i] & (subnetMaskBytes[i]));
            }
            return new IPAddress(networkAddress);
        }

        public static IPAddress GetFirstSubnetAddress(this IPAddress address, IPAddress subnetMask)
        {
            var networkAddress = address.GetNetworkAddress(subnetMask);

            byte[] ipAdressBytes = networkAddress.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

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


        public static bool IsInSameSubnet(this IPAddress address2, IPAddress address, IPAddress subnetMask)
        {
            IPAddress network1 = address.GetNetworkAddress(subnetMask);
            IPAddress network2 = address2.GetNetworkAddress(subnetMask);

            return network1.Equals(network2);
        }
    }
}
