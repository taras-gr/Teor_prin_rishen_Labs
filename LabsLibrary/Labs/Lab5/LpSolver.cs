using System.Collections.Generic;
using System.Linq;

namespace LabsLibrary.Labs.Lab5
{
    public class LpSolver
    {
        private void InitBasises(Dictionary<int, string> Basises, int m)
        {
            for (int i = 0; i < m; i++)
            {
                Basises.Add(i, "X" + (m + i + 1).ToString());
            }
        }

        private int Bland(double[,] _a, int m, int n)
        {
            for (int q = 0; q < m + n; q++)
                if (_a[m, q] > 0) { /*BlandCount++; */return q; }// return q;

            return -1;
        }

        private int MinRationRule(int m, int n, double[,] _a, int q)
        {
            int p = -1;
            for (int i = 0; i < m; i++)
            {
                if (_a[i, q] <= 0) continue;
                else if (p == -1) p = i;
                else if (_a[i, m + n] / _a[i, q] < _a[p, m + n] / _a[p, q])
                    p = i;
            }

            return p;
        }

        private void Pivot(Dictionary<int, string> Basises, double[,] _a, int m, int n, int p, int q)
        {
            Basises[p] = "X" + (q + 1).ToString();
            for (int i = 0; i <= m; i++)
                for (int j = 0; j <= m + n; j++)
                    if (i != p && j != q)
                        _a[i, j] -= _a[p, j] * _a[i, q] / _a[p, q];

            for (int i = 0; i <= m; i++)
                if (i != p) _a[i, q] = 0.0;

            for (int j = 0; j <= m + n; j++)
                if (j != q) _a[p, j] /= _a[p, q];
            _a[p, q] = 1.0;
        }

        public Dictionary<string, double> SolveBySimplexMethod(double[,] a, double[] b, double[] c)
        {
            var m = b.Length;
            var n = c.Length;
            var Basises = new Dictionary<int, string>();
            var _a = new double[m + 1, m + n + 1];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    _a[i, j] = a[i, j];
            for (int j = n; j < m + n; j++) _a[j - n, j] = 1.0;
            for (int j = 0; j < n; j++) _a[m, j] = c[j];
            for (int i = 0; i < m; i++) _a[i, m + n] = b[i];
            InitBasises(Basises, m);

            while (true)
            {
                int q = Bland(_a, m, n);
                if (q == -1) break;

                int p = MinRationRule(m, n, _a, q);
                if (p == -1) break;

                Pivot(Basises, _a, m, n, p, q);
            }

            var results = GetResult(_a, Basises, n);

            return results;
        }

        public Dictionary<string, double> GetResult(double[,] _a, Dictionary<int, string> Basises, int n)
        {
            int bIndex = _a.GetLength(1) - 1;
            var resultsDictionary = new Dictionary<string, double>();

            for (int i = 0; i < n; i++)
            {
                var resRowIndex = Basises.FirstOrDefault(x => x.Value == "X" + (i + 1).ToString()).Key;
                resultsDictionary.Add("X" + (i + 1).ToString(), _a[resRowIndex, bIndex]);
            }

            return resultsDictionary;
        }
    }
}