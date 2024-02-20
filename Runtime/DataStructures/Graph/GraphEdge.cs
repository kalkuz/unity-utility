using System;
using UnityEngine;

namespace KalkuzSystems.Utility.DataStructures.Graph
{
  /// <summary>
  /// GraphEdge is an edge in a graph that connects two vertices. It is a directed and weighted edge.
  /// </summary>
  /// <typeparam name="T">The type of data that is stored in the vertices that this edge connects.</typeparam>
  [Serializable]
  public class GraphEdge<T>
  {

    /// <summary>
    /// From is the vertex that this edge is starting from.
    /// </summary>
    private GraphVertex<T> from;

    /// <summary>
    /// To is the vertex that this edge is ending at.
    /// </summary>
    private GraphVertex<T> to;

    /// <summary>
    /// Weight is the weight of this edge. It is used for pathfinding.
    /// </summary>
    private float weight;

    /// <summary>
    /// IsLoop is true if the edge is starting and ending at the same vertex.
    /// </summary>
    private bool isLoop;

    /// <summary>
    /// UndirectedSibling is the other edge that is connected to the same two vertices when the graph created undirected edges.
    /// </summary>
    private GraphEdge<T> undirectedSibling;

    /// <summary>
    /// OnWeightChanged is called when the weight of this edge is changed.
    /// </summary>
    private Action<GraphEdge<T>> onWeightChanged;

    #region Properties

    /// <inheritdoc cref="from"/>
    public GraphVertex<T> From => from;

    /// <inheritdoc cref="to"/>
    public GraphVertex<T> To => to;

    /// <inheritdoc cref="weight"/>
    public float Weight
    {
      get => weight;
      set
      {
        weight = value;
        onWeightChanged?.Invoke(this);
      }
    }

    /// <inheritdoc cref="isLoop"/>
    public bool IsLoop => isLoop;

    /// <inheritdoc cref="undirectedSibling"/>
    public GraphEdge<T> UndirectedSibling
    {
      get => undirectedSibling;
      set => undirectedSibling = value;
    }

    /// <inheritdoc cref="onWeightChanged"/>
    public Action<GraphEdge<T>> OnWeightChanged
    {
      get => onWeightChanged;
      set => onWeightChanged = value;
    }

    #endregion

    #region Constructors

    public GraphEdge(GraphVertex<T> from, GraphVertex<T> to, float weight, Action<GraphEdge<T>> onWeightChanged = null)
    {
      this.from = from;
      this.to = to;
      this.weight = weight;
      this.isLoop = from == to;
      this.onWeightChanged = onWeightChanged;

      from.AddNeighbor(to);

      if (isLoop)
      {
        from.AddEdge(this, GraphVertexEdgeRelation.LOOP);
      }
      else
      {
        from.AddEdge(this);
        to.AddEdge(this, GraphVertexEdgeRelation.INCOMING);
      }

      this.onWeightChanged?.Invoke(this);
    }

    public GraphEdge(GraphVertex<T> from, GraphVertex<T> to, Action<GraphEdge<T>> onWeightChanged = null)
      : this(from, to, 1, onWeightChanged)
    {
    }

    #endregion

    /// <summary>
    /// Gets the other node connected to the edge rather than the specified vertex
    /// </summary>
    /// <param name="vertex">The vertex to check</param>
    /// <returns>The other vertex. Returns the given vertex if the edge is a loop.</returns>
    public GraphVertex<T> GetOtherNode(GraphVertex<T> vertex)
    {
      if (isLoop) return vertex;

      if (from == vertex) return to;
      if (to == vertex) return from;
      return null;
    }

    /// <summary>
    /// Checks if the edge is connected to the vertex
    /// </summary>
    /// <param name="vertex">The vertex to check</param>
    /// <returns>True if the edge is connected to the vertex, false otherwise.</returns>
    public bool IsConnectedTo(GraphVertex<T> vertex)
    {
      return from == vertex || to == vertex;
    }

    /// <summary>
    /// Checks if the edge is connected to the two vertices
    /// </summary>
    /// <param name="from">First vertex</param>
    /// <param name="to">Second vertex</param>
    /// <returns>True if the edge is connected to the two vertices, false otherwise.</returns>
    public bool IsConnectedTo(GraphVertex<T> from, GraphVertex<T> to)
    {
      return this.from == from && this.to == to || this.from == to && this.to == from;
    }

    /// <summary>
    /// Checks if the edge is the same as the other edge, but in the opposite direction
    /// </summary>
    /// <param name="edge">The edge to compare to</param>
    /// <returns>True if the edge is the directional opposite of the other edge, false otherwise.</returns>
    public bool IsOppositeDirection(GraphEdge<T> edge)
    {
      return from == edge.to && to == edge.from;
    }

    /// <summary>
    /// Checks if the edge is in the same direction
    /// </summary>
    /// <param name="edge">The edge to compare to</param>
    /// <returns>True if the edge is in the same direction as the other edge, false otherwise.</returns>
    public bool IsSameDirection(GraphEdge<T> edge)
    {
      return from == edge.from && to == edge.to;
    }

    /// <summary>
    /// Checks if the edge is the same as the other edge, but in the opposite direction considering the weight
    /// </summary>
    /// <param name="edge">The edge to compare to</param>
    /// <returns>True if the edge is the exact opposite of the other edge, false otherwise.</returns>
    public bool IsOpposite(GraphEdge<T> edge)
    {
      return IsOppositeDirection(edge) && Mathf.Abs(weight - edge.weight) < 0.0001f;
    }

    /// <summary>
    /// Disconnects the edge from the vertices it is connected to.
    /// If the vertices are not connected to any other edges, they will be disconnected from each other.
    /// </summary>
    public void Disconnect()
    {
      from.RemoveEdge(this);
      if (!from.OutgoingEdges.Exists(edge => edge.IsConnectedTo(to)))
      {
        from.RemoveNeighbor(to);
      }

      to.RemoveEdge(this);
      if (!to.OutgoingEdges.Exists(edge => edge.IsConnectedTo(from)))
      {
        to.RemoveNeighbor(from);
      }

      from = null;
      to = null;
      
      undirectedSibling = null;
    }

    public override string ToString()
    {
      return $"Graph Edge from {from} to {to} with weight {weight}";
    }
    
    protected bool Equals(GraphEdge<T> other)
    {
      return Equals(from, other.from) && Equals(to, other.to) && weight.Equals(other.weight) && isLoop == other.isLoop && Equals(undirectedSibling, other.undirectedSibling) && Equals(onWeightChanged, other.onWeightChanged);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      return obj.GetType() == this.GetType() && Equals((GraphEdge<T>)obj);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(from, to, weight, isLoop, undirectedSibling, onWeightChanged);
    }

    public static bool operator ==(GraphEdge<T> a, GraphEdge<T> b)
    {
      if (ReferenceEquals(a, b)) return true;
      if (ReferenceEquals(a, null)) return false;
      if (ReferenceEquals(b, null)) return false;
      return a.from == b.from && a.to == b.to;
    }

    public static bool operator !=(GraphEdge<T> a, GraphEdge<T> b)
    {
      return !(a == b);
    }
  }
}