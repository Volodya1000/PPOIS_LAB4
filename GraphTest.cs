using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Lab4;

[TestClass]
public class GraphTests
{
    [TestMethod]
    public void TestAddVertex()
    {
        Graph<int, string> graph = new Graph<int, string>();
        graph.AddVertex(1, "A");

        Assert.IsTrue(graph.ContainsVertex(1));
    }

    [TestMethod]
    public void TestAddEdge()
    {
        Graph<int, string> graph = new Graph<int, string>();
        graph.AddVertex(1, "A");
        graph.AddVertex(2, "B");
        graph.AddEdge(1, 2, "Edge1");

        Assert.IsTrue(graph.ContainsEdge(1, 2));
    }

    [TestMethod]
    public void TestRemoveVertex()
    {
        Graph<int, string> graph = new Graph<int, string>();
        graph.AddVertex(1, "A");
        graph.RemoveVertex(1);

        Assert.IsFalse(graph.ContainsVertex(1));
    }

    [TestMethod]
    public void TestRemoveEdge()
    {
        Graph<int, string> graph = new Graph<int, string>();
        graph.AddVertex(1, "A");
        graph.AddVertex(2, "B");
        graph.AddEdge(1, 2, "Edge1");
        graph.RemoveEdge(1, 2);

        Assert.IsFalse(graph.ContainsEdge(1, 2));
    }

    [TestMethod]
    public void TestVertexDegree()
    {
        Graph<int, string> graph = new Graph<int, string>();
        graph.AddVertex(1, "A");
        graph.AddVertex(2, "B");
        graph.AddEdge(1, 2, "Edge1");

        int degree = graph.GetVertexDegree(1);

        Assert.AreEqual(1, degree);
    }

    [TestMethod]
    public void TestVertexProperty()
    {
        Graph<int, string> graph = new Graph<int, string>();
        graph.AddVertex(1, "A");

        string property = graph.GetVertexProperty(1);

        Assert.AreEqual("A", property);
    }

    [TestMethod]
    public void TestEdgeProperty()
    {
        Graph<int, string> graph = new Graph<int, string>();
        graph.AddVertex(1, "A");
        graph.AddVertex(2, "B");
        graph.AddEdge(1, 2, "Edge1");

        string property = graph.GetEdgeProperty(1, 2);

        Assert.AreEqual("Edge1", property);
    }

    [TestMethod]
    public void TestGraphClone()
    {
        Graph<int, string> graph = new Graph<int, string>();
        graph.AddVertex(1, "A");
        graph.AddVertex(2, "B");
        graph.AddEdge(1, 2, "Edge1");

        Graph<int, string> clonedGraph = graph.Clone();

        Assert.IsTrue(graph == clonedGraph);
    }
}
