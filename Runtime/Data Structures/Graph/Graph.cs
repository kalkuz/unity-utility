using System;
using System.Collections.Generic;

namespace KalkuzSystems.Utility
{
  [Serializable]
  public class Graph
  {
    private Dictionary<int, GraphVertex> vertices;
    private List<GraphEdge> edges;

    public Dictionary<int, GraphVertex> Vertices => vertices;
    public List<GraphEdge> Edges => edges;

    public Graph()
    {
      vertices = new Dictionary<int, GraphVertex>();
      edges = new List<GraphEdge>();
    }

    public GraphVertex AddVertex(int id, string label, Action<GraphVertex> onNeighborAdded = null,
      Action<GraphVertex> onNeighborRemoved = null, Action<GraphEdge> onEdgeAdded = null,
      Action<GraphEdge> onEdgeRemoved = null)
    {
      if (vertices.ContainsKey(id)) throw new ArgumentException("Vertex with same id already exists.");

      var vertex = new GraphVertex(id, label, onNeighborAdded, onNeighborRemoved, onEdgeAdded, onEdgeRemoved);
      vertices.Add(id, vertex);
      return vertex;
    }

    public void RemoveVertex(GraphVertex vertex)
    {
      if (!vertices.ContainsKey(vertex.Id)) throw new ArgumentException("Vertex does not exist.");

      vertices.Remove(vertex.Id);
      vertex.Destroy();
    }

    public GraphEdge AddEdge(GraphVertex from, GraphVertex to, float weight = 1f, Action<GraphEdge> onWeightChanged = null)
    {
      var edge = new GraphEdge(from, to, weight, onWeightChanged);
      edges.Add(edge);
      return edge;
    }
    
    public GraphEdge AddEdge(int fromId, int toId, float weight = 1f, Action<GraphEdge> onWeightChanged = null)
    {
      if (!vertices.ContainsKey(fromId)) throw new ArgumentException("Vertex with id " + fromId + " does not exist.");
      if (!vertices.ContainsKey(toId)) throw new ArgumentException("Vertex with id " + toId + " does not exist.");
      
      var edge = new GraphEdge(vertices[fromId], vertices[toId], weight, onWeightChanged);
      edges.Add(edge);
      return edge;
    }

    public void RemoveEdge(GraphEdge edge)
    {
      edges.Remove(edge);
      edge.Destroy();
    }

    public GraphEdge GetEdge(GraphVertex from, GraphVertex to)
    {
      return edges.Find(e => e.From == from && e.To == to);
    }

    public GraphEdge GetEdge(int fromId, int toId)
    {
      return edges.Find(e => e.From.Id == fromId && e.To.Id == toId);
    }

    public GraphVertex GetVertex(int id)
    {
      return vertices[id];
    }

    public void Clear()
    {
      vertices.Clear();
      edges.Clear();
    }
  }
}