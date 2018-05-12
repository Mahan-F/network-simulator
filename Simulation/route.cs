using System;

namespace Simulation
{
    class Route
    {
        private static int _currentSequence;

        private static readonly Random Rnd = new Random();
        
        public int SequenceNo { get; }

        public int Time { get; }

        public string Destination { get; }

        public int Cost { get; }

        public int Port { get; }

        public Route(int time, string destination, int cost)
        {
            SequenceNo = _currentSequence;
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
