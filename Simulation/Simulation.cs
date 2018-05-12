using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulation
{
    public static class Simulation
    {
        // Program settings
        private static int _totalNodes = 10;
        private static int _packetsToSend = 100;
        private static int _maxPacketsPerNode = 100;
        private static int _maxNodesConnected = 4;
        private static int[] _packetTtlRange = { 3, 8 }; // Range of TTL for packets
        private static string _ipPrefix = "192.168.1.";
        private static string _tab = "  ";

        public static void Initialize(int totalNodes, int packetsToSend, int maxNodesConnected, int[] packetTtlRange, string ipPrefix, int maxPacketsPerNode, string tab)
        {
            _totalNodes = totalNodes;
            _packetsToSend = packetsToSend;
            _maxNodesConnected = maxNodesConnected;
            _packetTtlRange = packetTtlRange;
            _ipPrefix = ipPrefix;
            _maxPacketsPerNode = maxPacketsPerNode;
            _tab = tab;
        }

        public static void Run()
        {

            // Initialize random number generator
            Random rnd = new Random();

            // Create the network
            List<Node> network = new List<Node>();

            // Create all Nodes
            for (int i = 0; i < _totalNodes; ++i)
            {
                Node newNode = new Node(network, _ipPrefix, _maxPacketsPerNode);
                network.Add(newNode);
            }

            // Set source and destination
            Node source = network.First();
            Node destination = network.Last();
            Program.NodesToSend.Enqueue(source);

            // Link Nodes together
            foreach ( Node node in network )
            {
                int numberOfNeighbors = rnd.Next(1, (_maxNodesConnected + 1));

                // Connect node to a random number of nodes
                for (int j = 0; j < (numberOfNeighbors - node.Next.Count); ++j)
                {
                    // Initialize the node with minimum connections to be the first unless we are on the first node
                    Node minConnections = network.First();
                    int minConnectionsIndex = 0;

                    if (node == network.First())
                    {
                        minConnections = network[1];
                        minConnectionsIndex = 1;
                    }

                    // Find the node with the least number of connections
                    for (int k = 0; k < _totalNodes; ++k)
                    {
                        if (network[k] != node 
                            && node.Next.ContainsKey(network[k]) == false
                            && network.ElementAt(k).Next.Count < _maxNodesConnected
                            && network.ElementAt(k).Next.Count < minConnections.Next.Count)
                        {
                            minConnections = network.ElementAt(k);
                            minConnectionsIndex = k;
                        }
                    }

                    if (node.Next.ContainsKey(minConnections) == false
                        && minConnections.Next.ContainsKey(node) == false
                        && minConnections.Next.Count < _maxNodesConnected
                        && node.Next.Count < _maxNodesConnected)
                    {
                        // Generate a random number for the distance, up to 10
                        int rndNumber = rnd.Next(10);
                        node.Next.Add(minConnections, rndNumber);
                        network.ElementAt(minConnectionsIndex).Next.Add(node, rndNumber);
                    }
                }

            }

            // Randomise the network "Shuffle"
            network = network.OrderBy(x => rnd.Next()).ToList();

            // Create all packets and place them in the starting node
            for (int i = 0; i < _packetsToSend; ++i)
            {
                string content = "Packet with index " + i;
                int ttl = rnd.Next(_packetTtlRange[0], (_packetTtlRange[1] + 1));

                Packet packet = new Packet(content, source.Ip, destination.Ip, ttl, null, 0);

                source.Packets.Enqueue(packet);
            }

            // Send all packets from source node
            while (Program.NodesToSend.Count > 0)
                SendAllPackets(destination, Program.NodesToSend.Dequeue());

            // Pring a list of nodes and packets for debugging
            Program.Print(source, destination, network, _packetsToSend, _tab);

        }
        
        private static void SendAllPackets(Node destination, Node current)
        {
            if (current.Ip != destination.Ip)
            {
                foreach (Packet packet in current.Packets)
                {
                    if (packet.Ttl != 0)
                    {
                        foreach (KeyValuePair<Node, int> neighbor in current.Next)
                        {
                            // Send the packet only if it's not going back or to the original sender
                            if (neighbor.Key.Ip != packet.Sender && neighbor.Key.Ip != packet.Source)
                            {
                                Packet newPacket = Packet.Clone(packet);
                                newPacket.Sender = current.Ip;
                                newPacket.PathTaken.Add(neighbor.Key.Ip);
                                newPacket.Hops++;
                                newPacket.Ttl--;
                                neighbor.Key.AddPacketToQueue(newPacket, destination);
                                Program.NodesToSend.Enqueue(neighbor.Key);

                                // Add the packet route to the routing table
                                Route newRoute = new Route(1, neighbor.Key.Ip, neighbor.Value);
                                Program.RoutingTable.Add(newRoute);
                            }
                        }
                    }
                }
                // All packets are sent so clear the queue
                current.Packets.Clear();

            }
        }
    }
}