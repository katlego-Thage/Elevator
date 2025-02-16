using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator_Project.Service
{
    public interface IElevatorControl
    {
        IElevator? DispatchElevator(IElevatorRequest request);
        void UpdateElevators();
        void PrintElevatorStatus();
    }
}
