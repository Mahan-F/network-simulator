using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    public struct Packet
    {
        public string Content;
        public string Source;
        public string Sender;
        public string Destination;
        public int Hops;
        public int TTL;
        public List<string> PathTaken;

        // Constructor
        public Packet(string _content, string _source, string _destination, int _ttl, List<string> _pathTaken, int _hops)
        {
            Content = _content;
            Source = _source;
            Sender = _source;
            Destination = _destination;
            Hops = _hops;
            TTL = _ttl;
            PathTaken = new List<string>();
            if (_pathTaken == null)
            {
                PathTaken.Add(_source);
            }
            else
            {
                foreach (string item in _pathTaken)
                {
                    PathTaken.Add(item);
                }
            }
        }

        // Duplicate a packet
        public static Packet Clone(Packet original)
        {
            Packet packet = new Packet(original.Content, original.Source, original.Destination, original.TTL, original.PathTaken, original.Hops);
            return packet;
        }
    }
}
