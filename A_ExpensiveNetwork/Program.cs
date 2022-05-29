// https://contest.yandex.ru/contest/25070/run-report/68683369/

/* 
 * -- ПРИНЦИП РАБОТЫ --
   Алгоритм полностью совпадает с тем, который описан в уроке:
    1) Алгоритму не важно, с какой вершины начинать, так как в итоге все вершины попадут в минимальное остовное дерево. 
        Поэтому я взял первую вершину.
    2) Рассмотрим все рёбра, исходящие из этой вершины. Возьмём ребро с минимальным весом и добавим в остов ребро и вершину,
        в которую оно входило.
    3) Добавим ко множеству потенциально добавляемых рёбер все, которые исходят из новой вершины и входят в вершины, 
        ещё не включённые в остов.
    4) Повторяем пункты 2 и 3 до тех пор, пока в остовном дереве не будет nn вершин и, соответственно, n-1 рёбер.
        Так как на каждой итерации цикла мы добавляем ровно одно ребро и одну вершину, нам потребуется n−1 итерация.
  
 * -- ДОКАЗАТЕЛЬСТВО КОРРЕКТНОСТИ --
    На каждом шаге мы извлекаем наиболее приоритетный элемент из кучи. 
    Самый приоритетный элемент пирамиды находится на её вершине. 
    Поэтому когда извлекаем и удаляем его из кучи, мы обрабатываем все элементы и выдаем их в отсортированном порядке.
  
 * -- ВРЕМЕННАЯ СЛОЖНОСТЬ --
    Алгоритму требуется число шагов, пропорциональное количеству вершин. На каждом шаге мы находим минимальное по весу ребро. 
    На поиск минимального ребра нам требуется в худшем случае перебрать все рёбра. 
    В итоге сложность алгоритма будет O(∣V∣⋅∣E∣).
    Благодаря использованию алгоритма приоритетной очереди из прошлой финалки сложность алгоритма Прима стала O(∣E∣⋅log∣V∣), 
    где ∣E∣ — количество рёбер в графе, а ∣V∣ — количество вершин.
    
 * -- ПРОСТРАНСТВЕННАЯ СЛОЖНОСТЬ --
    Для описанной реализации алгоритма пирамидальной сортировки нужно выделить память под массив из n элементов. 
    То есть потребуется O(n) дополнительной памяти. 
 */


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
            int result = FindMaxWeightOfST(graph, n, m);

            // print result
            if (result != -1)
            {
                _writer.WriteLine(result);
            }
            else
            {
                _writer.WriteLine("Oops! I did it again");
            }


            CloseStreams();
        }


        private static int FindMaxWeightOfST(List<Ray>[] graph, int n, int m)
        {
            var notAdded = new HashSet<int>(Enumerable.Range(1, n));
            var rays = new MyHeapOfRays(m);
            int sum = 0;
            var v = 1;

            AddVertex(v, notAdded, rays, graph);
            while (notAdded.Count > 0)
            {
                Ray e = rays.GetMaxPriority();
                if (e == null)
                {
                    return -1;
                }
                if (notAdded.Contains(e.To))
                {
                    sum += e.Weight;
                    AddVertex(e.To, notAdded, rays, graph);
                }
            }

            return sum;
        }

        private static void AddVertex(int v, HashSet<int> notAdded, MyHeapOfRays rays, List<Ray>[] graph)
        {
            notAdded.Remove(v);
            foreach (var ray in graph[v])
            {
                if (notAdded.Contains(ray.To))
                    rays.Add(ray);
            }
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
                vertex[items[0]].Add(new Ray { To = items[1], Weight = items[2] });
                vertex[items[1]].Add(new Ray { To = items[0], Weight = items[2] });
            }

            return vertex;
        }

        class Ray : IComparable<Ray>
        {
            public int Weight;
            public int To;

            public int CompareTo(Ray other)
            {
                return (other.Weight, other.To).CompareTo((Weight, To));
            }

            public static bool operator <(Ray op1, Ray op2)
            {
                return op1.CompareTo(op2) < 0;
            }

            public static bool operator >(Ray op1, Ray op2)
            {
                return op1.CompareTo(op2) > 0;


            }
        }

        class MyHeapOfRays
        {
            private Ray[] _items;
            private int _maxSize;
            private int _lastIndex;


            public MyHeapOfRays(int n)
            {
                _items = new Ray[n + 1];
                _lastIndex = 0;
                _maxSize = n;
            }

            public void Add(Ray r)
            {
                if (_lastIndex > _maxSize)
                {
                    throw new IndexOutOfRangeException();
                }
                _items[++_lastIndex] = r;
                SiftUp(_lastIndex);
            }

            private void SiftUp(int index)
            {
                if (index <= 1)
                {
                    return;
                }

                int parentIndex = index / 2;
                if (_items[parentIndex] > _items[index])
                {
                    var temp = _items[parentIndex];
                    _items[parentIndex] = _items[index];
                    _items[index] = temp;
                    SiftUp(parentIndex);
                }

            }

            private void SiftDown(int index)
            {
                int left = 2 * index;
                int right = 2 * index + 1;

                if (_lastIndex < left)
                {
                    return;
                }
                int indexLargest;
                if (right <= _lastIndex)
                {
                    if (_items[left] < _items[right])
                    {
                        indexLargest = left;
                    }
                    else
                    {
                        indexLargest = right;
                    }
                }
                else
                {
                    indexLargest = left;
                }

                if (_items[index] > _items[indexLargest])
                {
                    var temp = _items[indexLargest];
                    _items[indexLargest] = _items[index];
                    _items[index] = temp;
                    SiftDown(indexLargest);
                }
            }

            public Ray GetMaxPriority()
            {
                if (_lastIndex == 0)
                {
                    return null;
                }
                var result = _items[1];
                _items[1] = _items[_lastIndex--];
                SiftDown(1);
                return result;
            }
        }

    }
}
