using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Elevator_Project.Service.IElevatorControl;

namespace Elevator_Project.Service
{
    public class ElevatorControlService : IElevatorService
    {
        private readonly IElevatorControl _control;
        private CancellationTokenSource? _cts;
        private Task? _simulationTask;
        private bool _running;

        public ElevatorControlService(IElevatorControl control)
        {
            _control = control;
        }

        public void Start()
        {
            if (_running)
            {
                return;

            }
               
            _running = true;
            _cts = new CancellationTokenSource();
            CancellationToken token = _cts.Token;

            _simulationTask = Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    _control.UpdateElevators();
                    Console.Clear();
                    _control.PrintElevatorStatus();
                    Thread.Sleep(1000);
                }
            }, token);
        }

        public void Stop()
        {
            if (!_running)
            {
                return;
            }

            _cts?.Cancel();
            _simulationTask?.Wait();
            _running = false;
        }
    }
}
