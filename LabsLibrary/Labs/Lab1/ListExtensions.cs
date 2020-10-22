using System.Collections.Generic;

namespace LabsLibrary.Labs.Lab1
{
    public static class ListExtensions
    {
        public static List<int> IndexesOf<T>(this List<T> list, T item)// where T: class
        {
            List<int> indexes = new List<int>();

            var listLength = list.Count;

            for (int i = 0; i < listLength; i++)
            {
                if (EqualityComparer<T>.Default.Equals(list[i], item))
                    indexes.Add(i);
            }

            return indexes;
        }
    }
}