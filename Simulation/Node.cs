using System;
using System.Collections.Generic;

namespace Simulation
{
    class Node
    {
        private static readonly Random Rnd = new Random();

        public string Ip { get; }

        public Dictionary<Node, int> Next { get; set; } = new Dictionary<Node, int>();

        public Queue<Packet> Packets { get; set; } = new Queue<Packet>();

        private int MaxPackets { get; }

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
                int number = Rnd.Next(256);
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
