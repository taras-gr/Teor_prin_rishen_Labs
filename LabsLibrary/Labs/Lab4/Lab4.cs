using System;
using System.IO;
using Common.Core.Interfaces;
using Newtonsoft.Json.Linq;

namespace LabsLibrary.Labs.Lab4
{
    public class Lab4 : IRunnable
    {
        public void Run()
        {
            Console.WriteLine("\n***** Lab4 *****");
            var parameters = GetArrayFromFile<string>("parameters");
            var weights = GetArrayFromFile<double>("weights");
            var items = GetArrayFromFile<string>("items");
            var scores = GetArrayFromFile<int[]>("scores");

            Expertise expertise = new Expertise();

            var result = expertise.GetOptimalItemByExpertsRatesMethod(items, weights, scores);

            Console.WriteLine("\nThe input table is:");
            Console.Write(string.Format("\n{0,-20} | {1,-8}", "Parameter", "Weight"));
            for (int i = 0; i < items.Length; i++)
            {
                Console.Write(string.Format("| {0,-11} ", items[i]));
            }

            Console.WriteLine("\n-----------------------------------------------------------------------------------------------------------");

            for (int i = 0; i < parameters.Length; i++)
            {
                Console.Write(string.Format("{0,-20} | {1,-8}", parameters[i], weights[i]));
                for (int j = 0; j < scores[i].Length; j++)
                {
                    Console.Write(string.Format("| {0,-12}", scores[i][j]));
                }
                Console.WriteLine("\n-----------------------------------------------------------------------------------------------------------");
            }

            Console.WriteLine($"\nThe optimal value by experts rates method is: {result}");
        }

        private T[] GetArrayFromFile<T>(string arrayName)
        {
            var jsonTestDataFileName = "lab4Data.json";
            var absolutePathForDocs = Path.Combine(AppDomain.CurrentDomain.BaseDirectory
                + jsonTestDataFileName);

            var json = File.ReadAllText(absolutePathForDocs);
            var jObject = JObject.Parse(json);
            var values = jObject[arrayName]?.ToObject<T[]>();

            return values != null ? values : throw new ArgumentNullException();
        }
    }
}