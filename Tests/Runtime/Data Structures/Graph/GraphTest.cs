using System.Collections;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace KalkuzSystems.Utility.Tests
{
  public class GraphTest
  {
    [UnityTest]
    public IEnumerator TestIntGraphWithNoEdgesNoVertices()
    {
      var graph = new Graph<int>();
      
      Assert.AreEqual(0, graph.Vertices.Count);
      Assert.AreEqual(0, graph.Edges.Count);
      
      yield return null;
    }
    
    [UnityTest]
    public IEnumerator TestIntGraphCanAddOneVertex()
    {
      var graph = new Graph<int>();
      var vertex = graph.AddVertex(0, "Vertex 0", 0);
      
      Assert.AreEqual(1, graph.Vertices.Count);
      Assert.AreEqual(0, graph.Edges.Count);
      
      Assert.AreEqual(0, vertex.Id);
      Assert.AreEqual("Vertex 0", vertex.Label);
      Assert.AreEqual(0, vertex.Data);
      
      yield return null;
    }
    
    [UnityTest]
    public IEnumerator TestIntGraphCanAddTwoVertices()
    {
      var graph = new Graph<int>();
      var vertex0 = graph.AddVertex(0, "Vertex 0", 0);
      var vertex1 = graph.AddVertex(1, "Vertex 1", 1);
      
      Assert.AreEqual(2, graph.Vertices.Count);
      Assert.AreEqual(0, graph.Edges.Count);
      
      Assert.AreEqual(0, vertex0.Id);
      Assert.AreEqual("Vertex 0", vertex0.Label);
      Assert.AreEqual(0, vertex0.Data);
      
      Assert.AreEqual(1, vertex1.Id);
      Assert.AreEqual("Vertex 1", vertex1.Label);
      Assert.AreEqual(1, vertex1.Data);
      
      yield return null;
    }
    
    [UnityTest]
    public IEnumerator TestIntGraphCanAddOneDirectedEdge()
    {
      var graph = new Graph<int>();
      var vertex0 = graph.AddVertex(0, "Vertex 0", 0);
      var vertex1 = graph.AddVertex(1, "Vertex 1", 1);
      var edge = graph.AddDirectedEdge(vertex0, vertex1);
      
      Assert.AreEqual(2, graph.Vertices.Count);
      Assert.AreEqual(1, graph.Edges.Count);
      
      Assert.AreEqual(0, vertex0.Id);
      Assert.AreEqual("Vertex 0", vertex0.Label);
      Assert.AreEqual(0, vertex0.Data);
      
      Assert.AreEqual(1, vertex1.Id);
      Assert.AreEqual("Vertex 1", vertex1.Label);
      Assert.AreEqual(1, vertex1.Data);
      
      Assert.AreEqual(1, edge.Weight);
      Assert.AreEqual(vertex0, edge.From);
      Assert.AreEqual(vertex1, edge.To);
      
      Assert.IsFalse(edge.IsLoop);
      
      Assert.IsTrue(vertex0.HasEdge(edge));
      Assert.IsTrue(vertex1.HasEdge(edge));
      
      Assert.IsTrue(vertex0.OutgoingEdges.Contains(edge));
      Assert.IsTrue(vertex1.IncomingEdges.Contains(edge));
      
      Assert.IsFalse(vertex0.IncomingEdges.Contains(edge));
      Assert.IsFalse(vertex1.OutgoingEdges.Contains(edge));
      
      Assert.AreEqual(0, vertex0.LoopingEdges.Count);
      Assert.AreEqual(0, vertex1.LoopingEdges.Count);
      
      Assert.IsTrue(vertex0.HasNeighbor(vertex1));
      Assert.IsFalse(vertex1.HasNeighbor(vertex0));
      
      yield return null;
    }
    
    [UnityTest]
    public IEnumerator TestIntGraphCanAddOneUndirectedEdge()
    {
      var graph = new Graph<int>();
      var vertex0 = graph.AddVertex(0, "Vertex 0", 0);
      var vertex1 = graph.AddVertex(1, "Vertex 1", 1);
      var edges = graph.AddUndirectedEdge(vertex0, vertex1);
      var edge = edges[0];
      var oppositeEdge = edges[1];
      
      Assert.AreEqual(2, graph.Vertices.Count);
      Assert.AreEqual(2, graph.Edges.Count);
      
      Assert.AreEqual(0, vertex0.Id);
      Assert.AreEqual("Vertex 0", vertex0.Label);
      Assert.AreEqual(0, vertex0.Data);
      
      Assert.AreEqual(1, vertex1.Id);
      Assert.AreEqual("Vertex 1", vertex1.Label);
      Assert.AreEqual(1, vertex1.Data);
      
      Assert.AreEqual(1, edge.Weight);
      Assert.AreEqual(1, oppositeEdge.Weight);
      
      Assert.AreEqual(vertex0, edge.From);
      Assert.AreEqual(vertex1, edge.To);
      Assert.AreEqual(vertex1, oppositeEdge.From);
      Assert.AreEqual(vertex0, oppositeEdge.To);
      
      Assert.IsFalse(edge.IsLoop);
      Assert.IsFalse(oppositeEdge.IsLoop);
      
      Assert.IsTrue(vertex0.HasEdge(edge));
      Assert.IsTrue(vertex1.HasEdge(edge));
      Assert.IsTrue(vertex0.HasEdge(oppositeEdge));
      Assert.IsTrue(vertex1.HasEdge(oppositeEdge));
      
      Assert.IsTrue(vertex0.OutgoingEdges.Contains(edge));
      Assert.IsTrue(vertex1.IncomingEdges.Contains(edge));
      
      Assert.IsTrue(vertex0.IncomingEdges.Contains(oppositeEdge));
      Assert.IsTrue(vertex1.OutgoingEdges.Contains(oppositeEdge));
      
      Assert.AreEqual(0, vertex0.LoopingEdges.Count);
      Assert.AreEqual(0, vertex1.LoopingEdges.Count);
      
      Assert.IsTrue(vertex0.HasNeighbor(vertex1));
      Assert.IsTrue(vertex1.HasNeighbor(vertex0));
      
      yield return null;
    }
    
    [UnityTest]
    public IEnumerator TestIntGraphCanAddOneLoopEdge()
    {
      var graph = new Graph<int>();
      var vertex0 = graph.AddVertex(0, "Vertex 0", 0);
      var edge = graph.AddDirectedEdge(vertex0, vertex0);
      
      Assert.AreEqual(1, graph.Vertices.Count);
      Assert.AreEqual(1, graph.Edges.Count);
      
      Assert.AreEqual(0, vertex0.Id);
      Assert.AreEqual("Vertex 0", vertex0.Label);
      Assert.AreEqual(0, vertex0.Data);
      
      Assert.AreEqual(1, edge.Weight);
      Assert.AreEqual(vertex0, edge.From);
      Assert.AreEqual(vertex0, edge.To);
      
      Assert.IsTrue(edge.IsLoop);
      
      Assert.IsTrue(vertex0.HasEdge(edge));
      
      Assert.IsFalse(vertex0.OutgoingEdges.Contains(edge));
      Assert.IsFalse(vertex0.IncomingEdges.Contains(edge));
      
      Assert.IsTrue(vertex0.LoopingEdges.Contains(edge));
      
      Assert.IsTrue(vertex0.HasNeighbor(vertex0));
      
      yield return null;
    }
  }
}