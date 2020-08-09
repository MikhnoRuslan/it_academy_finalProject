using System;
using MyLog.Models;

namespace CheckList.Models
{
    class Package
    {
        public ushort Length { get; set; }
        public ushort Width { get; set; }
        public ushort Height { get; set; }
        public float Weight { get; set; }

        public Package()
        {
            Length = SetFeature(Constants.Length);
            Width = SetFeature(Constants.Width);
            Height = SetFeature(Constants.Height);
            Weight = SetWeight();
        }

        private ushort SetFeature(string feature)
        {
            Console.Write($"\tEnter {feature}: ");
            ushort value;
            while (!ushort.TryParse(Console.ReadLine(), out value))
            {
                Console.WriteLine(Constants.IncorrectData);
                Log.Error($"Error while entering value");
            }

            return value;
        }

        private float SetWeight()
        {
            Console.Write($"\tEnter weight: ");
            float value;
            while (!float.TryParse(Console.ReadLine(), out value))
            {
                Console.WriteLine(Constants.IncorrectData);
                Log.Error($"Error while entering value");
            }

            return value;
        }
    }
}
