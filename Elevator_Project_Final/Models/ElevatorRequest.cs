using Elevator_Project.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator_Project.Models
{
    public class ElevatorRequest : IElevatorRequest
    {
        public int Floor { get; }
        public int Passengers { get; }

        public ElevatorRequest(int floor, int passengers)
        {
            Floor = floor;
            Passengers = passengers;
        }
    }
}
