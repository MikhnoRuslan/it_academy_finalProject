using System;
using CRUD.Interfaces;
using DataBase.Models;
using DataBase.Models.Repositories;
using MyLog.Models;

namespace CRUD.Models
{
    public class TaskCrud : ICrud
    {
        private static readonly ProductRepositories DatabaseProduct = new ProductRepositories();
        public void Create()
        {
            ConsoleKeyInfo key;
            do
            {
                var product = new ProductData();
                Log.Info($"Creation data.");
                DatabaseProduct.Create(product);
                Console.WriteLine($"Do you want to create more (Y/N)?");
                key = Console.ReadKey();
                Console.WriteLine();
            } while (key.Key != ConsoleKey.N);
        }

        public void Delete()
        {
            ConsoleKeyInfo key;
            do
            {
                Console.WriteLine($"Enter id to delete");
                var id = Console.ReadLine();
                Log.Info($"Data deletion.");
                DatabaseProduct.Delete(id);
                Console.WriteLine($"Do you want to delete more (Y/N)?");
                key = Console.ReadKey();
                Console.WriteLine();
            } while (key.Key != ConsoleKey.N);
        }

        public void GetDataByTask()
        {
            ConsoleKeyInfo key;
            do
            {
                Console.WriteLine($"Enter task:");
                var item = Convert.ToInt32(Console.ReadLine());
                Tuple<string, string, int, int>[] dataByTask = DatabaseProduct.GetDataByTask(item);
                foreach (var i in dataByTask)
                    Console.WriteLine($"{i.Item1}\t{i.Item2}\t{i.Item3}\t{i.Item4}");

                Console.WriteLine($"\nDo you want to see more (Y/N)?");
                key = Console.ReadKey();
                Console.WriteLine();
            } while (key.Key != ConsoleKey.N);
        }

        public void GetAllData()
        {
            DatabaseProduct.GetAllData();
        }
    }
}
