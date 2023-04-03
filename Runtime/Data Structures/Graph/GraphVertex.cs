using System;
using System.Collections.Generic;

namespace KalkuzSystems.Utility
{
  [Serializable]
  public class GraphVertex
  {
    private readonly int id;
    private string label;

    private List<GraphEdge> edges;
    private List<GraphVertex> neighbors;

    private Action<GraphVertex> onNeighborAdded;
    private Action<GraphVertex> onNeighborRemoved;

    private Action<GraphEdge> onEdgeAdded;
    private Action<GraphEdge> onEdgeRemoved;

    #region Properties

    public int Id => id;

    public string Label
    {
      get => label;
      set => label = value;
    }

    public List<GraphEdge> Edges => edges;
    public List<GraphVertex> Neighbors => neighbors;

    public Action<GraphVertex> OnNeighborAdded
    {
      get => onNeighborAdded;
      set => onNeighborAdded = value;
    }

    public Action<GraphVertex> OnNeighborRemoved
    {
      get => onNeighborRemoved;
      set => onNeighborRemoved = value;
    }

    public Action<GraphEdge> OnEdgeAdded
    {
      get => onEdgeAdded;
      set => onEdgeAdded = value;
    }

    public Action<GraphEdge> OnEdgeRemoved
    {
      get => onEdgeRemoved;
      set => onEdgeRemoved = value;
    }

    #endregion

    #region Constructors

    public GraphVertex(int id, string label, Action<GraphVertex> onNeighborAdded = null,
      Action<GraphVertex> onNeighborRemoved = null, Action<GraphEdge> onEdgeAdded = null,
      Action<GraphEdge> onEdgeRemoved = null)
    {
      this.id = id;
      this.label = label;

      this.onNeighborAdded = onNeighborAdded;
      this.onNeighborRemoved = onNeighborRemoved;

      this.onEdgeAdded = onEdgeAdded;
      this.onEdgeRemoved = onEdgeRemoved;

      edges = new List<GraphEdge>();
      neighbors = new List<GraphVertex>();
    }

    public GraphVertex(int id, Action<GraphVertex> onNeighborAdded = null,
      Action<GraphVertex> onNeighborRemoved = null, Action<GraphEdge> onEdgeAdded = null,
      Action<GraphEdge> onEdgeRemoved = null) : this(id, id.ToString(), onNeighborAdded, onNeighborRemoved, onEdgeAdded, onEdgeRemoved)
    {
    }

    #endregion

    public void AddNeighbor(GraphVertex vertex)
    {
      if (neighbors.Contains(vertex)) return;
      neighbors.Add(vertex);
      onNeighborAdded?.Invoke(vertex);
    }
    
    public void RemoveNeighbor(GraphVertex vertex)
    {
      if (!neighbors.Contains(vertex)) return;
      neighbors.Remove(vertex);
      onNeighborRemoved?.Invoke(vertex);
    }
    
    public void AddEdge(GraphEdge edge)
    {
      if (edges.Contains(edge)) return;
      edges.Add(edge);
      onEdgeAdded?.Invoke(edge);
    }
    
    public void RemoveEdge(GraphEdge edge)
    {
      if (!edges.Contains(edge)) return;
      edges.Remove(edge);
      onEdgeRemoved?.Invoke(edge);
    }
    
    public bool HasNeighbor(GraphVertex vertex)
    {
      return neighbors.Contains(vertex);
    }
    
    public bool HasEdge(GraphEdge edge)
    {
      return edges.Contains(edge);
    }
    
    public void Destroy()
    {
      throw new NotImplementedException("GraphVertex.Destroy() is not implemented yet.");
      foreach (var edge in edges)
      {
        edge.Destroy();
      }
      
      foreach (var neighbor in neighbors)
      {
        neighbor.RemoveNeighbor(this);
      }
      
      edges.Clear();
      neighbors.Clear();
    }
    
    public override string ToString()
    {
      return label;
    }

    public override bool Equals(object obj)
    {
      if (obj == null) return false;
      if (obj.GetType() != typeof(GraphVertex)) return false;
      return ((GraphVertex)obj).id == id;
    }

    public override int GetHashCode()
    {
      return id;
    }

    public static bool operator ==(GraphVertex a, GraphVertex b)
    {
      if (ReferenceEquals(a, b)) return true;
      if (ReferenceEquals(a, null)) return false;
      if (ReferenceEquals(b, null)) return false;
      return a.id == b.id;
    }

    public static bool operator !=(GraphVertex a, GraphVertex b)
    {
      return !(a == b);
    }
  }
}