using Common.Core.Interfaces;
using LabsLoader.Factories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LabsLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            List<IRunnable> labs = LabFactory.GetLabs().ToList();

            if (labs.Count == 0)
            {
                Console.WriteLine("There aren't available labs");
            }

            else
            {
                Console.WriteLine("Available labs: ");
                foreach (var lab in labs)
                {
                    Console.WriteLine(lab.GetType().Name);
                }


                int labToRunNumber;

                while (true)
                {
                    Console.Write("\nEnter the lab number you want to run (0 to exit): ");
                    labToRunNumber = int.Parse(Console.ReadLine());
                    if (labToRunNumber == 0)
                        break;

                    labs[labToRunNumber - 1].Run();
                }
            }
        }
    }
}