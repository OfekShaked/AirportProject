using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL.DataStructures
{
    public class Path
    {
        public int TotalWeight { get; set; }
        public LinkedList<int> VerticesIndexes { get; set; }
        public List<int> PathIndexes { get; set; }
        public Path()
        {
            PathIndexes = new List<int>();
            VerticesIndexes = new LinkedList<int>();
        }
        public void AddIndex(int index)
        {
            VerticesIndexes.AddFirst(index);
        }
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("Indexes");
            foreach (var item in VerticesIndexes)
            {
                str.AppendLine(item.ToString()); 
            }
            return str.ToString();
        }
    }
    public class Graph
    {
        LinkedList<Vertices>[] db;
        public Graph(int length)
        {
            db = new LinkedList<Vertices>[length];
            for (int i = 0; i < length; i++)
            {
                db[i] = new LinkedList<Vertices>(new Vertices[] { });
            }
        }
        private Node[] FillDijkstraTable(int source)
        {
            Node[] nodes = new Node[db.Length];
            for (int i = 0; i < db.Length; i++)
            {
                nodes[i] = new Node();
            }
            nodes[source].WeightFromSource = 0;
            nodes[source].Path.PathIndexes.Add(source);
            int currentIndex = source;
            for (int i = 0; i < db.Length; i++)
            {
                int minIndex = 0, minValue = int.MaxValue;
                if (currentIndex >= 0 && currentIndex < db.Length)
                {
                    foreach (var item in db[currentIndex])
                    {
                        if (nodes[item.destVertices].Permanent) continue;
                        if (nodes[currentIndex].WeightFromSource + item.weight < nodes[item.destVertices].WeightFromSource)
                        {
                            nodes[item.destVertices].WeightFromSource = nodes[currentIndex].WeightFromSource + item.weight;
                            nodes[item.destVertices].Path.PathIndexes = new List<int>(nodes[currentIndex].Path.PathIndexes);
                            nodes[item.destVertices].Path.PathIndexes.Add(item.destVertices);
                        }
                        if (nodes[item.destVertices].WeightFromSource < minValue)
                        {
                            minValue = nodes[item.destVertices].WeightFromSource;
                            minIndex = item.destVertices;
                        }
                    }
                    nodes[currentIndex].Permanent = true;
                    currentIndex = FindMinValueNode(nodes);
                }
            }
            return nodes;

        }
        public Node FindMinPath(int source, int dest)
        {
            return FillDijkstraTable(source)[dest];
        }
        public void FindAndShowDijkstraMinPath(int source)
        {
            Node[] nodes = FillDijkstraTable(source);
            int count = 0;
            foreach (Node item in nodes)
            {
                Console.WriteLine($"Node Number {count++} Weight : {item.WeightFromSource} Path : ");
                foreach (int pathItem in item.Path.PathIndexes)
                {
                    Console.Write(pathItem + ", ");
                }
                Console.WriteLine(" \n Next Node :  \n \n");
            }
        }

        private int FindMinValueNode(Node[] nodes)
        {
            int minIndex = -1, minValue = int.MaxValue;
            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i].Permanent == false && nodes[i].WeightFromSource < minValue)
                {
                    minValue = nodes[i].WeightFromSource;
                    minIndex = i;
                }
            }
            return minIndex;
        }


        public void AddEdge(int source, int dest, int weight)
        {
            db[source].AddLast(new Vertices(dest, weight));
        }
        public void RemoveEdge(int source, int dest, int weight)
        {
            db[source].Remove(new Vertices(dest, weight));
        }
        public void UpdateEdgeWeight(int source, int dest, int weight)
        {
            Vertices verticesToUpdate = db[source].FirstOrDefault(v => v.destVertices == dest && v.weight == weight);
            if (verticesToUpdate != null)
            {
                verticesToUpdate.weight = weight;
            }

        }
        public IEnumerable<Path> FindAllPaths(int source, int dest)
        {
            List<Path> allPath = new List<Path>();
            bool[] visited = new bool[db.Length];
            visited[source] = true;
            FindAllPaths(source, dest, allPath, visited);
            allPath.ForEach(p => { p.VerticesIndexes.AddLast(dest); });
            return allPath;

        }
        private void FindAllPaths(int source, int dest, List<Path> allPath, bool[] visited)
        {
            if (source == dest)
            {
                allPath.Add(new Path());
            }
            if (db[source] == null) return;
            foreach (Vertices item in db[source])
            {
                if (visited[item.destVertices]) continue;
                visited[item.destVertices] = true;
                int oldCount = allPath.Count;
                FindAllPaths(item.destVertices, dest, allPath, visited);
                for (int i = oldCount; i < allPath.Count; i++)
                {
                    allPath[i].AddIndex(source);
                    allPath[i].TotalWeight += item.weight;
                }
                visited[item.destVertices] = false;
            }
        }
        public bool IsPathExist(int source, int dest, int maxSteps)
        {
            bool[] visited = new bool[db.Length];
            visited[source] = true;
            return IsPathExist(source, dest, maxSteps, visited);
        }
        private bool IsPathExist(int source, int dest, int maxSteps, bool[] visited)
        {
            if (source == dest) return true;
            if (maxSteps == 0) return false;
            if (db[source] == null) return false;
            foreach (Vertices item in db[source])
            {
                if (visited[item.destVertices]) continue;
                visited[item.destVertices] = true;
                bool isPathFound = IsPathExist(item.destVertices, dest, maxSteps - 1, visited);
                if (isPathFound) return true;
                visited[item.destVertices] = false;
            }
            return false;
        }

        class Vertices
        {
            public int weight;
            public int destVertices;
            public Vertices(int destVertices, int weight)
            {
                this.weight = weight;
                this.destVertices = destVertices;
            }
        }
        public class Node
        {
            public int WeightFromSource { get; set; }
            public bool Permanent { get; set; }
            public Path Path { get; set; }
            public Node()
            {
                Path = new Path();
                WeightFromSource = int.MaxValue;
                Permanent = false;
            }
        }
    }
}
