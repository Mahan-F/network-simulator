using System;

namespace Simulation
{
    class Route
    {
        private static int _currentSequence;
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

        public Route(int time, string destination, int cost)
        {
            _sequenceNo = _currentSequence;
            Time = time;
            Destination = destination;
            Cost = cost;
            Port = GeneratePort();
            _currentSequence++;
        }

        private static int GeneratePort()
        {
            return Rnd.Next(1023, 65536);
        }
    }
}
