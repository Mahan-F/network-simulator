﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Simulation
{
    class Program
    {

        static void Main()
        {
            // Program settings
            int TotalNodes = 10;
            int PacketsToSend = 10;
            int MaxNodesConnected = 3;
            int[] packetTtlRange = { 3, 8 }; // Range of TTL for packets
            string IpPrefix = "192.168.1.";

            RunSimulation(TotalNodes, PacketsToSend, MaxNodesConnected, packetTtlRange, IpPrefix);
        }

        static void RunSimulation(int TotalNodes, int PacketsToSend, int MaxNodesConnected, int[] packetTtlRange, string IpPrefix)
        {

            // Initialize random number generator
            Random rnd = new Random();

            // Create the network
            List<Node> network = new List<Node>();

            // Create all Nodes
            for (int i = 0; i < TotalNodes; ++i)
            {
                Node newNode = new Node(network, IpPrefix);
                network.Add(newNode);
            }

            // Set source and destination
            Node source = network.First();
            Node destination = network.Last();

            // Link Nodes together
            foreach ( Node node in network )
            {

                for (int j = 0; j < rnd.Next(1, (MaxNodesConnected + 1)); ++j)
                {
                    Node minConnections = network.First();
                    if (node == network.First())
                        minConnections = network[1];

                    for (int k = 0; k < TotalNodes; ++k)
                    {
                        if (network[k] != node 
                            && node.Next.IndexOf(network[k]) == -1 
                            && network.ElementAt(k).Next.Count < minConnections.Next.Count)
                        {
                            minConnections = network.ElementAt(k);
                        }
                    }

                    if (node.Next.IndexOf(minConnections) == -1 
                        && minConnections.Next.Count < MaxNodesConnected)
                        node.Next.Add(minConnections);
                }

            }

            // Randomise the network "Shuffle"
            network = network.OrderBy(x => rnd.Next()).ToList();

            // Create all packets and place them in the starting node
            for (int i = 0; i < PacketsToSend; ++i)
            {
                string content = "Packet with index " + i;
                int ttl = rnd.Next(packetTtlRange[0], (packetTtlRange[1] + 1));

                Packet packet = new Packet(content, source.IP, destination.IP, ttl);

                source.Packets.Enqueue(packet);
            }










            /***************
             * DEBUG STUFF *
             **************/

            // Print the list of all nodes, their IP and their neighbors' IP
            Console.WriteLine("Starting node: " + source.IP);
            Console.WriteLine("Ending node: " + destination.IP + "\n");

            foreach ( Node item in network )
            {
                Console.WriteLine("Node details: ");
                Console.WriteLine("Address: " + item.IP);
                Console.WriteLine("Neighboring nodes: ");
                foreach( Node neighbor in item.Next )
                {
                    Console.WriteLine("\t" + neighbor.IP);
                }
                Console.WriteLine("Packets: ");
                foreach (Packet packet in item.Packets)
                {
                    Console.WriteLine("\t" + packet.Content + " with TTL " + packet.TTL);
                    Console.WriteLine("\t\tPath taken: ");
                    foreach ( string path in packet.PathTaken )
                    {
                        Console.WriteLine("\t\t" + path);
                    }
                }
                if (item.Packets.Count == 0)
                    Console.WriteLine("No packets.");
                Console.WriteLine("\n\n");
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

            // Keep console open
            Console.WriteLine("\n\nPress enter to close...");
            Console.ReadLine();
        }
    }
}
