using System;
using System.Linq;

namespace LabsLibrary.Labs.Lab4
{
    public class Expertise
    {
        public string GetOptimalItemByExpertsRatesMethod(
            string[] items, double[] weights, int[][] scores)
        {
            double[] maxOfEachColumn = new double[items.Length];

            for (int i = 0; i < scores[0].GetLength(0); i++)
            {
                for (int j = 0; j < scores.Length; j++)
                {
                    maxOfEachColumn[i] += scores[i][j] * weights[j];
                }
            }

            var maxValue = maxOfEachColumn.Max();
            int indexOfMax = Array.IndexOf(maxOfEachColumn, maxValue);

            var optimalItem = items[indexOfMax];

            return optimalItem;
        }
    }
}