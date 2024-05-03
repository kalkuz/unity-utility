using System;
using System.Collections.Generic;

namespace Kalkuz.Utility.DataStructures.Graph
{
  /// <summary>
  /// GraphVertex is a vertex in a graph.
  /// </summary>
  /// <typeparam name="T">The type of data that is stored in this vertex.</typeparam>
  [Serializable]
  public class GraphVertex<T>
  {
    /// <summary>
    /// Id is the unique identifier of this vertex.
    /// </summary>
    private readonly int id;
    
    /// <summary>
    /// Label is the name of this vertex.
    /// </summary>
    private string label;
    
    /// <summary>
    /// Data is the object of type T that is stored in this vertex.
    /// </summary>
    private T data;

    /// <summary>
    /// Incoming edges are edges that are ending at this vertex.
    /// </summary>
    private List<GraphEdge<T>> incomingEdges;
    
    /// <summary>
    /// Outgoing edges are edges that are starting from this vertex.
    /// </summary>
    private List<GraphEdge<T>> outgoingEdges;
    
    /// <summary>
    /// Looping edges are edges that are starting and ending at this vertex.
    /// </summary>
    private List<GraphEdge<T>> loopingEdges;
    
    /// <summary>
    /// Neighbors are vertices that are connected to this vertex with outgoing edges.
    /// </summary>
    private List<GraphVertex<T>> neighbors;

    /// <summary>
    /// OnNeighborAdded is called when a neighbor is added to this vertex.
    /// </summary>
    private Action<GraphVertex<T>> onNeighborAdded;
    
    /// <summary>
    /// OnNeighborRemoved is called when a neighbor is removed from this vertex.
    /// </summary>
    private Action<GraphVertex<T>> onNeighborRemoved;

    /// <summary>
    /// OnEdgeAdded is called when an edge is added to this vertex.
    /// </summary>
    private Action<GraphEdge<T>> onEdgeAdded;
    
    /// <summary>
    /// OnEdgeRemoved is called when an edge is removed from this vertex.
    /// </summary>
    private Action<GraphEdge<T>> onEdgeRemoved;

    #region Properties

    /// <inheritdoc cref="id"/>
    public int Id => id;

    /// <inheritdoc cref="label"/>
    public string Label
    {
      get => label;
      set => label = value;
    }
    
    /// <inheritdoc cref="data"/>
    public T Data
    {
      get => data;
      set => data = value;
    }

    /// <inheritdoc cref="incomingEdges"/>
    public List<GraphEdge<T>> IncomingEdges => incomingEdges;
    
    /// <inheritdoc cref="outgoingEdges"/>
    public List<GraphEdge<T>> OutgoingEdges => outgoingEdges;
    
    /// <inheritdoc cref="loopingEdges"/>
    public List<GraphEdge<T>> LoopingEdges => loopingEdges;
    
    /// <inheritdoc cref="neighbors"/>
    public List<GraphVertex<T>> Neighbors => neighbors;

    /// <inheritdoc cref="onNeighborAdded"/>
    public Action<GraphVertex<T>> OnNeighborAdded
    {
      get => onNeighborAdded;
      set => onNeighborAdded = value;
    }

    /// <inheritdoc cref="onNeighborRemoved"/>
    public Action<GraphVertex<T>> OnNeighborRemoved
    {
      get => onNeighborRemoved;
      set => onNeighborRemoved = value;
    }

    /// <inheritdoc cref="onEdgeAdded"/>
    public Action<GraphEdge<T>> OnEdgeAdded
    {
      get => onEdgeAdded;
      set => onEdgeAdded = value;
    }

    /// <inheritdoc cref="onEdgeRemoved"/>
    public Action<GraphEdge<T>> OnEdgeRemoved
    {
      get => onEdgeRemoved;
      set => onEdgeRemoved = value;
    }

    #endregion

    #region Constructors

    public GraphVertex(int id, string label, T data, Action<GraphVertex<T>> onNeighborAdded = null,
      Action<GraphVertex<T>> onNeighborRemoved = null, Action<GraphEdge<T>> onEdgeAdded = null,
      Action<GraphEdge<T>> onEdgeRemoved = null)
    {
      this.id = id;
      this.label = label;
      this.data = data;

      this.onNeighborAdded = onNeighborAdded;
      this.onNeighborRemoved = onNeighborRemoved;

      this.onEdgeAdded = onEdgeAdded;
      this.onEdgeRemoved = onEdgeRemoved;

      incomingEdges = new List<GraphEdge<T>>();
      outgoingEdges = new List<GraphEdge<T>>();
      loopingEdges = new List<GraphEdge<T>>();
      neighbors = new List<GraphVertex<T>>();
    }

    public GraphVertex(int id, Action<GraphVertex<T>> onNeighborAdded = null,
      Action<GraphVertex<T>> onNeighborRemoved = null, Action<GraphEdge<T>> onEdgeAdded = null,
      Action<GraphEdge<T>> onEdgeRemoved = null) : this(id, id.ToString(), default, onNeighborAdded, onNeighborRemoved, onEdgeAdded, onEdgeRemoved)
    {
    }

    #endregion

    /// <summary>
    /// Adds a neighbor to this vertex. If the neighbor already exists, nothing happens.
    /// </summary>
    /// <param name="vertex">The neighbor to add.</param>
    public void AddNeighbor(GraphVertex<T> vertex)
    {
      if (neighbors.Contains(vertex)) return;
      neighbors.Add(vertex);
      onNeighborAdded?.Invoke(vertex);
    }
    
    /// <summary>
    /// Removes a neighbor from this vertex. If the neighbor does not exist, nothing happens.
    /// </summary>
    /// <param name="vertex">The neighbor to remove.</param>
    public void RemoveNeighbor(GraphVertex<T> vertex)
    {
      if (!neighbors.Contains(vertex)) return;
      neighbors.Remove(vertex);
      onNeighborRemoved?.Invoke(vertex);
    }
    
    /// <summary>
    /// Checks if this vertex has a neighbor.
    /// </summary>
    /// <param name="vertex">The neighbor to check.</param>
    /// <returns>True if the neighbor exists, false otherwise.</returns>
    public bool HasNeighbor(GraphVertex<T> vertex)
    {
      return neighbors.Contains(vertex);
    }
    
    /// <summary>
    /// Adds an edge to this vertex. If the edge already exists, nothing happens.
    /// After adding the edge, neighbors should be added manually.
    /// </summary>
    /// <param name="edge">The edge to add.</param>
    /// <param name="vertexEdgeRelation">The relation between the edge and the vertex.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the vertexEdgeRelation is not valid.</exception>
    public void AddEdge(GraphEdge<T> edge, GraphVertexEdgeRelation vertexEdgeRelation = GraphVertexEdgeRelation.OUTGOING)
    {
      switch (vertexEdgeRelation)
      {
        case GraphVertexEdgeRelation.INCOMING:
          if (incomingEdges.Contains(edge)) return;
          incomingEdges.Add(edge);
          break;
        case GraphVertexEdgeRelation.OUTGOING:
          if (outgoingEdges.Contains(edge)) return;
          outgoingEdges.Add(edge);
          break;
        case GraphVertexEdgeRelation.LOOP:
          if (loopingEdges.Contains(edge)) return;
          loopingEdges.Add(edge);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(vertexEdgeRelation), vertexEdgeRelation, null);
      }
      
      onEdgeAdded?.Invoke(edge);
    }
    
    /// <summary>
    /// Removes an edge from this vertex. If the edge does not exist, nothing happens.
    /// </summary>
    /// <param name="edge">The edge to remove.</param>
    public void RemoveEdge(GraphEdge<T> edge)
    {
      if (incomingEdges.Contains(edge))
      {
        incomingEdges.Remove(edge);
      }
      
      if (outgoingEdges.Contains(edge))
      {
        outgoingEdges.Remove(edge);
      }
      
      if (loopingEdges.Contains(edge))
      {
        loopingEdges.Remove(edge);
      }

      onEdgeRemoved?.Invoke(edge);
    }
    
    /// <summary>
    /// Checks if this vertex has an edge.
    /// </summary>
    /// <param name="edge">The edge to check.</param>
    /// <returns>True if the edge exists, false otherwise.</returns>
    public bool HasEdge(GraphEdge<T> edge)
    {
      return incomingEdges.Contains(edge) || outgoingEdges.Contains(edge) || loopingEdges.Contains(edge);
    }
    
    /// <summary>
    /// Disconnects this vertex from all its neighbors and edges.
    /// </summary>
    public void Disconnect()
    {
      foreach (var edge in incomingEdges)
      {
        edge.Disconnect();
      }
      foreach (var edge in outgoingEdges)
      {
        edge.Disconnect();
      }
      foreach (var edge in loopingEdges)
      {
        edge.Disconnect();
      }
      
      incomingEdges.Clear();
      outgoingEdges.Clear();
      loopingEdges.Clear();
      neighbors.Clear();
    }
    
    public override string ToString()
    {
      return label;
    }

    public override bool Equals(object obj)
    {
      if (obj == null) return false;
      if (obj.GetType() != typeof(GraphVertex<T>)) return false;
      return ((GraphVertex<T>)obj).id == id;
    }

    public override int GetHashCode()
    {
      return id;
    }

    public static bool operator ==(GraphVertex<T> a, GraphVertex<T> b)
    {
      if (ReferenceEquals(a, b)) return true;
      if (ReferenceEquals(a, null)) return false;
      if (ReferenceEquals(b, null)) return false;
      return a.id == b.id;
    }

    public static bool operator !=(GraphVertex<T> a, GraphVertex<T> b)
    {
      return !(a == b);
    }
  }
}