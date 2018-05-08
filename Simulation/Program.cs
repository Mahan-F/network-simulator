using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Simulation
{
    class Program
    {

        public static Queue<Node> nodesToSend = new Queue<Node>();
        public static List<Route> routingTable = new List<Route>();

        static void Main()
        {
            // Program settings
            int totalNodes = 10;
            int packetsToSend = 100;
            int maxPacketsPerNode = 100;
            int maxNodesConnected = 4;
            int[] packetTtlRange = { 3, 8 }; // Range of TTL for packets
            string ipPrefix = "192.168.1.";

            RunSimulation(totalNodes, packetsToSend, maxNodesConnected, packetTtlRange, ipPrefix, maxPacketsPerNode);
        }

        static void RunSimulation(int TotalNodes, int PacketsToSend, int MaxNodesConnected, int[] packetTtlRange, string IpPrefix, int maxPacketsPerNode)
        {

            // Initialize random number generator
            Random rnd = new Random();

            // Create the network
            List<Node> network = new List<Node>();

            // Create all Nodes
            for (int i = 0; i < TotalNodes; ++i)
            {
                Node newNode = new Node(network, IpPrefix, maxPacketsPerNode);
                network.Add(newNode);
            }

            // Set source and destination
            Node source = network.First();
            Node destination = network.Last();
            nodesToSend.Enqueue(source);

            // Link Nodes together
            foreach ( Node node in network )
            {
                int numberOfNeighbors = rnd.Next(1, (MaxNodesConnected + 1));

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
                    for (int k = 0; k < TotalNodes; ++k)
                    {
                        if (network[k] != node 
                            && node.Next.IndexOf(network[k]) == -1 
                            && network.ElementAt(k).Next.Count < MaxNodesConnected
                            && network.ElementAt(k).Next.Count < minConnections.Next.Count)
                        {
                            minConnections = network.ElementAt(k);
                            minConnectionsIndex = k;
                        }
                    }

                    if (node.Next.IndexOf(minConnections) == -1
                        && minConnections.Next.IndexOf(node) == -1
                        && minConnections.Next.Count < MaxNodesConnected
                        && node.Next.Count < MaxNodesConnected)
                    {
                        node.Next.Add(minConnections);
                        network.ElementAt(minConnectionsIndex).Next.Add(node);
                    }
                }

            }

            // Randomise the network "Shuffle"
            network = network.OrderBy(x => rnd.Next()).ToList();

            // Create all packets and place them in the starting node
            for (int i = 0; i < PacketsToSend; ++i)
            {
                string content = "Packet with index " + i;
                int ttl = rnd.Next(packetTtlRange[0], (packetTtlRange[1] + 1));

                Packet packet = new Packet(content, source.IP, destination.IP, ttl, null, 0);

                source.Packets.Enqueue(packet);
            }

            // Send all packets from source node
            while (nodesToSend.Count > 0)
                SendAllPackets(source, destination, nodesToSend.Dequeue());

            // Pring a list of nodes and packets for debugging
            Print(source, destination, network, PacketsToSend);

            // Keep console open
            Console.WriteLine("\n\nPress enter to close...");
            Console.ReadLine();
        }
        
        static void SendAllPackets(Node source, Node destination, Node current)
        {
            if (current.IP != destination.IP)
            {
                foreach (Packet packet in current.Packets)
                {
                    if (packet.TTL != 0)
                    {
                        foreach (Node neighbor in current.Next)
                        {
                            // Send the packet only if it's not going back or to the original sender
                            if (neighbor.IP != packet.Sender && neighbor.IP != packet.Source)
                            {
                                Packet newPacket = Packet.Clone(packet);
                                newPacket.Sender = current.IP;
                                newPacket.PathTaken.Add(neighbor.IP);
                                newPacket.Hops++;
                                newPacket.TTL--;
                                neighbor.AddPacketToQueue(newPacket, destination);
                                nodesToSend.Enqueue(neighbor);

                                // Add the packet route to the routing table
                                Route newRoute = new Route(1, neighbor.IP, newPacket.Hops);
                                routingTable.Add(newRoute);
                            }
                        }
                    }
                }
                // All packets are sent so clear the queue
                current.Packets.Clear();

            }
        }

        static void Print(Node source, Node destination, List<Node> network, int packetsToSend)
        {

            // Print the list of all nodes, their IP and their neighbors' IP
            Console.WriteLine("Starting node: " + source.IP);
            Console.WriteLine("Ending node: " + destination.IP);
            Console.WriteLine("Packets received: " + destination.Packets.Count + "/" + packetsToSend + "\n");

            foreach (Node item in network)
            {
                Console.WriteLine("Node details: ");
                Console.WriteLine("Address: " + item.IP);
                Console.WriteLine("Neighboring nodes: ");
                foreach (Node neighbor in item.Next)
                {
                    Console.WriteLine("\t" + neighbor.IP);
                }
                Console.WriteLine("Packets: ");
                foreach (Packet packet in item.Packets)
                {
                    Console.WriteLine("\t" + packet.Content + " with TTL " + packet.TTL);
                    Console.WriteLine("\t\tPath taken: ");
                    foreach (string path in packet.PathTaken)
                    {
                        Console.WriteLine("\t\t" + path);
                    }
                }
                if (item.Packets.Count == 0)
                    Console.WriteLine("No packets.");
                Console.WriteLine("\n\n");
            }

            //Print routing table
            Console.WriteLine("\nRouting table:\n");
            Console.WriteLine("Time\tDestination\tCost\tPort");
            foreach(Route route in routingTable)
            {
                Console.WriteLine(route.Time + "\t" + route.Destination + "\t" + route.Cost + "\t" + route.Port);
            }

            /*
            // Print the table of all nodes and their connections
            for (int i = 0; i < TotalNodes; i++)
            {
                Console.Write("\t" + i);
            }
            for ( int r = 0; r < TotalNodes; ++r )
            {
                Console.Write("\n" + r + "\t");
                for ( int c = 0; c < TotalNodes; ++c )
                {
                    if (network[r].Next.Contains(network[c]))
                        Console.Write("x\t");
                    else
                        Console.Write(" \t");
                }
            }*/
        }
    }
}
