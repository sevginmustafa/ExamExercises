using OnlineShop.Models.Products.Components;
using OnlineShop.Models.Products.Computers;
using OnlineShop.Models.Products.Peripherals;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineShop.Core
{
    public class Controller : IController
    {
        private List<IComputer> computers = new List<IComputer>();
        private List<IComponent> components = new List<IComponent>();
        private List<IPeripheral> peripherals = new List<IPeripheral>();

        public string AddComponent(int computerId, int id, string componentType, string manufacturer, string model, decimal price, double overallPerformance, int generation)
        {
            if (!computers.Any(x => x.Id == computerId))
            {
                throw new ArgumentException("Computer with this id does not exist.");
            }

            IComputer computer = computers.FirstOrDefault(x => x.Id == computerId);

            if (components.Any(x => x.Id == id))
            {
                throw new ArgumentException("Component with this id already exists.");
            }

            IComponent component = null;

            if (componentType == "CentralProcessingUnit")
            {
                component = new CentralProcessingUnit(id, manufacturer, model, price, overallPerformance, generation);
            }
            else if (componentType == "Motherboard")
            {
                component = new Motherboard(id, manufacturer, model, price, overallPerformance, generation);
            }
            else if (componentType == "PowerSupply")
            {
                component = new PowerSupply(id, manufacturer, model, price, overallPerformance, generation);
            }
            else if (componentType == "RandomAccessMemory")
            {
                component = new RandomAccessMemory(id, manufacturer, model, price, overallPerformance, generation);
            }
            else if (componentType == "SolidStateDrive")
            {
                component = new SolidStateDrive(id, manufacturer, model, price, overallPerformance, generation);
            }
            else if (componentType == "VideoCard")
            {
                component = new VideoCard(id, manufacturer, model, price, overallPerformance, generation);
            }
            else
            {
                throw new ArgumentException("Component type is invalid.");
            }

            computer.AddComponent(component);
            components.Add(component);

            return $"Component {componentType} with id {id} added successfully in computer with id {computerId}.";
        }

        public string AddComputer(string computerType, int id, string manufacturer, string model, decimal price)
        {
            if (computers.Any(x => x.Id == id))
            {
                throw new ArgumentException("Computer with this id already exists.");
            }

            IComputer computer = null;

            if (computerType == "Laptop")
            {
                computer = new Laptop(id, manufacturer, model, price);
            }
            else if (computerType == "DesktopComputer")
            {
                computer = new DesktopComputer(id, manufacturer, model, price);
            }
            else
            {
                throw new ArgumentException("Computer type is invalid.");
            }

            computers.Add(computer);

            return $"Computer with id {id} added successfully.";
        }

        public string AddPeripheral(int computerId, int id, string peripheralType, string manufacturer, string model, decimal price, double overallPerformance, string connectionType)
        {
            if (!computers.Any(x => x.Id == computerId))
            {
                throw new ArgumentException("Computer with this id does not exist.");
            }

            IComputer computer = computers.FirstOrDefault(x => x.Id == computerId);

            if (peripherals.Any(x => x.Id == id))
            {
                throw new ArgumentException("Peripheral with this id already exists.");
            }

            IPeripheral peripheral = null;

            if (peripheralType == "Headset")
            {
                peripheral = new Headset(id, manufacturer, model, price, overallPerformance, connectionType);
            }
            else if (peripheralType == "Keyboard")
            {
                peripheral = new Keyboard(id, manufacturer, model, price, overallPerformance, connectionType);
            }
            else if (peripheralType == "Monitor")
            {
                peripheral = new Monitor(id, manufacturer, model, price, overallPerformance, connectionType);
            }
            else if (peripheralType == "Mouse")
            {
                peripheral = new Mouse(id, manufacturer, model, price, overallPerformance, connectionType);
            }
            else
            {
                throw new ArgumentException("Peripheral type is invalid.");
            }

            computer.AddPeripheral(peripheral);
            peripherals.Add(peripheral);

            return $"Peripheral {peripheralType} with id {id} added successfully in computer with id {computerId}.";
        }

        public string BuyBest(decimal budget)
        {
            IComputer computer = computers.OrderByDescending(x => ((Computer)x).AverageOverallPerformance).FirstOrDefault(x => ((Computer)x).TotalPrice <= budget);

            if (computers.Count == 0 || computer == null)
            {
                throw new ArgumentException($"Can't buy a computer with a budget of ${budget}.");
            }

            computers.Remove(computer);

            return computer.ToString();
        }

        public string BuyComputer(int id)
        {
            if (!computers.Any(x => x.Id == id))
            {
                throw new ArgumentException("Computer with this id does not exist.");
            }

            IComputer computer = computers.FirstOrDefault(x => x.Id == id);

            computers.Remove(computer);

            return computer.ToString();
        }

        public string GetComputerData(int id)
        {
            if (!computers.Any(x => x.Id == id))
            {
                throw new ArgumentException("Computer with this id does not exist.");
            }

            IComputer computer = computers.FirstOrDefault(x => x.Id == id);

            return computer.ToString();
        }

        public string RemoveComponent(string componentType, int computerId)
        {
            if (!computers.Any(x => x.Id == computerId))
            {
                throw new ArgumentException("Computer with this id does not exist.");
            }

            IComputer computer = computers.FirstOrDefault(x => x.Id == computerId);

            IComponent toRemove = computer.RemoveComponent(componentType);

            components.Remove(toRemove);

            return $"Successfully removed {componentType} with id {toRemove.Id}.";
        }

        public string RemovePeripheral(string peripheralType, int computerId)
        {
            if (!computers.Any(x => x.Id == computerId))
            {
                throw new ArgumentException("Computer with this id does not exist.");
            }

            IComputer computer = computers.FirstOrDefault(x => x.Id == computerId);

            IPeripheral toRemove = computer.RemovePeripheral(peripheralType);

            peripherals.Remove(toRemove);

            return $"Successfully removed {peripheralType} with id {toRemove.Id}.";
        }
    }
}
