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
        public int TTL;
        public List<string> PathTaken;

        // Constructor
        public Packet(string _content, string _source, string _destination, int _ttl)
        {
            Content = _content;
            Source = _source;
            Sender = _source;
            Destination = _destination;
            TTL = _ttl;
            PathTaken = new List<string>();
            PathTaken.Add(_source);
        }

        public static Packet Clone(Packet original)
        {
            Packet packet = new Packet(original.Content, original.Source, original.Destination, original.TTL);
            return packet;
        }
    }
}
