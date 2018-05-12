using System.Collections.Generic;

namespace Simulation
{
    public struct Packet
    {
        public readonly string Content;
        public readonly string Source;
        public string Sender;
        private readonly string _destination;
        public int Hops;
        public int Ttl;
        public readonly List<string> PathTaken;

        // Constructor
        public Packet(string content, string source, string destination, int ttl, List<string> pathTaken, int hops)
        {
            Content = content;
            Source = source;
            Sender = source;
            _destination = destination;
            Hops = hops;
            Ttl = ttl;
            PathTaken = new List<string>();
            if (pathTaken == null)
            {
                PathTaken.Add(source);
            }
            else
            {
                foreach (string item in pathTaken)
                {
                    PathTaken.Add(item);
                }
            }
        }

        // Duplicate a packet
        public static Packet Clone(Packet original)
        {
            Packet packet = new Packet(original.Content, original.Source, original._destination, original.Ttl, original.PathTaken, original.Hops);
            return packet;
        }
    }
}
