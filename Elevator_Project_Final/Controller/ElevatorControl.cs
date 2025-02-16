using System;
using System.Collections.Generic;
using System.Linq;
using Elevator_Project.Models;
using Elevator_Project.Service;
using static Elevator_Project.Service.IElevatorControl;

namespace Elevator_Project.Controller
{
    public class ElevatorControl : IElevatorControl
    {
        public List<IElevator> Elevators { get; }

        public ElevatorControl(List<IElevator> elevators)
        {
            Elevators = elevators;
        }

        public IElevator? DispatchElevator(IElevatorRequest request)
        {
            var availableElevators = Elevators
                .Where(e => e.Direction == ElevatorDirection.Idle && e.NumberOfPassengers < e.MaxCapacity)
                .ToList();

            if (!availableElevators.Any())
            {
                return null;
            }

            var selected = availableElevators
                .OrderBy(e => Math.Abs(e.CurrentFloor - request.Floor))
                .First();

            selected.SetTarget(request.Floor);

            int availableSpace = selected.MaxCapacity - selected.NumberOfPassengers;
            int boarding = Math.Min(availableSpace, request.Passengers);

            selected.NumberOfPassengers += boarding;

            return selected;
        }

        public void UpdateElevators()
        {
            foreach (var elevator in Elevators)
            {
                if (elevator.IsMoving)
                {
                    elevator.MoveOneStep();
                }
            }
        }

        public void PrintElevatorStatus()
        {
            foreach (var elevator in Elevators)
            {
                Console.WriteLine(elevator.GetStatus());
            }
        }
    }
}
