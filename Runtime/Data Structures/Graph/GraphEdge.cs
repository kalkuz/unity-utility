using System;

namespace KalkuzSystems.Utility
{
  [Serializable]
  public class GraphEdge
  {
    private readonly GraphVertex from;
    private readonly GraphVertex to;
    private float weight;
    private bool isLoop;
    
    private Action<GraphEdge> onWeightChanged;

    public GraphVertex From => from;
    public GraphVertex To => to;

    public float Weight
    {
      get => weight;
      set
      {
        weight = value;
        onWeightChanged?.Invoke(this);
      }
    }
    
    public bool IsLoop => isLoop;
    
    public Action<GraphEdge> OnWeightChanged
    {
      get => onWeightChanged;
      set => onWeightChanged = value;
    }

    #region Constructors

    public GraphEdge(GraphVertex from, GraphVertex to, float weight, Action<GraphEdge> onWeightChanged = null)
    {
      this.from = from;
      this.to = to;
      this.weight = weight;
      this.isLoop = from == to;
      this.onWeightChanged = onWeightChanged;
      
      from.AddNeighbor(to);
      
      from.AddEdge(this);
      to.AddEdge(this);
      
      this.onWeightChanged?.Invoke(this);
    }

    public GraphEdge(GraphVertex from, GraphVertex to, Action<GraphEdge> onWeightChanged = null) : this(from, to, 1, onWeightChanged)
    {
    }

    #endregion

    public GraphVertex GetOtherNode(GraphVertex vertex)
    {
      if (from == vertex) return to;
      if (to == vertex) return from;
      return null;
    }
    
    public bool IsConnectedTo(GraphVertex vertex)
    {
      return from == vertex || to == vertex;
    }
    
    public bool IsConnectedTo(GraphVertex from, GraphVertex to)
    {
      return this.from == from && this.to == to || this.from == to && this.to == from;
    }
    
    public void Destroy()
    {
      from.RemoveEdge(this);
      to.RemoveEdge(this);

      if (!from.Edges.Exists(edge => edge.IsConnectedTo(to)))
      {
        from.RemoveNeighbor(to);
      }
      if (!to.Edges.Exists(edge => edge.IsConnectedTo(from)))
      {
        to.RemoveNeighbor(from);
      }
    }
    
    public override string ToString()
    {
      return $"Graph Edge from {from} to {to} with weight {weight}";
    }
    
    public override bool Equals(object obj)
    {
      if (obj == null) return false;
      if (obj.GetType() != typeof(GraphEdge)) return false;
      return ((GraphEdge) obj).from == from && ((GraphEdge) obj).to == to;
    }
    
    public override int GetHashCode()
    {
      return from.GetHashCode() ^ to.GetHashCode();
    }
    
    public static bool operator ==(GraphEdge a, GraphEdge b)
    {
      if (ReferenceEquals(a, b)) return true;
      if (ReferenceEquals(a, null)) return false;
      if (ReferenceEquals(b, null)) return false;
      return a.from == b.from && a.to == b.to;
    }
    
    public static bool operator !=(GraphEdge a, GraphEdge b)
    {
      return !(a == b);
    }
  }
}