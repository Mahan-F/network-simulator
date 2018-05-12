using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Simulation
{
    class Program
    {

        private static Queue<Node> nodesToSend = new Queue<Node>();
        private static List<Route> routingTable = new List<Route>();

        private static void Main()
        {
            // Program settings
            const int totalNodes = 10;
            const int packetsToSend = 100;
            const int maxPacketsPerNode = 100;
            const int maxNodesConnected = 4;
            int[] packetTtlRange = { 3, 8 }; // Range of TTL for packets
            const string ipPrefix = "192.168.1.";
            const string tab = "  ";

            RunSimulation(totalNodes, packetsToSend, maxNodesConnected, packetTtlRange, ipPrefix, maxPacketsPerNode, tab);
        }

        private static void RunSimulation(int TotalNodes, int PacketsToSend, int MaxNodesConnected, int[] packetTtlRange, string IpPrefix, int maxPacketsPerNode, string tab)
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
                            && node.Next.ContainsKey(network[k]) == false
                            && network.ElementAt(k).Next.Count < MaxNodesConnected
                            && network.ElementAt(k).Next.Count < minConnections.Next.Count)
                        {
                            minConnections = network.ElementAt(k);
                            minConnectionsIndex = k;
                        }
                    }

                    if (node.Next.ContainsKey(minConnections) == false
                        && minConnections.Next.ContainsKey(node) == false
                        && minConnections.Next.Count < MaxNodesConnected
                        && node.Next.Count < MaxNodesConnected)
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
            Print(source, destination, network, PacketsToSend, tab);

        }
        
        private static void SendAllPackets(Node source, Node destination, Node current)
        {
            if (current.IP != destination.IP)
            {
                foreach (Packet packet in current.Packets)
                {
                    if (packet.TTL != 0)
                    {
                        foreach (KeyValuePair<Node, int> neighbor in current.Next)
                        {
                            // Send the packet only if it's not going back or to the original sender
                            if (neighbor.Key.IP != packet.Sender && neighbor.Key.IP != packet.Source)
                            {
                                Packet newPacket = Packet.Clone(packet);
                                newPacket.Sender = current.IP;
                                newPacket.PathTaken.Add(neighbor.Key.IP);
                                newPacket.Hops++;
                                newPacket.TTL--;
                                neighbor.Key.AddPacketToQueue(newPacket, destination);
                                nodesToSend.Enqueue(neighbor.Key);

                                // Add the packet route to the routing table
                                Route newRoute = new Route(1, neighbor.Key.IP, neighbor.Value);
                                routingTable.Add(newRoute);
                            }
                        }
                    }
                }
                // All packets are sent so clear the queue
                current.Packets.Clear();

            }
        }

        private static void Print(Node source, Node destination, List<Node> network, int packetsToSend, string tab)
        {

            // Print the list of all nodes, their IP and their neighbors' IP
            Console.WriteLine("Starting node: " + source.IP);
            Console.WriteLine("Ending node: " + destination.IP);
            Console.WriteLine("Packets received: " + destination.Packets.Count + "/" + packetsToSend + "\n");
    
            int option;
            do
            {
                Console.WriteLine("\nChoose an option:");
                PrintMenu();
                
                option = Convert.ToInt32(Console.ReadLine());
                Console.Write("\n");
                Console.Clear();
                
                switch (option)
                {
                    case 1: PrintNodeDetails(network, "none", tab);
                        break;
                    case 2: PrintNodeDetails(network, "neighbors", tab);
                        break;
                    case 3: PrintNodeDetails(network, "packets", tab);
                        break;
                    case 4: PrintNodeDetails(network, "all", tab);
                        break;
                    case 5: PrintRoutingTable();
                        break;
                    case 6: Console.WriteLine("Starting node: " + source.IP);
                        Console.WriteLine("Ending node: " + destination.IP);
                        Console.WriteLine("Packets received: " + destination.Packets.Count + "/" + packetsToSend + "\n");
                        break;
                    default: Console.WriteLine("That is not a valid option, please try again.");
                        break;
                }
            } while (option != -1);

        }

        private static void PrintMenu()
        {
            Console.WriteLine("1. View all Nodes");
            Console.WriteLine("2. View all Nodes along with their neighbors");
            Console.WriteLine("3. View all Nodes along with their packets");
            Console.WriteLine("4. View all Nodes with full details");
            Console.WriteLine("5. View the routing table");
            Console.WriteLine("6. View the program outcome");
            
        }

        private static void PrintRoutingTable()
        {
            Console.WriteLine("\nRouting table:\n");
            Console.WriteLine("Time\tDestination\tCost\tPort");
            foreach(Route route in routingTable)
            {
                Console.WriteLine(route.SequenceNo + "\t" + route.Time + "\t" + route.Destination + "\t" + route.Cost + "\t" + route.Port);
                Thread.Sleep(10);
            }
        }

        private static void PrintNodeDetails(List<Node> network, string type, string tab)
        {
            int nodeIndex = 1;
            Console.WriteLine("Node details: ");
            foreach (Node item in network)
            {
                Console.WriteLine("Node " + nodeIndex + ":");
                Console.WriteLine(tab + "Address: " + item.IP);
                if (type == "neighbors" || type == "all")
                {
                    Console.WriteLine(tab + "Neighboring nodes: ");
                    foreach (KeyValuePair<Node, int> neighbor in item.Next)
                    {
                        Console.WriteLine(tab + tab + neighbor.Key.IP);
                    }
                }

                if (type == "packets" || type == "all")
                {
                    Console.WriteLine(tab + "Packets: ");
                    foreach (Packet packet in item.Packets)
                    {
                        Console.WriteLine(tab + tab + packet.Content + " with TTL " + packet.TTL);
                        Console.WriteLine(tab + tab + tab + "Path taken: ");
                        foreach (string path in packet.PathTaken)
                        {
                            Console.WriteLine(tab + tab + tab + path);
                        }
                    }

                    if (item.Packets.Count == 0)
                        Console.WriteLine(tab + tab + "No packets.");
                }
                
                Console.WriteLine("\n");
                nodeIndex++;
                Thread.Sleep(50);
            }
        }
    }
}
