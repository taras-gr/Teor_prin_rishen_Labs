using Common.Core.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace LabsLibrary.Labs.Lab3
{
    class Lab3 : IRunnable
    {
        public void Run()
        {
            Console.WriteLine("\n***** Lab3 *****");

            var superiorities = GetMatrixFromFile<string>("superiorities");
            var votersCounts = GetArrayFromFile("voterCount");

            Election election = new Election();

            var candidates = election.GetListOfCandidatesFromRow(superiorities[0]);

            var electionMatrix = election.GetElectionMatrix(superiorities, votersCounts);
            int m = electionMatrix.GetLength(0);

            var winnerByCondorcetMethod =
                election.GetWinnerFromElectionMatrixByCondorcet(electionMatrix);
            var winnerByBordaMethod =
                election.GetWinnerFromElectionMatrixByBorda(electionMatrix);

            Console.WriteLine($"\nThere are {candidates.Count} candidates: ");
            foreach (var candidate in candidates)
            {
                Console.Write(candidate + ", ");
            }

            Console.WriteLine($"\n\nThe superiorities matrix with voters count: ");
            for (int i = 0; i < superiorities.Length; i++)
            {
                Console.Write(votersCounts[i] + " \t");
                for (int j = 0; j < superiorities[0].Length; j++)
                {
                    Console.Write(superiorities[i][j] + $"{((j != superiorities[0].Length - 1) ? " -> "  : string.Empty)}");
                }
                Console.WriteLine();
            }         

            Console.WriteLine("\nThe election matrix is:");
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Console.Write(electionMatrix[i,j] + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine($"\nWinner by Condorcet method is: \t{candidates[winnerByCondorcetMethod]}");
            Console.WriteLine($"Winner by Borda method is: \t{candidates[winnerByBordaMethod]}");
        }       

        private T[][] GetMatrixFromFile<T>(string matrixName)
        {
            var jsonTestDataFileName = "lab3Data.json";
            var absolutePathForDocs = Path.Combine(AppDomain.CurrentDomain.BaseDirectory
                + jsonTestDataFileName);

            var json = File.ReadAllText(absolutePathForDocs);
            var jObject = JObject.Parse(json);
            var values = jObject[matrixName]?.ToObject<T[][]>();

            return values != null ? values : throw new ArgumentNullException();
        }

        private int[] GetArrayFromFile(string arrayName)
        {
            var jsonTestDataFileName = "lab3Data.json";
            var absolutePathForDocs = Path.Combine(AppDomain.CurrentDomain.BaseDirectory
                + jsonTestDataFileName);

            var json = File.ReadAllText(absolutePathForDocs);
            var jObject = JObject.Parse(json);
            var values = jObject[arrayName]?.ToObject<int[]>();

            return values != null ? values : throw new ArgumentNullException();
        }
    }
}