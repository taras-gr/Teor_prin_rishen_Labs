using System;
using System.Collections.Generic;
using System.Linq;

namespace LabsLibrary.Labs.Lab5
{
    enum User
    {
        First,
        Second
    }
    class MatrixGameSolver
    {
        private LpSolver _lpSolver;

        public MatrixGameSolver()
        {
            _lpSolver = new LpSolver();
        }
                        
        public (int, int) GetPoorStrategyForUser(int[,] payoffMatrix, User user)
        {
            var strategy = user == User.First ? GetMaxMinOfTheRows(payoffMatrix) : GetMinMaxOfTheColumns(payoffMatrix);

            return strategy;
        }

        public int? TryObtainSaddlePoint((int, int) poorStrategyForFirst, (int, int) poorStrategyForSecond)
        {
            int saddlePoint;

            if (poorStrategyForFirst == poorStrategyForSecond)
            {
                saddlePoint = poorStrategyForFirst.Item2;
                return saddlePoint;
            }

            return null;
        }

        public (List<double> firstPlayerProbs, List<double> secondPlayerProbs) GetProbabilitiesForMixedStrategy(
            int[,] payoffMatrix)
        {
            var xStar = SolveTaskForUser(payoffMatrix, User.First);
            var fStar = GetSumFromResultsDictionary(xStar);

            var yStar = SolveTaskForUser(payoffMatrix, User.Second);

            var gamePrice = GetGamePrice(fStar);
            Console.WriteLine($"\nGame price is: {gamePrice}");

            var firstPlayerOptimalStrategy = GetOptimalMixedStrategyForUser(gamePrice, yStar);
            var secondPlayerOptimalStrategy = GetOptimalMixedStrategyForUser(gamePrice, xStar);

            return (firstPlayerOptimalStrategy, secondPlayerOptimalStrategy);
        }

        private (int minFromMaxes, int j) GetMinMaxOfTheColumns(int[,] payoffMatrix)
        {
            List<int> maxesForEachColumn = new List<int>();

            var lengthOfFirstDimesion = payoffMatrix.GetLength(0);
            var lengthOfSecondDimesion = payoffMatrix.GetLength(1);

            for (int i = 0; i < lengthOfSecondDimesion; i++)
            {
                maxesForEachColumn.Add(payoffMatrix[0, i]);

                for (int j = 1; j < lengthOfFirstDimesion; j++)
                {
                    if (payoffMatrix[j, i] > maxesForEachColumn[i])
                        maxesForEachColumn[i] = payoffMatrix[j, i];
                }
            }

            var minFromMaxesForEachColumn = maxesForEachColumn.Min();
            var indexOfMin = maxesForEachColumn.IndexOf(minFromMaxesForEachColumn);

            return (minFromMaxesForEachColumn, indexOfMin);
        }

        private (int maxFromMins, int i) GetMaxMinOfTheRows(int[,] payoffMatrix)
        {
            List<int> minsForEachRow = new List<int>();

            var lengthOfFirstDimesion = payoffMatrix.GetLength(0);
            var lengthOfSecondDimesion = payoffMatrix.GetLength(1);

            for (int i = 0; i < lengthOfFirstDimesion; i++)
            {
                minsForEachRow.Add(payoffMatrix[i, 0]);

                for (int j = 1; j < lengthOfSecondDimesion; j++)
                {
                    if (payoffMatrix[i, j] < minsForEachRow[i])
                        minsForEachRow[i] = payoffMatrix[i, j];
                }
            }

            var maxFromMinsForEachRow = minsForEachRow.Max();
            var indexOfMax = minsForEachRow.IndexOf(maxFromMinsForEachRow);

            return (maxFromMinsForEachRow, indexOfMax);
        }
           
        private double[] GetOnesMatrix(int n)
        {
            var matrix = new double[n];
            for (int i = 0; i < n; i++)
            {
                matrix[i] = 1.0;
            }

            return matrix;
        }

        private Dictionary<string, double> SolveTaskForUser(int[,] payoffMatrix, User user)
        {
            int n = payoffMatrix.GetLength(0);
            int m = payoffMatrix.GetLength(1);

            var b = GetOnesMatrix(n);

            double[,] a = new double[n, m];

            bool isFirstUser = user == User.First;

            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    a[i, j] = isFirstUser ? payoffMatrix[i, j] : payoffMatrix[j, i];
                }
            }

            var results = _lpSolver.SolveBySimplexMethod(a, b, b);

            return results;
        }

        private double GetSumFromResultsDictionary(Dictionary<string, double> resultsDictionary)
        {
            var resultsSum = resultsDictionary.Sum(s => s.Value);

            return resultsSum;
        }

        private double GetGamePrice(double fStar)
        {
            return 1.0 / fStar;
        }

        private List<double> GetOptimalMixedStrategyForUser(double gamePrice, Dictionary<string, double> values)
        {
            var resultsList = new List<double>();

            foreach (var item in values)
            {
                resultsList.Add(gamePrice * item.Value);
            }

            return resultsList;
        }
    }
}