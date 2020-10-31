using Common.Core.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace LabsLibrary.Labs.Lab2
{
    class Lab2 : IRunnable
    {
        public void Run()
        {
            Console.WriteLine("\n***** Lab2 *****");

            var data = GetMatrixFromFile();
            var m1Res = GetIncomeFromOption(data[0].ToObject<Option>(), 5);
            var m2Res = GetIncomeFromOption(data[1].ToObject<Option>(), 5);
            var m3Res = GetResFromLastOption(data[2].ToObject<LastOption>(), data[0].ToObject<Option>(), 4);

            Console.WriteLine($"Income for fisrt option: {m1Res}");
            Console.WriteLine($"Income for second option: {m2Res}");
            Console.WriteLine($"Income for third option: {m3Res}");

            Console.WriteLine($"\nProfit for first option: {GetProfitFromOption(m1Res, data[0].ToObject<Option>().Cost)}");
            Console.WriteLine($"Profit for second option: {GetProfitFromOption(m2Res, data[1].ToObject<Option>().Cost)}");
            Console.WriteLine($"Profit for third option: {GetProfitFromOption(m3Res, data[0].ToObject<Option>().Cost)}");
        }
            
        private double GetIncomeFromOption(Option option, int yearsCount)
        {
            var res = option.D1 * option.P1 - option.D2 * option.P2;
            return res * yearsCount;
        }

        private double GetProfitFromOption(double income, double cost)
        {
            return income - cost;
        }

        private double GetResFromLastOption(LastOption option, Option option1, int yearsCount)
        {
            return option.P3 * (option.P1 * option1.D1 - option.P2 * option1.D2) * yearsCount;
        }

        private dynamic GetMatrixFromFile()
        {
            var jsonTestDataFileName = "lab2Data.json";
            var absolutePathForDocs = Path.Combine(AppDomain.CurrentDomain.BaseDirectory
                + jsonTestDataFileName);

            var json = File.ReadAllText(absolutePathForDocs);
            var jObject = JObject.Parse(json);
            var values = jObject["options"]?.ToObject<object[]>();

            return values != null ? values : throw new ArgumentNullException();
        }
    }

    internal class Option
    {
        public string Name { get; set; }
        public int Cost { get; set; }
        public int D1 { get; set; }
        public double P1 { get; set; }
        public int D2 { get; set; }
        public double P2 { get; set; }
    }

    internal class LastOption
    {
        public double P3 { get; set; }
        public double P4 { get; set; }
        public double P1 { get; set; }
        public double P2 { get; set; }
    }
}