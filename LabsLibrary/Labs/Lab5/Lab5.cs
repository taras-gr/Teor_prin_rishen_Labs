using Common.Core.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace LabsLibrary.Labs.Lab5
{
    class Lab5 : IRunnable
    {
        public void Run()
        {
            Console.WriteLine("\n***** Lab5 *****");
            MatrixGameSolver matrixGameSolver = new MatrixGameSolver();
            var payoffMatrix = GetArrayFromFile<int>("payoffMatrix");
            int n = payoffMatrix.GetLength(0);
            int m = payoffMatrix.GetLength(1);
            Console.WriteLine("\nThe payoff matrix is:");
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Console.Write(payoffMatrix[i, j] + " ");
                }
                Console.WriteLine();
            }

            var poorStrategyForFirst = matrixGameSolver.GetPoorStrategyForUser(payoffMatrix, User.First);
            var poorStrategyForSecond = matrixGameSolver.GetPoorStrategyForUser(payoffMatrix, User.Second);
            var saddlePoint = matrixGameSolver.TryObtainSaddlePoint(poorStrategyForFirst, poorStrategyForSecond);
            var probsForMixedStrategy = matrixGameSolver.GetProbabilitiesForMixedStrategy(payoffMatrix);

            Console.WriteLine($"\nThe poor strategy for first player is: {poorStrategyForFirst.Item1} at {poorStrategyForFirst.Item2}");
            Console.WriteLine($"The poor strategy for second player is: {poorStrategyForSecond.Item1} at {poorStrategyForSecond.Item2}");
            Console.WriteLine($"\nIs there a saddle point: {saddlePoint != null}");
            if (saddlePoint != null)
                Console.WriteLine($"Saddle point is: {saddlePoint}");

            Console.WriteLine("\nProbabilities for optimal mixed strategy for first user:");
            for (int i = 0; i < probsForMixedStrategy.firstPlayerProbs.Count; i++)
            {
                Console.WriteLine("P" + i + $": {probsForMixedStrategy.firstPlayerProbs[i]}");
            }

            Console.WriteLine("\nProbabilities for optimal mixed strategy for second user:");
            for (int i = 0; i < probsForMixedStrategy.secondPlayerProbs.Count; i++)
            {
                Console.WriteLine("P" + i + $": {probsForMixedStrategy.secondPlayerProbs[i]}");
            }
        }

        private T[,] GetArrayFromFile<T>(string arrayName)
        {
            var jsonTestDataFileName = "lab5Data.json";
            var absolutePathForDocs = Path.Combine(AppDomain.CurrentDomain.BaseDirectory
                + jsonTestDataFileName);

            var json = File.ReadAllText(absolutePathForDocs);
            var jObject = JObject.Parse(json);
            var values = jObject[arrayName]?.ToObject<T[,]>();

            return values != null ? values : throw new ArgumentNullException();
        }
    }
}