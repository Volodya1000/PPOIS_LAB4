using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lab4
{
    public class Graph<TVertex, TProperty> : IEnumerable<TVertex>
    {
        private class Edge
        {
            private TVertex targetVertex;
            private TProperty property;

            public TVertex TargetVertex
            {
                get { return targetVertex; }
                set { targetVertex = value; }
            }

            public TProperty Property
            {
                get { return property; }
                set { property = value; }
            }

            public Edge(TVertex target, TProperty prop)
            {
                targetVertex = target;
                property = prop;
            }
        }

        private Dictionary<TVertex, List<Edge>> adjacencyList;
        private Dictionary<TVertex, TProperty> vertexProperties;

        public Graph()
        {
            adjacencyList = new Dictionary<TVertex, List<Edge>>();
            vertexProperties = new Dictionary<TVertex, TProperty>();
        }

        public int VertexCount
        {
            get { return adjacencyList.Count; }
        }

        public int EdgeCount
        {
            get
            {
                int count = 0;
                foreach (var edges in adjacencyList.Values)
                {
                    count += edges.Count;
                }
                return count;
            }
        }

        public void AddVertex(TVertex vertex, TProperty property)
        {
            if (adjacencyList.ContainsKey(vertex))
                throw new ArgumentException("Вершина с таким ключом уже существует.");
            adjacencyList[vertex] = new List<Edge>();
            vertexProperties[vertex] = property;
        }

        public void AddEdge(TVertex source, TVertex target, TProperty property)
        {
            if (!adjacencyList.ContainsKey(source) || !adjacencyList.ContainsKey(target))
                throw new ArgumentException("Одна из вершин не существует.");
            adjacencyList[source].Add(new Edge(target, property));
        }

        public void RemoveVertex(TVertex vertex)
        {

            if (!adjacencyList.ContainsKey(vertex))
                throw new ArgumentException("Вершины с таким ключом не существует.");
            foreach (var edge in adjacencyList[vertex])
                    adjacencyList[edge.TargetVertex].RemoveAll(e => EqualityComparer<TVertex>.Default.Equals(e.TargetVertex, vertex));  
                adjacencyList.Remove(vertex);
        }

        public void RemoveEdge(TVertex source, TVertex target)
        {
            if (!adjacencyList.ContainsKey(source) || !adjacencyList.ContainsKey(target))
                throw new ArgumentException("Одна из вершин не существует.");
            adjacencyList[source].RemoveAll(edge => EqualityComparer<TVertex>.Default.Equals(edge.TargetVertex, target));
        }


        public bool ContainsVertex(TVertex vertex)
        {
            return adjacencyList.ContainsKey(vertex);
        }

        public bool ContainsEdge(TVertex source, TVertex target)
        {
            return adjacencyList.ContainsKey(source) && adjacencyList[source].Exists(edge => EqualityComparer<TVertex>.Default.Equals(edge.TargetVertex, target));
        }

        public int GetVertexDegree(TVertex vertex)
        {
            if (!adjacencyList.ContainsKey(vertex))
                throw new ArgumentException("Вершины с таким ключом не существует.");
            return adjacencyList[vertex].Count;
        }

        public TProperty GetVertexProperty(TVertex vertex)
        {
            if (!vertexProperties.ContainsKey(vertex))
                throw new ArgumentException("Вершины с таким ключом не существует.");
            return vertexProperties[vertex];
        }

        public TProperty GetEdgeProperty(TVertex source, TVertex target)
        {
            if (!adjacencyList.ContainsKey(source))
                throw new ArgumentException("Исходной вершины с таким ключом не существует.");

            var edge = adjacencyList[source].Find(e => EqualityComparer<TVertex>.Default.Equals(e.TargetVertex, target));

            if (edge == null)
                throw new ArgumentException("Ребра между указанными вершинами не существует.");

            return edge.Property;
        }

        public void SetVertexProperty(TVertex vertex, TProperty newProperty)
        {
            if (!vertexProperties.ContainsKey(vertex))
            {
                throw new ArgumentException("Вершины с таким ключом не существует.");
            }

            vertexProperties[vertex] = newProperty;
        }

        public void SetEdgeProperty(TVertex source, TVertex target, TProperty newProperty)
        {
            if (!adjacencyList.ContainsKey(source))
                throw new ArgumentException("Исходной вершины с таким ключом не существует.");

            Edge edge = adjacencyList[source].Find(e => EqualityComparer<TVertex>.Default.Equals(e.TargetVertex, target));

            if (edge == null)
                throw new ArgumentException("Ребра между указанными вершинами не существует.");

            edge.Property = newProperty;
        }

        public void Clear()
        {
            adjacencyList.Clear();
            vertexProperties.Clear();
        }

        public bool Empty()
        {
            return adjacencyList.Count == 0;
        }

        public Graph<TVertex, TProperty> Clone()
        {
            Graph<TVertex, TProperty> newGraph = new Graph<TVertex, TProperty>();

            foreach (var vertex in adjacencyList)
            {
                newGraph.AddVertex(vertex.Key, vertexProperties[vertex.Key]);
            }

            foreach (var vertex in adjacencyList)
            {
                foreach (var edge in vertex.Value)
                {
                    newGraph.AddEdge(vertex.Key, edge.TargetVertex, edge.Property);
                }
            }

            return newGraph;
        }

        public override string ToString()
        {
            string result = "";
            foreach (var vertex in adjacencyList)
            {
                result += vertex.Key + " (" + vertexProperties[vertex.Key] + ") -> ";
                foreach (var edge in vertex.Value)
                {
                    result += edge.TargetVertex + " ";
                }
                result += Environment.NewLine;
            }
            return result;
        }

        public static bool operator ==(Graph<TVertex, TProperty> left, Graph<TVertex, TProperty> right)
        {
            if (ReferenceEquals(left, right)||
                left is null || right is null||
               left.VertexCount != right.VertexCount)
                return false;

            foreach (var vertex in left.adjacencyList)
            {
                if (!right.ContainsVertex(vertex.Key))
                    return false;

                if (!EqualityComparer<TProperty>.Default.Equals(left.vertexProperties[vertex.Key], right.GetVertexProperty(vertex.Key)))
                    return false;

                foreach (var edge in vertex.Value)
                {
                    if (!right.ContainsEdge(vertex.Key, edge.TargetVertex))
                        return false;

                    if (!EqualityComparer<TProperty>.Default.Equals(edge.Property, right.GetEdgeProperty(vertex.Key, edge.TargetVertex)))
                        return false;
                }
            }

            return true;
        }
      
        public override bool Equals(object obj)
        {
            if (obj is Graph<TVertex, TProperty> other)
            {
                return this == (Graph<TVertex, TProperty>)obj;
            }
            return false;
        }

        public override int GetHashCode()
        {
            //указания компилятору на то, что операции целочисленной арифметики могут выполняться без проверки на переполнение
            unchecked
            {
                int hash = 17; 

                hash = hash * 23 + VertexCount.GetHashCode();
                hash = hash * 23 + EdgeCount.GetHashCode();

                foreach (var vertex in adjacencyList)
                {
                    hash = hash * 23 + vertexProperties[vertex.Key].GetHashCode();
                }
               
                foreach (var edges in adjacencyList.Values)
                {
                    foreach (var edge in edges)
                    {
                        hash = hash * 23 + edge.Property.GetHashCode();
                    }
                }

                return hash;
            }
        }

        public static bool operator !=(Graph<TVertex, TProperty> left, Graph<TVertex, TProperty> right)
        {
            return !(left == right);
        }
        private static int CompareTo(Graph<TVertex, TProperty> left, Graph<TVertex, TProperty> right)
        {
            return left.VertexCount.CompareTo(right.VertexCount);
        }

        public static bool operator >(Graph<TVertex, TProperty> left, Graph<TVertex, TProperty> right)
        {
            return CompareTo(left, right) > 0;
        }
        public static bool operator <(Graph<TVertex, TProperty> left, Graph<TVertex, TProperty> right)
        {
            return CompareTo(left, right) < 0;
        }

        public static bool operator >=(Graph<TVertex, TProperty> left, Graph<TVertex, TProperty> right)
        {
            return CompareTo(left, right) >= 0;
        }

        public static bool operator <=(Graph<TVertex, TProperty> left, Graph<TVertex, TProperty> right)
        {
            return CompareTo(left, right) <= 0;
        }

        public IEnumerator<TVertex> GetEnumerator()
        {
            return adjacencyList.Keys.GetEnumerator();
        }


        public IEnumerable<TVertex> Reverse()
        {
            var keys = adjacencyList.Keys.ToList();
            for (int i = keys.Count - 1; i >= 0; i--)
            {
                yield return keys[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

    public class GraphProperty<TProperty>
    {
        private TProperty property;

        public TProperty Property
        {
            get { return property; }
            set { property = value; }
        }

        public GraphProperty(TProperty prop)
        {
            property = prop;
        }
    }
}