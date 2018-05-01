using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    class Node
    {
        private string _IP;
        private List<Node> _Next = new List<Node>();
        static Random rnd = new Random();

        public string IP
        {
            get { return _IP; }
            set { _IP = value; }

        }

        public List<Node> Next
        {
            get { return _Next; }
            set { _Next = value; }

        }

        // Constructor
        public Node(List<Node> network, string IpPrefix)
        {
            IP = generateIP(network, IpPrefix);
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
