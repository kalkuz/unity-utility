using System;
using System.Collections.Generic;

namespace KalkuzSystems.Utility
{
  /// <summary>
  /// Graph is a graph data structure that stores vertices and edges.
  /// </summary>
  /// <typeparam name="T">The type of data that is stored in the vertices of this graph.</typeparam>
  [Serializable]
  public class Graph<T>
  {
    /// <summary>
    /// Vertices of this graph.
    /// </summary>
    private Dictionary<int, GraphVertex<T>> vertices;
    
    /// <summary>
    /// Edges of this graph.
    /// </summary>
    private List<GraphEdge<T>> edges;

    /// <inheritdoc cref="vertices"/>
    public Dictionary<int, GraphVertex<T>> Vertices => vertices;
    
    /// <inheritdoc cref="edges"/>
    public List<GraphEdge<T>> Edges => edges;

    public Graph()
    {
      vertices = new Dictionary<int, GraphVertex<T>>();
      edges = new List<GraphEdge<T>>();
    }

    /// <summary>
    /// Adds a vertex to this graph.
    /// </summary>
    /// <param name="id">The unique identifier of the vertex.</param>
    /// <param name="label">The name of the vertex.</param>
    /// <param name="data">The object stored in the vertex.</param>
    /// <param name="onNeighborAdded">Event called when a neighbor is added to this vertex.</param>
    /// <param name="onNeighborRemoved">Event called when a neighbor is removed from this vertex.</param>
    /// <param name="onEdgeAdded">Event called when an edge is added to this vertex.</param>
    /// <param name="onEdgeRemoved">Event called when an edge is removed from this vertex.</param>
    /// <returns>The vertex that was added.</returns>
    /// <exception cref="ArgumentException">Thrown when a vertex with the same id already exists.</exception>
    public GraphVertex<T> AddVertex(
      int id,
      string label,
      T data,
      Action<GraphVertex<T>> onNeighborAdded = null,
      Action<GraphVertex<T>> onNeighborRemoved = null,
      Action<GraphEdge<T>> onEdgeAdded = null,
      Action<GraphEdge<T>> onEdgeRemoved = null
    )
    {
      if (vertices.ContainsKey(id)) throw new ArgumentException("Vertex with same id already exists.");

      var vertex = new GraphVertex<T>(id, label, data, onNeighborAdded, onNeighborRemoved, onEdgeAdded, onEdgeRemoved);
      vertices.Add(id, vertex);
      return vertex;
    }

    /// <summary>
    /// Removes a vertex from this graph.
    /// </summary>
    /// <param name="vertex">The vertex to remove.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the vertex does not exist.</exception>
    public void RemoveVertex(GraphVertex<T> vertex)
    {
      if (!vertices.ContainsKey(vertex.Id)) throw new KeyNotFoundException("Vertex does not exist.");

      vertices.Remove(vertex.Id);
      vertex.Disconnect();
    }

    /// <summary>
    /// Adds a directed edge to this graph.
    /// </summary>
    /// <param name="from">The vertex that the edge is starting from.</param>
    /// <param name="to">The vertex that the edge is ending at.</param>
    /// <param name="weight">The weight of the edge.</param>
    /// <param name="onWeightChanged">Event called when the weight of the edge is changed.</param>
    /// <returns>The edge that was added.</returns>
    public GraphEdge<T> AddDirectedEdge(
      GraphVertex<T> from,
      GraphVertex<T> to,
      float weight = 1f,
      Action<GraphEdge<T>> onWeightChanged = null
    )
    {
      var edge = new GraphEdge<T>(from, to, weight, onWeightChanged);
      edges.Add(edge);
      return edge;
    }

    /// <summary>
    /// Adds a directed edge to this graph.
    /// </summary>
    /// <param name="fromId">The id of the vertex that the edge is starting from.</param>
    /// <param name="toId">The id of the vertex that the edge is ending at.</param>
    /// <param name="weight">The weight of the edge.</param>
    /// <param name="onWeightChanged">Event called when the weight of the edge is changed.</param>
    /// <returns>The edge that was added.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when a vertex with the specified id does not exist.</exception>
    public GraphEdge<T> AddDirectedEdge(
      int fromId,
      int toId,
      float weight = 1f,
      Action<GraphEdge<T>> onWeightChanged = null
    )
    {
      if (!vertices.ContainsKey(fromId)) throw new KeyNotFoundException("Vertex with id " + fromId + " does not exist.");
      if (!vertices.ContainsKey(toId)) throw new KeyNotFoundException("Vertex with id " + toId + " does not exist.");

      var edge = new GraphEdge<T>(vertices[fromId], vertices[toId], weight, onWeightChanged);
      edges.Add(edge);
      return edge;
    }

    /// <summary>
    /// Adds an undirected edge to this graph. This creates two directed edges with the same weight and opposite directions to simulate an undirected edge.
    /// </summary>
    /// <param name="from">The vertex that the edge has at one side.</param>
    /// <param name="to">The vertex that the edge has at the other side.</param>
    /// <param name="weight">The weight of the edge.</param>
    /// <param name="onWeightChanged">Event called when the weight of the edge is changed.</param>
    /// <returns>The edges that were added.</returns>
    public (GraphEdge<T>, GraphEdge<T>) AddUndirectedEdge(
      GraphVertex<T> from,
      GraphVertex<T> to,
      float weight = 1f,
      Action<GraphEdge<T>> onWeightChanged = null
    )
    {
      var edge1 = AddDirectedEdge(from, to, weight, onWeightChanged);
      var edge2 = AddDirectedEdge(to, from, weight, onWeightChanged);
      
      edge1.UndirectedSibling = edge2;
      edge2.UndirectedSibling = edge1;
      
      return (edge1, edge2);
    }
    
    /// <summary>
    /// Adds an undirected edge to this graph. This creates two directed edges with the same weight and opposite directions to simulate an undirected edge.
    /// </summary>
    /// <param name="fromId">The id of the vertex that the edge has at one side.</param>
    /// <param name="toId">The id of the vertex that the edge has at the other side.</param>
    /// <param name="weight">The weight of the edge.</param>
    /// <param name="onWeightChanged">Event called when the weight of the edge is changed.</param>
    /// <returns>The edges that were added.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when a vertex with the specified id does not exist.</exception>
    public GraphEdge<T>[] AddUndirectedEdge(
      int fromId,
      int toId,
      float weight = 1f,
      Action<GraphEdge<T>> onWeightChanged = null
    )
    {
      if (!vertices.ContainsKey(fromId)) throw new KeyNotFoundException("Vertex with id " + fromId + " does not exist.");
      if (!vertices.ContainsKey(toId)) throw new KeyNotFoundException("Vertex with id " + toId + " does not exist.");

      var edge1 = AddDirectedEdge(vertices[fromId], vertices[toId], weight, onWeightChanged);
      var edge2 = AddDirectedEdge(vertices[toId], vertices[fromId], weight, onWeightChanged);
      return new[] { edge1, edge2 };
    }

    /// <summary>
    /// Removes an edge from this graph.
    /// </summary>
    /// <param name="edge">The edge to remove.</param>
    public void RemoveEdge(GraphEdge<T> edge)
    {
      edges.Remove(edge);
      edge.Disconnect();
    }

    /// <summary>
    /// Gets the edge between two vertices.
    /// </summary>
    /// <param name="from">The vertex that the edge is expected to start from.</param>
    /// <param name="to">The vertex that the edge is expected to end at.</param>
    /// <returns>The edge between the two vertices.</returns>
    public GraphEdge<T> GetEdge(GraphVertex<T> from, GraphVertex<T> to)
    {
      return edges.Find(e => e.From == from && e.To == to);
    }

    /// <summary>
    /// Gets the edge between two vertices.
    /// </summary>
    /// <param name="fromId">The id of the vertex that the edge is expected to start from.</param>
    /// <param name="toId">The id of the vertex that the edge is expected to end at.</param>
    /// <returns>The edge between the two vertices.</returns>
    public GraphEdge<T> GetEdge(int fromId, int toId)
    {
      return edges.Find(e => e.From.Id == fromId && e.To.Id == toId);
    }

    /// <summary>
    /// Gets the vertex with the specified id.
    /// </summary>
    /// <param name="id">The id of the vertex to get.</param>
    /// <returns>The vertex with the specified id.</returns>
    public GraphVertex<T> GetVertex(int id)
    {
      return vertices[id];
    }

    /// <summary>
    /// Clears this graph.
    /// </summary>
    public void Clear()
    {
      vertices.Clear();
      edges.Clear();
    }

    #region Algorithms
    
    

    #endregion

    #region Describe

    public override string ToString()
    {
      return base.ToString();
    }

    #endregion
  }
}