using System;
using MyLog.Models;

namespace DataBase.Models
{
    public class ProductData
    {
        public string Id { get; set; }
        public string Color { get; set; }
        public int NumberOfSeats { get; set; }
        public int Task { get; set;}

        public ProductData()
        {
            Id = SetId();
            Color = SetColor();
            NumberOfSeats = SetNumberOfSeats();
            Task = SetTask();
        }

        private string SetId()
        {
            Console.Write($"Enter id: ");
            return Console.ReadLine();
        }

        private string SetColor()
        {
            Console.Write($"Enter color: ");
            return Console.ReadLine();
        }

        private int SetNumberOfSeats()
        {
            Console.Write($"Enter number of seats: ");
            int value;
            while (!int.TryParse(Console.ReadLine(), out value))
            {
                Console.Write($"Error, try again: ");
                Log.Error($"Error while entering value");
            }

            return value;
        }

        private int SetTask()
        {
            Console.Write($"Enter task: ");
            int value;
            while (!int.TryParse(Console.ReadLine(), out value))
            {
                Console.Write($"Error, try again: ");
                Log.Error($"Error while entering value");
            }
            
            return value;
        }
    }
}
