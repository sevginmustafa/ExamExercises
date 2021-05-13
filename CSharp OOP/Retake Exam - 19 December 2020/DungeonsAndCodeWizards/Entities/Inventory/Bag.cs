using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WarCroft.Constants;
using WarCroft.Entities.Items;

namespace WarCroft.Entities.Inventory
{
    public abstract class Bag : IBag
    {
        protected Bag(int capacity)
        {
            Capacity = capacity;
            Items = new List<Item>();
        }

        public int Capacity { get; set; } = 100;

        public int Load => Items.Sum(x => x.Weight);

        public IReadOnlyCollection<Item> Items { get; }

        public void AddItem(Item item)
        {
            if (item.Weight + Load > Capacity)
            {
                throw new InvalidOperationException(ExceptionMessages.ExceedMaximumBagCapacity);
            }

            ((IList)Items).Add(item);
        }

        public Item GetItem(string name)
        {
            if (Items.Count == 0)
            {
                throw new InvalidOperationException(ExceptionMessages.EmptyBag);
            }

            Item item = Items.FirstOrDefault(x => x.GetType().Name == name);

            if (item == null)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.ItemNotFoundInBag, name));
            }

            ((IList)Items).Remove(item);

            return item;
        }
    }
}
