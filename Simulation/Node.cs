using System;
using System.Collections.Generic;

namespace Simulation
{
    class Node
    {
        private string _ip;
        private Dictionary<Node, int> _next = new Dictionary<Node, int>();
        private Queue<Packet> _packets = new Queue<Packet>();
        private int _maxPackets;

        static Random _rnd = new Random();

        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }

        public Dictionary<Node, int> Next
        {
            get { return _next; }
            set { _next = value; }
        }

        public Queue<Packet> Packets
        {
            get { return _packets; }
            set { _packets = value; }
        }

        public int MaxPackets
        {
            get { return _maxPackets; }
            set { _maxPackets = value; }
        }

        // Constructor
        public Node(List<Node> network, string ipPrefix, int maxPackets)
        {
            Ip = GenerateIp(network, ipPrefix);
            MaxPackets = maxPackets;
        }

        // Add item to packets queue
        public void AddPacketToQueue(Packet packet, Node destination)
        {
            if (Packets.Count < MaxPackets)
                if (Ip == destination.Ip)
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
        private static string GenerateIp(List<Node> network, string ipPrefix)
        {
            bool ipIsDuplicate;
            string newIp;

            do
            {
                ipIsDuplicate = false;
                newIp = ipPrefix;
                int number = _rnd.Next(256);
                newIp += number.ToString();

                // Check for duplicate
                foreach(Node node in network)
                {
                    if (node.Ip == newIp)
                    {
                        ipIsDuplicate = true;
                        break;
                    }
               
                }

            } while (ipIsDuplicate);

            return newIp;
        }
    }
}
