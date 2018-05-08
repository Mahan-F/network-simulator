using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    class Route
    {
        private int _time;
        private string _destination;
        private int _cost;
        private int _port;

        static Random rnd = new Random();

        public int Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public string Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }

        public int Cost
        {
            get { return _cost; }
            set { _cost = value; }
        }

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        public Route(int _time, string _destination, int _cost)
        {
            Time = _time;
            Destination = _destination;
            Cost = _cost;
            Port = GeneratePort();
        }

        private int GeneratePort()
        {
            return rnd.Next(1023, 65536);
        }
    }
}
