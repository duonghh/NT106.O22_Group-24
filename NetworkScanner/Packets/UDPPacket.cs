﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetworkScanner.Packets
{
    public class UDPPacket
    {
        private ushort _usSourcePort;               // 16 bits for source port  
        private ushort _usDestinationPort;          // 16 bits for destination port
        private ushort _usLength;                   // 16 bits for lenght(in octets)
        private short _sChecksum;                   // 16 bits for checksum
        private byte[] _bUDPData = new byte[4096];  // Buffer for data carried by UDP datagram

        public UDPPacket(byte[] byBuffer, int nReceived)
        {
            // Preparing for data reading from UDP packets
            MemoryStream memoryStream = null;
            BinaryReader binaryReader = null;

            try
            {
                memoryStream = new MemoryStream(byBuffer, 0, nReceived);
                binaryReader = new BinaryReader(memoryStream);

                // reading first 16 bits wich contains source port
                //IPAddress.NetworkToHostOrder(...) -> This method converts a value from host byte order to network byte order
                _usSourcePort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                // 16 bits contains destination port
                _usDestinationPort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                //next 16 bits contains lenght
                _usLength = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                // next 16 bits contains checksum
                _sChecksum = IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                // copy UDP packet data in to a buffer
                Array.Copy(byBuffer, 8, _bUDPData, 0, nReceived - 8);
            }
            catch (Exception) { }

            finally
            {
                binaryReader.Close();
                memoryStream.Close();
            }
        }

        public string SourcePort
        {
            get { return _usSourcePort.ToString(); }
        }

        public string DestinationPort
        {
            get { return _usDestinationPort.ToString(); }
        }

        public string Length
        {
            get { return _usLength.ToString(); }
        }

        public string Checksum
        {
            get { return "0x" + _sChecksum.ToString("x"); }
        }

        public byte[] Data
        {
            get { return _bUDPData; }
        }
    }
}
