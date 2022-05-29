using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace A_ExpensiveNetwork
{
    public class Solution
    {
        private static TextReader _reader;
        private static TextWriter _writer;

        public static TextWriter Writer { get => _writer; set => _writer = value; }

        public static void Main(string[] args)
        {
            InitialiseStreams();

            var numbers = ReadList();
            var n = numbers[0];
            var m = numbers[1];

            List<Ray>[] graph = ReadGraphToAdjacencyList(n, m);
            try
            {
                int result = FindMaxWeightOfST(graph);
                _writer.WriteLine(result);
            }
            catch (InvalidOperationException)
            {
                _writer.WriteLine("Oops! I did it again");
            }


            CloseStreams();
        }


        private static int FindMaxWeightOfST(List<Ray>[] graph)
        {
            var added = new HashSet<int>();
            var notAdded = new HashSet<int>(Enumerable.Range(1, graph.Length - 1));
            var rays = new List<Ray>();
            int sum = 0;
            var v = 1;

            AddVertex(v, added, notAdded, rays, graph);
            while (notAdded.Count > 0)
            {
                var e = rays.OrderByDescending(r => r.Weight).First();
                rays.Remove(e);
                if (notAdded.Contains(e.To))
                {
                    sum += e.Weight;
                    AddVertex(e.To, added, notAdded, rays, graph);
                }
            }

            return sum;
        }

        private static void AddVertex(int v, HashSet<int> added, HashSet<int> notAdded, List<Ray> rays, List<Ray>[] graph)
        {
            added.Add(v);
            notAdded.Remove(v);
            rays.AddRange(graph[v].Where(r => notAdded.Contains(r.To)));
        }

        private static void CloseStreams()
        {
            _reader.Close();
            Writer.Close();
        }

        private static void InitialiseStreams()
        {
            _reader = new StreamReader(Console.OpenStandardInput());
            Writer = new StreamWriter(Console.OpenStandardOutput());
        }

        private static int ReadInt()
        {
            return int.Parse(_reader.ReadLine());
        }

        private static List<int> ReadList()
        {
            return _reader.ReadLine()
                .Split(new[] { ' ', '\t', }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();
        }

        private static List<Ray>[] ReadGraphToAdjacencyList(int n, int m)
        {
            List<Ray>[] vertex = new List<Ray>[n + 1];
            for (int i = 1; i <= n; i++)
                vertex[i] = new List<Ray>();

            for (var i = 0; i < m; i++)
            {
                var items = ReadList();
                vertex[items[0]].Add(new Ray { From = items[0], To = items[1], Weight = items[2] });
                vertex[items[1]].Add(new Ray { From = items[1], To = items[0], Weight = items[2] });
            }

            return vertex;
        }

        struct Ray
        {
            public int From;
            public int To;
            public int Weight;
        }
    }
}
