using System;
using System.Collections.Generic;
using System.Text;

namespace Simulation
{
    class Route
    {
        private static int currentSequence;
        private int _sequenceNo;
        private int _time;
        private string _destination;
        private int _cost;
        private int _port;

        private static readonly Random Rnd = new Random();
        
        public int SequenceNo
        {
            get { return _sequenceNo; }
            set { _sequenceNo = value; }
        }
        
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
            _sequenceNo = currentSequence;
            Time = _time;
            Destination = _destination;
            Cost = _cost;
            Port = GeneratePort();
            currentSequence++;
        }

        private static int GeneratePort()
        {
            return Rnd.Next(1023, 65536);
        }
    }
}
