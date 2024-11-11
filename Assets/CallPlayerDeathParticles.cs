using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ChildCollisionDetector : MonoBehaviour
{
    private ParticleManager _particleManager;
    private List<Vector3> collisionPoints = new List<Vector3>();
    
    void Start()
    {
        _particleManager = FindObjectOfType<ParticleManager>();

        // Get all child colliders (including this object's collider if it has one)
        Collider[] childColliders = GetComponentsInChildren<Collider>();
        
        foreach (Collider childCollider in childColliders)
        {
            // Ensure the collider has a Rigidbody (necessary for OnCollision events)
            if (childCollider.gameObject.GetComponent<Rigidbody>() == null)
            {
                childCollider.gameObject.AddComponent<Rigidbody>().isKinematic = true;
            }
            
            // Subscribe to collision events
            var collisionHandler = childCollider.gameObject.AddComponent<ChildCollisionHandler>();
            collisionHandler.OnChildCollision += HandleChildCollision;
        }
    }

    // This function will be called when any child object collides
    private void HandleChildCollision(Collision collision, GameObject childObject)
    {
        // Check if the object collided with the ground
        if (collision.gameObject.CompareTag("Ground"))
        {

            // Add the collision point to the list
            collisionPoints.Add(collision.GetContact(0).point);

            // If all parts have collided, calculate the center point
            if (collisionPoints.Count >= 3)
            {
                Vector3 centerPosition = CalculateCenterPosition(collisionPoints);
                
                // Spawn the particle at the calculated center position
                _particleManager.SpawnTemporaryParticle("playerDeath", centerPosition, quaternion.identity);

                // Clear the list for future collisions
                collisionPoints.Clear();
            }
        }
    }

    // Calculate the average position of all collision points
    private Vector3 CalculateCenterPosition(List<Vector3> points)
    {
        Vector3 sum = Vector3.zero;
        foreach (Vector3 point in points)
        {
            sum += point;
        }
        return sum / points.Count;
    }
}

// ChildCollisionHandler handles collision detection for each child
public class ChildCollisionHandler : MonoBehaviour
{
    public delegate void CollisionEvent(Collision collision, GameObject childObject);
    public event CollisionEvent OnChildCollision;

    private void OnCollisionEnter(Collision collision)
    {
        OnChildCollision?.Invoke(collision, gameObject);
    }
}
