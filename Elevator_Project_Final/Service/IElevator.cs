using Elevator_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator_Project.Service
{
    public interface IElevator
    {
        int Id { get; }
        int CurrentFloor { get; set; }
        int TargetFloor { get; }
        ElevatorDirection Direction { get; }
        int NumberOfPassengers { get; set; }
        int MaxCapacity { get; }
        bool IsMoving { get; }
        void SetTarget(int floor);
        void MoveOneStep();
        string GetStatus();
    }
}
