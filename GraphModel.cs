using System;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace Graph
{
    public class GraphModel
    {
        List<Vertex> _vertexes = new List<Vertex>();
        List<Edge> _edges = new List<Edge>();
        List<Edge> _invertedEdges = new List<Edge>();
        bool[] _opened = new bool[0];
        bool[] _processed = new bool[0];
        int[] _transactionCounter = new int[0];
        int?[] _countPath = new int?[0];
        int?[] _sourcePath = new int?[0];
        Dictionary<int, int?>  _vertexToAnalyze = new Dictionary<int, int?>();
        int _counter = 0;

        public int VertexCount => _vertexes.Count;
        public int EdgeCount => _edges.Count;
        public void AddVertex(Vertex vertex)
        {
            _vertexes.Add(vertex);
        }
        public void AddEdge(Vertex from, Vertex to, int weight = 1)
        {
            var edge = new Edge(from, to, weight);
            _edges.Add(edge);
        }
        public int[,] GetMatrix()
        {
            var matrix = new int[VertexCount, VertexCount];
            foreach(var edge in _edges)
            {
                var row = edge.From.Number;
                var column = edge.To.Number;

                matrix[row, column] = edge.Weight;
                if(row == column)
                {
                    matrix[column, row] = edge.Weight * 2;
                }
                else
                {
                    matrix[column, row] = edge.Weight;
                }
            }
            return matrix;
        }
        public void PrintMatrix()
        {
            var matrix = GetMatrix();
            for (int i = 0; i < VertexCount; i++)
            {
                Console.Write(" | " + i + " | ");
            }
            Console.WriteLine();
            Console.WriteLine("------------------------------------");
            for (int i = 0; i < VertexCount; i++)
            {
                Console.Write(i + 1);
                for (int j = 0; j < VertexCount; j++)
                {
                    Console.Write(" | " + matrix[i, j] + " | ");
                }
                Console.WriteLine();
            }
        }
        public int[][] GetVector()
        {
            int[][] adjacencyVector = new int[VertexCount][];
            for(int i = 0; i < VertexCount; i++)
            {
                var direction = _edges.Where(x => x.From.Number == i).Select(x => x.To.Number).ToArray();
                adjacencyVector[i] = direction;
            }
            return adjacencyVector;
        }
        public int[] Kosaraju()
        {
            _opened = new bool[VertexCount];
            _processed = new bool[VertexCount];
            _transactionCounter = new int[VertexCount];
            foreach (var vertex in _vertexes)
            {
                _counter = 0;
                DFS(vertex);
            }
            InvertGraph();
            _opened = new bool[VertexCount];
            _processed = new bool[VertexCount];
            _counter = 1;
            var sortedVerexes = _vertexes.OrderByDescending(x => x.TransactionCounter);
            foreach (var vertex in sortedVerexes)
            {
                if (LookUpComponent(vertex)) { _counter++;};
            }
            var result = _vertexes.OrderBy(x => x.Number).Select(x => x.Component).ToArray();
            return result;
        }
        private void DFS(Vertex vertex)
        {
            if (_processed[vertex.Number] == true) { return; }
            _counter++;
            vertex.TransactionCounter = _counter;
            _opened[vertex.Number] = true;
            var nextVertexes = _edges.Where(x => x.From.Number == vertex.Number).Select(x => x.To).ToArray();
            foreach(var iterVetrex in nextVertexes)
            {
                if (_opened[iterVetrex.Number] == false) { DFS(iterVetrex); }
            }
            _processed[vertex.Number] = true;
        }
        private bool LookUpComponent(Vertex vertex)
        {
            if (_processed[vertex.Number] == true) { return false; }
            _transactionCounter[vertex.Number] = _counter;
            _opened[vertex.Number] = true;
            vertex.Component = _counter;
            var nextVertexes = _invertedEdges.Where(x => x.From.Number == vertex.Number).Select(x => x.To).ToArray();
            foreach (var iterVetrex in nextVertexes)
            {
                if (_opened[iterVetrex.Number] == false) { LookUpComponent(iterVetrex); }
            }
            _processed[vertex.Number] = true;
            return true;
        }
        private void InvertGraph()
        {
            foreach (var edge in _edges)
            {
                _invertedEdges.Add(new Edge(edge.To, edge.From));
            }
        }
        public int[] DemukronSort()
        {
            CheckingCycle();
            var sortedVertexes = new List<int>();
            var matrix = GetSourceMatrix();
            var working = true;

            var sumRow = new int[VertexCount];
            for (int i = VertexCount - 1; i >= 0; i--)
            {
                for (int j = VertexCount - 1; j >= 0; j--)
                    sumRow[i] += matrix[j, i];
            }

            while (working)
            {
                
                var iterSortedVertexes = new List<int>();
                for (int i = VertexCount - 1; i >= 0; i--)
                {
                    if(sumRow[i] == 0 && sortedVertexes.Contains(i) == false)
                        iterSortedVertexes.Add(i);
                }

                foreach(var vertex in iterSortedVertexes)
                {
                    for (int i = VertexCount - 1; i >= 0; i--)
                    {
                        sumRow[i] -= matrix[vertex, i];
                    }
                    sortedVertexes.Add(vertex);
                }
                working = sumRow.Where(x => x != 0).Any();
            }

            var lastElements = _vertexes.Where(x => sortedVertexes.Contains(x.Number) == false);
            foreach (var element in lastElements)
                sortedVertexes.Add(element.Number);

            return sortedVertexes.ToArray();
        }
        private int[,] GetSourceMatrix()
        {
            var matrix = new int[VertexCount, VertexCount];
            foreach(var edge in _edges)
            {
                matrix[edge.From.Number, edge.To.Number] = 1;
            }
            return matrix;
        }
        private void CheckingCycle()
        {
            _opened = new bool[VertexCount];
            _processed = new bool[VertexCount];
            foreach (var vertex in _vertexes)
            {
                if (OrientedDFS(vertex)) { throw new Exception("Graph contains a cycle"); };
            }
        }
        private bool OrientedDFS(Vertex vertex)
        {
            _opened[vertex.Number] = true;
            var nextVertexes = _edges.Where(x => x.From.Number == vertex.Number).Select(x => x.To).ToArray();
            foreach (var iterVetrex in nextVertexes)
            {
                if (!_opened[iterVetrex.Number])
                {
                    if (OrientedDFS(iterVetrex)) { return true; }
                }
                else if (!_processed[iterVetrex.Number]) 
                {
                    return true;
                }
            }
            _processed[vertex.Number] = true;
            return false;
        }
        public Edge[] Kruskal()
        {
            var result = new List<Edge>();
            foreach (var vertex in _vertexes)
            {
                vertex.SetParent(vertex);
            }

            var sortedEdges = _edges.OrderBy(x => x.Weight).ToArray();
            foreach (var edge in sortedEdges)
            {
                var firstParent = edge.From.GetParent(edge.From);
                var secondParent = edge.To.GetParent(edge.To);

                if (firstParent != secondParent)
                {
                    edge.From.SetParent(secondParent);
                    //Console.WriteLine("Added " + edge.ToString());
                    result.Add(edge);
                }
            }

            return result.ToArray();
        }
        public Edge[] Dijkstra(Vertex? from = null, Vertex? to = null)
        {
            var result = new List<Edge>();
            _sourcePath = new int?[VertexCount];

            from = from == null ? _vertexes.MinBy(x => x.Number) : from;
            to = to == null ? _vertexes.MaxBy(x => x.Number) : from;

            _vertexToAnalyze = new Dictionary<int,int?>();

            foreach (var vertex in _vertexes)
            {
                _vertexToAnalyze.Add(vertex.Number, null);
            }

            _vertexToAnalyze[from.Number] = 0;


            for (int i = VertexCount; i > 0; i--)
            {
                var minIndex = _vertexToAnalyze.MinBy(x => x.Value).Key;
                if (minIndex == to.Number) { break; }
                if (_vertexToAnalyze.ContainsKey(minIndex)) { IterateForDijkstra(minIndex); }

            }

            for (int i = 0; i < _sourcePath.Length; i++)
            {
                if (_sourcePath[i] == null) continue;

                var edge = _edges.Where(x => x.From.Number == _sourcePath[i] && x.To.Number == i).First();
                result.Add(edge);
            }
            return result.ToArray();
        }
        private void IterateForDijkstra(int vertexValue)
        {
            var nextVertexes = _edges.Where(x => x.From.Number == vertexValue).Select(x => x.To).ToArray();
            foreach (var iterVetrex in nextVertexes)
            {
                if (!_vertexToAnalyze.ContainsKey(iterVetrex.Number)) { continue; }

                var linkedEdge = _edges.Where(x => x.From.Number == vertexValue & x.To.Number == iterVetrex.Number).First();

                if (_vertexToAnalyze[iterVetrex.Number] == null || _vertexToAnalyze[iterVetrex.Number] > linkedEdge.Weight)
                {
                    _vertexToAnalyze[iterVetrex.Number] = linkedEdge.Weight;
                    _sourcePath[iterVetrex.Number] = vertexValue;
                }
            }

            _vertexToAnalyze.Remove(vertexValue);

        }
    }
}
