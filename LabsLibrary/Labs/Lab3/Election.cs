using System;
using System.Collections.Generic;
using System.Linq;

namespace LabsLibrary.Labs.Lab3
{
    class Election
    {
        public int[,] GetElectionMatrix(string[][] superiorities, int[] votersCounts)
        {
            int m = superiorities.GetLength(0);
            int n = superiorities[0].GetLength(0);

            var candidates = GetListOfCandidatesFromRow<string>(superiorities[3]);
            var numberOfCandidates = candidates.Count;

            int[,] electionMatrix = new int[numberOfCandidates,
                numberOfCandidates];

            for (int i = 0; i < numberOfCandidates; i++)
            {
                var candidateToProcess = candidates[i];

                for (int j = 0; j < m; j++)
                {
                    for (int k = 0; k < n; k++)
                    {
                        if (k == i)
                            continue;

                        if (Array.IndexOf(superiorities[j], candidateToProcess)
                            < Array.IndexOf(superiorities[j], candidates[k]))
                        {
                            electionMatrix[i, k] += votersCounts[j];
                            electionMatrix[i, i] += votersCounts[j];
                        }

                    }
                }
            }

            return electionMatrix;
        }

        public int GetWinnerFromElectionMatrixByCondorcet(int[,] electionMatrix)
        {
            var m = electionMatrix.GetLength(0);
            var winsCountForEachCandidate = new int[m];

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (i == j)
                        continue;

                    if (electionMatrix[i, j] > electionMatrix[j, i])
                        winsCountForEachCandidate[i]++;
                }
            }

            var maxWinsCount = winsCountForEachCandidate.Max();
            var winnerIndex = Array.IndexOf(winsCountForEachCandidate, maxWinsCount);

            return winnerIndex;
        }

        public int GetWinnerFromElectionMatrixByBorda(int[,] electionMatrix)
        {
            int m = electionMatrix.GetLength(0);

            var winnerIndex = 0;

            for (int i = 1; i < m; i++)
            {
                if (electionMatrix[i, i] > electionMatrix[i - 1, i - 1])
                    winnerIndex = i;
            }

            return winnerIndex;
        }

        public List<T> GetListOfCandidatesFromRow<T>(T[] candidateRow)
        {
            var list = candidateRow.ToList();
            list.Sort();

            return list;
        }
    }
}