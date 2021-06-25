using OnlineShop.Models.Products.Components;
using OnlineShop.Models.Products.Peripherals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineShop.Models.Products.Computers
{
    public abstract class Computer : Product, IComputer
    {
        public decimal TotalPrice => CalculatePrice();
        public double AverageOverallPerformance => CalculateOverallPerformance();

        protected Computer(int id, string manufacturer, string model, decimal price, double overallPerformance)
       : base(id, manufacturer, model, price, overallPerformance)
        {
            Components = new List<IComponent>();
            Peripherals = new List<IPeripheral>();
        }

        protected Computer(int id, string manufacturer, string model, decimal price)
     : base(id, manufacturer, model, price)
        {
            Components = new List<IComponent>();
            Peripherals = new List<IPeripheral>();
        }

        public IReadOnlyCollection<IComponent> Components { get; private set; }

        public IReadOnlyCollection<IPeripheral> Peripherals { get; private set; }

        public void AddComponent(IComponent component)
        {
            if (Components.Any(x => x.GetType() == component.GetType()))
            {
                throw new ArgumentException($"Component {component.GetType().Name} already exists in {GetType().Name} with Id {Id}.");
            }

           ((ICollection<IComponent>)Components).Add(component);
        }

        public void AddPeripheral(IPeripheral peripheral)
        {
            if (Peripherals.Any(x => x.GetType() == peripheral.GetType()))
            {
                throw new ArgumentException($"Peripheral {peripheral.GetType().Name} already exists in {GetType().Name} with Id {Id}.");
            }

           ((ICollection<IPeripheral>)Peripherals).Add(peripheral);
        }

        public IComponent RemoveComponent(string componentType)
        {
            IComponent toRemove = Components.FirstOrDefault(x => x.GetType().Name == componentType);

            if (Components.Count == 0 || toRemove == null)
            {
                throw new ArgumentException($"Component {componentType} does not exist in {GetType().Name} with Id {Id}.");
            }

            ((ICollection<IComponent>)Components).Remove(toRemove);

            return toRemove;
        }

        public IPeripheral RemovePeripheral(string peripheralType)
        {
            IPeripheral toRemove = Peripherals.FirstOrDefault(x => x.GetType().Name == peripheralType);

            if (Peripherals.Count == 0 || toRemove == null)
            {
                throw new ArgumentException($"Peripheral {peripheralType} does not exist in {GetType().Name} with Id {Id}.");
            }

            ((ICollection<IPeripheral>)Peripherals).Remove(toRemove);

            return toRemove;
        }

        public decimal CalculatePrice()
        {
            return Price + Components.Sum(x => x.Price) + Peripherals.Sum(x => x.Price);
        }

        public double CalculateOverallPerformance()
        {
            double averageOverallPerformance = OverallPerformance;

            if (Components.Count != 0)
            {
                averageOverallPerformance += Components.Average(x => x.OverallPerformance);
            }

            return averageOverallPerformance;
        }

        public override string ToString()
        {
            double average = 0;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Overall Performance: {AverageOverallPerformance:f2}. Price: {TotalPrice:f2} - {GetType().Name}: {Manufacturer} {Model} (Id: {Id})");
            sb.AppendLine($" Components ({Components.Count}):");

            foreach (var component in Components)
            {
                sb.AppendLine($"  {component}");
            }

            if (Peripherals.Count != 0)
            {
                average = Peripherals.Average(x => x.OverallPerformance);
            }

            sb.AppendLine($" Peripherals ({Peripherals.Count}); Average Overall Performance ({average:f2}):");

            foreach (var peripheral in Peripherals)
            {
                sb.AppendLine($"  {peripheral}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
