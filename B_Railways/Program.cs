// https://contest.yandex.ru/contest/25070/run-report/68685131/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace B_Railways
{
    public class Solution
    {
        private static TextReader _reader;
        private static TextWriter _writer;

        public static void Main(string[] args)
        {
            InitialiseStreams();

            var n = ReadInt();

            List<int>[] verteces = ReadGraphToAdjacencyList(n);

            var colors = new List<Color>(Enumerable.Repeat(Color.White, n + 1));

            bool optimal = true;

            for (int v = 1; v <= n; v++)
            {
                if (colors[v] == Color.White)
                {
                    optimal = DFS(verteces, v, colors);
                    if (!optimal)
                    {
                        break;
                    }
                }
            }

            if (optimal)
            {
                _writer.WriteLine("YES");
            }
            else
            {
                _writer.WriteLine("NO");
            }

            CloseStreams();
        }

        private static int ReadInt()
        {
            return int.Parse(_reader.ReadLine());
        }

        private static bool DFS(List<int>[] vertex, int start, List<Color> colors)
        {
            colors[start] = Color.Gray;

            bool ok = true;

            foreach (var v in vertex[start])
            {
                if (colors[v] == Color.Gray)
                {
                    return false;
                }
                else if (colors[v] == Color.White)
                {
                    if(ok)
                        ok = ok && DFS(vertex, v, colors);
                    else
                        return false;
                }
            }
            colors[start] = Color.Black;

            return ok;
        }

        /// <summary>
        /// Read graph storing it in adjacency list with indexes from 1 to n, where vertex[0] is fake
        /// </summary>
        /// <param name="n">Number of vertex</param>
        /// <returns></returns>
        private static List<int>[] ReadGraphToAdjacencyList(int n)
        {
            List<int>[] verteces = new List<int>[n + 1];
            for (int i = 1; i <= n; i++)
                verteces[i] = new List<int>();

            for (var i = 1; i < n; i++)
            {
                var s = _reader.ReadLine();
                var len = n - i;
                for (int j = 1; j <= len; j++)
                {
                    if (s[j - 1] == 'R')
                    {
                        verteces[i].Add(j + i);
                    }
                    else
                    {
                        verteces[j + i].Add(i);
                    }

                }
            }
            return verteces;
        }

        private static void CloseStreams()
        {
            _reader.Close();
            _writer.Close();
        }

        private static void InitialiseStreams()
        {
            _reader = new StreamReader(Console.OpenStandardInput());
            _writer = new StreamWriter(Console.OpenStandardOutput());
        }


        enum Color
        {
            White,
            Gray,
            Black,
        };
    }
}
