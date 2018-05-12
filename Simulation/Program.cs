using System;
using System.Collections.Generic;
using System.Threading;

namespace Simulation
{
    static class Program
    {

        public static readonly Queue<Node> NodesToSend = new Queue<Node>();
        public static readonly List<Route> RoutingTable = new List<Route>();

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

            Simulation.Initialize(totalNodes, packetsToSend, maxNodesConnected, packetTtlRange, ipPrefix, maxPacketsPerNode, tab);
            Simulation.Run();
        }

        public static void Print(Node source, Node destination, List<Node> network, int packetsToSend, string tab)
        {

            // Print the list of all nodes, their IP and their neighbors' IP
            Console.WriteLine("Starting node: " + source.Ip);
            Console.WriteLine("Ending node: " + destination.Ip);
            Console.WriteLine("Packets received: " + destination.Packets.Count + "/" + packetsToSend + "\n");
    
            int option;
            do
            {
                Console.WriteLine("\nChoose an option (-1 to exit):");
                PrintMenu();
                Console.Write("Option: ");

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
                    case 6: Console.WriteLine("Starting node: " + source.Ip);
                        Console.WriteLine("Ending node: " + destination.Ip);
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
            foreach(Route route in RoutingTable)
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
                Console.WriteLine(tab + "Address: " + item.Ip);
                if (type == "neighbors" || type == "all")
                {
                    Console.WriteLine(tab + "Neighboring nodes: ");
                    foreach (KeyValuePair<Node, int> neighbor in item.Next)
                    {
                        Console.WriteLine(tab + tab + neighbor.Key.Ip);
                    }
                }

                if (type == "packets" || type == "all")
                {
                    Console.WriteLine(tab + "Packets: ");
                    foreach (Packet packet in item.Packets)
                    {
                        Console.WriteLine(tab + tab + packet.Content + " with TTL " + packet.Ttl);
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
