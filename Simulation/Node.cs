using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    class Node
    {
        private string _IP;
        private Dictionary<Node, int> _Next = new Dictionary<Node, int>();
        private Queue<Packet> _Packets = new Queue<Packet>();
        private int _MaxPackets;

        static Random rnd = new Random();

        public string IP
        {
            get { return _IP; }
            set { _IP = value; }
        }

        public Dictionary<Node, int> Next
        {
            get { return _Next; }
            set { _Next = value; }
        }

        public Queue<Packet> Packets
        {
            get { return _Packets; }
            set { _Packets = value; }
        }

        public int MaxPackets
        {
            get { return _MaxPackets; }
            set { _MaxPackets = value; }
        }

        // Constructor
        public Node(List<Node> network, string ipPrefix, int maxPackets)
        {
            IP = generateIP(network, ipPrefix);
            MaxPackets = maxPackets;
        }

        // Add item to packets queue
        public void AddPacketToQueue(Packet packet, Node destination)
        {
            if (Packets.Count < MaxPackets)
                if (IP == destination.IP)
                {
                    bool packetExists = false;
                    foreach ( Packet item in Packets )
                    {
                        if (item.Content == packet.Content)
                            packetExists = true;
                    }
                    if (packetExists == false)
                        Packets.Enqueue(packet);
                }
                else
                    Packets.Enqueue(packet);
        }

        // Generate a random IP address
        static private string generateIP(List<Node> network, string IpPrefix)
        {
            bool IpIsDuplicate;
            string newIP = IpPrefix;

            do
            {
                IpIsDuplicate = false;
                newIP = IpPrefix;
                int number = rnd.Next(256);
                newIP += number.ToString();

                // Check for duplicate
                foreach(Node node in network)
                {
                    if (node.IP == newIP)
                    {
                        IpIsDuplicate = true;
                        break;
                    }
               
                }

            } while (IpIsDuplicate);

            return newIP;
        }
    }
}
