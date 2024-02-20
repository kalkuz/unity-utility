using System.Linq;
using UnityEngine;

namespace Kalkuz.Utility
{
  public static class PhysicsExtensions
  {
    /// <summary>
    /// Overlap a ring shaped area
    /// </summary>
    /// <param name="center">The center of the ring</param>
    /// <param name="innerRadius">The inner radius of the ring</param>
    /// <param name="outerRadius">The outer radius of the ring</param>
    /// <param name="layerMask">Layer mask for the overlap</param>
    /// <param name="minDepth">Minimum z-depth of the overlap</param>
    /// <param name="maxDepth">Maximum z-depth of the overlap</param>
    /// <returns>Array of colliders that overlap the ring</returns>
    public static Collider2D[] OverlapRing2D(Vector2 center, float innerRadius, float outerRadius,
      int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity)
    {
      var colliders = Physics2D.OverlapCircleAll(center, outerRadius, layerMask, minDepth, maxDepth);

      var innerRadiusSquared = innerRadius * innerRadius;
      var outerRadiusSquared = outerRadius * outerRadius;

      // check collider bounds is inside ring
      return colliders.Where(collider =>
      {
        var colliderCenter = (Vector2)collider.bounds.center;
        var relativeVector = colliderCenter - center;
        var outerLimit = center + relativeVector.normalized * outerRadius;
        var closestPoint = collider.ClosestPoint(outerLimit);
        var distanceSquared = (closestPoint - center).sqrMagnitude;
        return distanceSquared >= innerRadiusSquared && distanceSquared <= outerRadiusSquared;
      }).ToArray();
    }

    /// <summary>
    /// Overlap a ring shaped area
    /// </summary>
    /// <param name="center">The center of the ring</param>
    /// <param name="innerRadius">The inner radius of the ring</param>
    /// <param name="outerRadius">The outer radius of the ring</param>
    /// <param name="contactFilter">Contact filter for the overlap</param>
    /// <returns>Array of colliders that overlap the ring</returns>
    public static Collider2D[] OverlapRing2D(Vector2 center, float innerRadius, float outerRadius,
      ContactFilter2D contactFilter)
    {
      var colliders = Physics2D.OverlapCircleAll(center, outerRadius, contactFilter.layerMask, contactFilter.minDepth,
        contactFilter.maxDepth);

      var innerRadiusSquared = innerRadius * innerRadius;
      var outerRadiusSquared = outerRadius * outerRadius;

      // check collider bounds is inside ring
      return colliders.Where(collider =>
      {
        var colliderCenter = (Vector2)collider.bounds.center;
        var relativeVector = colliderCenter - center;
        var outerLimit = center + relativeVector.normalized * outerRadius;
        var closestPoint = collider.ClosestPoint(outerLimit);
        var distanceSquared = (closestPoint - center).sqrMagnitude;
        return distanceSquared >= innerRadiusSquared && distanceSquared <= outerRadiusSquared;
      }).ToArray();
    }

    /// <summary>
    /// Overlap a circle sector
    /// </summary>
    /// <param name="center">The center of the circle</param>
    /// <param name="radius">The radius of the circle</param>
    /// <param name="angle">The angle of the sector</param>
    /// <param name="direction">The central direction of the sector</param>
    /// <param name="layerMask">Layer mask for the overlap</param>
    /// <param name="minDepth">Minimum z-depth of the overlap</param>
    /// <param name="maxDepth">Maximum z-depth of the overlap</param>
    /// <returns>Array of colliders that overlap the circle sector</returns>
    public static Collider2D[] OverlapSector2D(Vector2 center, float radius, float angle, Vector2 direction,
      int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity)
    {
      var colliders = Physics2D.OverlapCircleAll(center, radius, layerMask, minDepth, maxDepth);

      var halfAngle = angle * 0.5f;
      var halfAngleCos = Mathf.Cos(halfAngle * Mathf.Deg2Rad);
      var directionNormalized = direction.normalized;

      // check collider bounds is inside cone
      return colliders.Where(collider =>
      {
        var colliderCenter = (Vector2)collider.bounds.center;
        var relativeVector = colliderCenter - center;
        var dot = Vector2.Dot(directionNormalized, relativeVector.normalized);
        var centerSatisfies = dot >= halfAngleCos;

        if (centerSatisfies) return true;

        // check if collider bounds are inside cone with raycast
        var rotatedVector1 = Quaternion.Euler(0, 0, halfAngle) * directionNormalized;
        var rotatedVector2 = Quaternion.Euler(0, 0, -halfAngle) * directionNormalized;
        var raycastHit1 = Physics2D.RaycastAll(center, rotatedVector1, radius, layerMask, minDepth, maxDepth);
        var raycastHit2 = Physics2D.RaycastAll(center, rotatedVector2, radius, layerMask, minDepth, maxDepth);

        return raycastHit1.Any(hit => hit.collider == collider) || raycastHit2.Any(hit => hit.collider == collider);
      }).ToArray();
    }

    /// <summary>
    /// Overlap a circle sector
    /// </summary>
    /// <param name="center">The center of the circle</param>
    /// <param name="radius">The radius of the circle</param>
    /// <param name="angle">The angle of the sector</param>
    /// <param name="direction">The central direction of the sector</param>
    /// <param name="contactFilter">Contact filter for the overlap</param>
    /// <returns>Array of colliders that overlap the circle sector</returns>
    public static Collider2D[] OverlapSector2D(Vector2 center, float radius, float angle, Vector2 direction,
      ContactFilter2D contactFilter)
    {
      var colliders = Physics2D.OverlapCircleAll(center, radius, contactFilter.layerMask, contactFilter.minDepth,
        contactFilter.maxDepth);

      var halfAngle = angle * 0.5f;
      var halfAngleCos = Mathf.Cos(halfAngle * Mathf.Deg2Rad);
      var directionNormalized = direction.normalized;

      // check collider bounds is inside cone
      return colliders.Where(collider =>
      {
        var colliderCenter = (Vector2)collider.bounds.center;
        var relativeVector = colliderCenter - center;
        var dot = Vector2.Dot(directionNormalized, relativeVector.normalized);
        var centerSatisfies = dot >= halfAngleCos;

        if (centerSatisfies) return true;

        // check if collider bounds are inside cone with raycast
        var rotatedVector1 = Quaternion.Euler(0, 0, halfAngle) * directionNormalized;
        var rotatedVector2 = Quaternion.Euler(0, 0, -halfAngle) * directionNormalized;
        var raycastHit1 = Physics2D.RaycastAll(center, rotatedVector1, radius, contactFilter.layerMask,
          contactFilter.minDepth, contactFilter.maxDepth);
        var raycastHit2 = Physics2D.RaycastAll(center, rotatedVector2, radius, contactFilter.layerMask,
          contactFilter.minDepth, contactFilter.maxDepth);

        return raycastHit1.Any(hit => hit.collider == collider) || raycastHit2.Any(hit => hit.collider == collider);
      }).ToArray();
    }
  }
}