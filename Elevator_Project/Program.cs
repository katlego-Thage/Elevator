// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

enum ElevatorDirection
{
    Up,
    Down,
    Idle
}

class Elevator
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

class ElevatorRequest
{
    public int Floor { get; }
    public int Passengers { get; }

    public ElevatorRequest(int floor, int passengers)
    {
        Floor = floor;
        Passengers = passengers;
    }
}

class ElevatorController
{
    public List<Elevator> Elevators { get; }

    public ElevatorController(List<Elevator> elevators)
    {
        Elevators = elevators;
    }

    public Elevator? DispatchElevator(ElevatorRequest request)
    {
        var availableElevators = Elevators
            .Where(e => e.Direction == ElevatorDirection.Idle && e.NumberOfPassengers < e.MaxCapacity)
            .ToList();

        if (!availableElevators.Any())
        {
            return null;
        }

        var selected = availableElevators.OrderBy(e => Math.Abs(e.CurrentFloor - request.Floor)).First();

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

class Program
{
    static void Main(string[] args)
    {
        int numberOfFloors = 10;
        var elevators = new List<Elevator>
        {
            new Elevator(1, 1, 10),
            new Elevator(2, 1, 10),
            new Elevator(3, 1, 10)
        };

        var controller = new ElevatorController(elevators);

        bool running = true;
        Thread simulationThread = new Thread(() =>
        {
            while (running)
            {
                controller.UpdateElevators();
                Thread.Sleep(1000);
            }
        });
        simulationThread.Start();

        Console.WriteLine("Elevator Simulation Started.");
        Console.WriteLine("Commands: call <floor> <passengers>  OR  exit");

        while (true)
        {
            Console.WriteLine("\nCurrent Elevator Status:");
            controller.PrintElevatorStatus();
            Console.Write("\nEnter command: ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            if (input.Trim().ToLower() == "exit")
            {
                running = false;
                break;
            }

            // NB  when executing the program, enter the values as follows e.g ---> "call 5 3" which will be the number of passenger and the floor number
            var parts = input.Split(' ');
            if (parts.Length == 3 && parts[0].ToLower() == "call")
            {
                if (int.TryParse(parts[1], out int floor) && int.TryParse(parts[2], out int passengers))
                {
                    if (floor < 1 || floor > numberOfFloors)
                    {
                        Console.WriteLine("Invalid floor. Floors range from 1 to " + numberOfFloors);
                        continue;
                    }

                    ElevatorRequest request = new ElevatorRequest(floor, passengers);
                    var assigned = controller.DispatchElevator(request);

                    if (assigned != null)
                    {
                        Console.WriteLine($"Elevator {assigned.Id} is dispatched to floor {floor}.");
                    }
                    else
                    {
                        Console.WriteLine("No available elevator at the moment.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid command format. Use: call <floor> <passengers>");
                }
            }
            else
            {
                Console.WriteLine("Invalid command. Use: call <floor> <passengers> or exit.");
            }
        }

        simulationThread.Join();
        Console.WriteLine("Simulation ended.");
    }
}

