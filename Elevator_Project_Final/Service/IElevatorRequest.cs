using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator_Project.Service
{
    public interface IElevatorRequest
    {
        int Floor { get; }
        int Passengers { get; }
    }
}
