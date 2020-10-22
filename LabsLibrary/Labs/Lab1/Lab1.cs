using Common.Core.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LabsLibrary.Labs.Lab1
{
    class Lab1 : IRunnable
    {
        public void Run()
        {
            Console.WriteLine("\n***** Lab1 *****");

            var data = GetMatrixFromFile();
            var resultByWaldsCrtr = WaldsCriterion(data);
            var resultByLaplaceCrtr = LaplaceCriterion(data);

            var positiveRealPart = GetPositiveRealPartFromUser();
            var resultByHurwitzCrtr = HurwitzCriterion(data, positiveRealPart);

            var probabilities = GetProbabilitiesFromUser(data.Length);
            var resultByBayesLaplaceCrtr = BayesLaplaceCriterion(data, probabilities);

            Console.WriteLine("\nThe data is: ");
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    Console.Write(data[i][j] + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine($"\nResult by Walds criterion: \t\tA{ResultsAsList(resultByWaldsCrtr)}");
            Console.WriteLine($"Result by Laplace criterion: \t\tA{ResultsAsList(resultByLaplaceCrtr)}");
            Console.WriteLine($"Result by Hurwitz criterion: \t\tA{ResultsAsList(resultByHurwitzCrtr)}");
            Console.WriteLine($"Result by Bayes-Laplace criterion: \tA{ResultsAsList(resultByBayesLaplaceCrtr)}");
        }

        private List<int> WaldsCriterion(int[][] data)
        {
            List<int> minsForEachRow = new List<int>();

            foreach (int[] row in data)
            {
                var min = row.Min();
                minsForEachRow.Add(min);
            }

            var max = minsForEachRow.Max();
            var indexesOfMax = minsForEachRow.IndexesOf(max);

            return indexesOfMax;
        }

        private List<int> LaplaceCriterion(int[][] data)
        {
            List<double> averageForEachRow = new List<double>();

            foreach (int[] row in data)
            {
                var min = row.Average();
                averageForEachRow.Add(min);
            }

            var max = averageForEachRow.Max();
            var indexesOfMax = averageForEachRow.IndexesOf(max);

            return indexesOfMax;
        }

        private List<int> HurwitzCriterion(int[][] data, 
            double positiveRealPart)
        {
            List<double> tempResults = new List<double>();

            foreach (var row in data)
            {
                var rowValue =
                    row.Max() * positiveRealPart + row.Min() * positiveRealPart;

                tempResults.Add(rowValue);
            }

            var max = tempResults.Max();
            var indexesOfmax = tempResults.IndexesOf(max);

            return indexesOfmax;
        }

        private List<int> BayesLaplaceCriterion(int[][] data,
            List<double> probabilities)
        {
            List<double> tempResults = new List<double>();

            for (int i = 0; i < data.Length; i++)
            {
                double rowResult = 0;

                for (int j = 0; j < probabilities.Count; j++)
                {
                    rowResult += data[i][j] * probabilities[j];
                }

                tempResults.Add(rowResult);
            }

            var max = tempResults.Max();
            var indexesOfmax = tempResults.IndexesOf(max);

            return indexesOfmax;
        }

        private int[][] GetMatrixFromFile()
        {
            var jsonTestDataFileName = "lab1Data.json";
            var absolutePathForDocs = Path.Combine(AppDomain.CurrentDomain.BaseDirectory
                + jsonTestDataFileName);

            var json = File.ReadAllText(absolutePathForDocs);
            var jObject = JObject.Parse(json);
            var values = jObject["matrix"]?.ToObject<int[][]>();

            return values != null ? values : throw new ArgumentNullException();
        }

        private List<double> GetProbabilitiesFromUser(int count)
        {
            List<double> probabilities = new List<double>();

            Console.WriteLine($"Input {count} probabilities for Bayes-Laplace criterion: ");

            for (int i = 0; i < count; i++)
            {
                double.TryParse(Console.ReadLine(), out var prob);
                probabilities.Add(prob);
            }

            return probabilities;
        }

        private double GetPositiveRealPartFromUser()
        {
            Console.Write("\nInput positive real part for Hurwitz criterion: ");
            double.TryParse(Console.ReadLine(), out var pos);
            return pos;
        }

        private string ResultsAsList(List<int> results)
        {
            string resultsAsString = "";

            for (int i = 0; i < results.Count; i++)
            {
                resultsAsString += ++results[i] + ",";
            }

            return resultsAsString;
        }
    }
}