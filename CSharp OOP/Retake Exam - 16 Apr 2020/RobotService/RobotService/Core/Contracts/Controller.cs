using RobotService.Models.Garages;
using RobotService.Models.Garages.Contracts;
using RobotService.Models.Procedures;
using RobotService.Models.Procedures.Contracts;
using RobotService.Models.Robots;
using RobotService.Models.Robots.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotService.Core.Contracts
{
    public class Controller : IController
    {
        private IGarage Garage = new Garage();
        private List<IProcedure> Procedures = new List<IProcedure>();

        public string Charge(string robotName, int procedureTime)
        {
            CheckIfRobotExist(robotName);

            if (Procedures.FirstOrDefault(x => x.GetType().Name == "Charge") == null)
            {
                Procedures.Add(new Charge());
            }

            Procedures.FirstOrDefault(x => x.GetType().Name == "Charge").DoService(Garage.Robots[robotName], procedureTime);

            return $"{robotName} had charge procedure";
        }

        public string Chip(string robotName, int procedureTime)
        {
            CheckIfRobotExist(robotName);

            if (Procedures.FirstOrDefault(x => x.GetType().Name == "Chip") == null)
            {
                Procedures.Add(new Chip());
            }

            Procedures.FirstOrDefault(x => x.GetType().Name == "Chip").DoService(Garage.Robots[robotName], procedureTime);

            return $"{robotName} had chip procedure";
        }

        public string History(string procedureType)
        {
            IProcedure procedure = Procedures.FirstOrDefault(x => x.GetType().Name == procedureType);

            return procedure.History();
        }

        public string Manufacture(string robotType, string name, int energy, int happiness, int procedureTime)
        {
            IRobot robot = null;

            if (robotType == "HouseholdRobot")
            {
                robot = new HouseholdRobot(name, energy, happiness, procedureTime);
            }
            else if (robotType == "PetRobot")
            {
                robot = new PetRobot(name, energy, happiness, procedureTime);
            }
            else if (robotType == "WalkerRobot")
            {
                robot = new WalkerRobot(name, energy, happiness, procedureTime);
            }
            else
            {
                throw new ArgumentException($"{robotType} type doesn't exist");
            }

            Garage.Manufacture(robot);

            return $"Robot {name} registered successfully";
        }

        public string Polish(string robotName, int procedureTime)
        {
            CheckIfRobotExist(robotName);

            if (Procedures.FirstOrDefault(x => x.GetType().Name == "Polish") == null)
            {
                Procedures.Add(new Polish());
            }

            Procedures.FirstOrDefault(x => x.GetType().Name == "Polish").DoService(Garage.Robots[robotName], procedureTime);

            return $"{robotName} had polish procedure";
        }

        public string Rest(string robotName, int procedureTime)
        {
            CheckIfRobotExist(robotName);

            if (Procedures.FirstOrDefault(x => x.GetType().Name == "Rest") == null)
            {
                Procedures.Add(new Rest());
            }

            Procedures.FirstOrDefault(x => x.GetType().Name == "Rest").DoService(Garage.Robots[robotName], procedureTime);

            return $"{robotName} had rest procedure";
        }

        public string Sell(string robotName, string ownerName)
        {
            CheckIfRobotExist(robotName);

            IRobot robot = Garage.Robots.Values.FirstOrDefault(x => x.Name == robotName);

            Garage.Sell(robotName, ownerName);

            if (robot.IsChipped)
            {
                return $"{ownerName} bought robot with chip";
            }

            return $"{ownerName} bought robot without chip";
        }

        public string TechCheck(string robotName, int procedureTime)
        {
            CheckIfRobotExist(robotName);

            if (Procedures.FirstOrDefault(x => x.GetType().Name == "TechCheck") == null)
            {
                Procedures.Add(new TechCheck());
            }

            Procedures.FirstOrDefault(x => x.GetType().Name == "TechCheck").DoService(Garage.Robots[robotName], procedureTime);

            return $"{robotName} had tech check procedure";
        }

        public string Work(string robotName, int procedureTime)
        {
            CheckIfRobotExist(robotName);

            if (Procedures.FirstOrDefault(x => x.GetType().Name == "Work") == null)
            {
                Procedures.Add(new Work());
            }

            Procedures.FirstOrDefault(x => x.GetType().Name == "Work").DoService(Garage.Robots[robotName], procedureTime);

            return $"{robotName} was working for {procedureTime} hours";
        }

        private bool CheckIfRobotExist(string robotName)
        {
            if (!Garage.Robots.ContainsKey(robotName))
            {
                throw new ArgumentException($"Robot {robotName} does not exist");
            }

            return true;
        }
    }
}
