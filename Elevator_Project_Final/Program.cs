//// See https://aka.ms/new-console-template for more information
////Console.WriteLine("Hello, World!");
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Elevator_Project.Controller;
using Elevator_Project.Models;
using Elevator_Project.Service;

namespace Elevator_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            //var elevators = new List<Elevator>
            //    {
            //        new Elevator(1, 1, 10),
            //        new Elevator(2, 1, 10),
            //        new Elevator(3, 1, 10)
            //    };

            //var controller = new ElevatorControl(elevators);

            int numberOfFloors = 10;
            List<IElevator> elevators = new List<IElevator>
            {
                new Elevator(1, 1, 10),
                new Elevator(2, 1, 10),
                new Elevator(3, 1, 10)
            };

            IElevatorControl control = new ElevatorControl(elevators);

            bool running = true;
            Thread simulationThread = new Thread(() =>
            {
                while (running)
                {
                    control.UpdateElevators();
                    Thread.Sleep(1000);
                }
            });
            simulationThread.Start();

            Console.WriteLine("Elevator Simulation Started.");
            Console.WriteLine("Commands: call <floor> <passengers>  OR  exit");

            while (true)
            {
                Console.WriteLine("\nCurrent Elevator Status:");
                control.PrintElevatorStatus();
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
                        var assigned = control.DispatchElevator(request);

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
}