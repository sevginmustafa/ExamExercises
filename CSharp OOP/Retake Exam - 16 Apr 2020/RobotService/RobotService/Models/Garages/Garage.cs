using RobotService.Models.Garages.Contracts;
using RobotService.Models.Robots.Contracts;
using System;
using System.Collections.Generic;

namespace RobotService.Models.Garages
{
    public class Garage : IGarage
    {
        private const int CAPACITY = 10;

        public IReadOnlyDictionary<string, IRobot> Robots { get; }

        public Garage()
        {
            Robots = new Dictionary<string, IRobot>();
        }

        public void Manufacture(IRobot robot)
        {
            if (Robots.Count == CAPACITY)
            {
                throw new InvalidOperationException("Not enough capacity");
            }

            if (Robots.ContainsKey(robot.Name))
            {
                throw new ArgumentException($"Robot {robot.Name} already exist");
            }

            ((Dictionary<string, IRobot>)Robots).Add(robot.Name, robot);
        }

        public void Sell(string robotName, string ownerName)
        {
            if (!Robots.ContainsKey(robotName))
            {
                throw new ArgumentException($"Robot {robotName} does not exist");
            }

            Robots[robotName].Owner = ownerName;
            Robots[robotName].IsBought = true;
            ((Dictionary<string, IRobot>)Robots).Remove(robotName);
        }
    }
}
