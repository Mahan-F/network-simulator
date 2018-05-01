using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    class Packet
    {
        private string _content;
        private string _location;
        private string _destination;
        private int _ttl;
        private int _timesTransferred;

        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public string Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }

        public int TTL
        {
            get { return _ttl; }
            set { _ttl = value; }
        }

        public int TimesTransferred
        {
            get { return _timesTransferred; }
            set { _timesTransferred = value; }
        }

        // Constructor
        public Packet(string _content, string _location, string _destination, int _ttl)
        {
            Content = _content;
            Location = _location;
            Destination = _destination;
            TTL = _ttl;
            TimesTransferred = 0;
        }
    }
}
