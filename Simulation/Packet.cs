using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    public struct Packet
    {
        public string Content;
        public string Location;
        public string Destination;
        public int TTL;
        public List<string> PathTaken;

        // Constructor
        public Packet(string _content, string _location, string _destination, int _ttl)
        {
            Content = _content;
            Location = _location;
            Destination = _destination;
            TTL = _ttl;
            PathTaken = new List<string>();
            PathTaken.Add(_location);
        }
    }
}
