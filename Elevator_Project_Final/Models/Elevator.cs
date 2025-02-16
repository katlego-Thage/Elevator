using Elevator_Project.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator_Project.Models
{
    public enum ElevatorDirection
    {
        Up,
        Down,
        Idle
    }

    public class Elevator : IElevator
    {
        public int Id { get; }
        public int CurrentFloor { get; set; }
        public int TargetFloor { get; private set; }
        public ElevatorDirection Direction { get; private set; }
        public int NumberOfPassengers { get; set; }
        public int MaxCapacity { get; }

        public bool IsMoving => Direction != ElevatorDirection.Idle;

        public Elevator(int id, int startingFloor, int maxCapacity)
        {
            Id = id;
            CurrentFloor = startingFloor;
            TargetFloor = startingFloor;
            Direction = ElevatorDirection.Idle;
            NumberOfPassengers = 0;
            MaxCapacity = maxCapacity;
        }

        public void SetTarget(int floor)
        {
            TargetFloor = floor;
            if (CurrentFloor < TargetFloor)
            {
                Direction = ElevatorDirection.Up;

            }
            else if (CurrentFloor > TargetFloor)
            {
                Direction = ElevatorDirection.Down;

            }
            else
            {
                Direction = ElevatorDirection.Idle;

            }
        }

        public void MoveOneStep()
        {
            if (CurrentFloor < TargetFloor)
            {
                CurrentFloor++;
                Direction = ElevatorDirection.Up;
            }
            else if (CurrentFloor > TargetFloor)
            {
                CurrentFloor--;
                Direction = ElevatorDirection.Down;
            }
            else
            {
                Direction = ElevatorDirection.Idle;
            }
        }

        public string GetStatus()
        {
            return $"Elevator {Id}: Floor {CurrentFloor}, Target {TargetFloor}, Direction {Direction}, Passengers {NumberOfPassengers}/{MaxCapacity}";
        }
    }

}
