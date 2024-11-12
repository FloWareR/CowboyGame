using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu()]
public class AiAgentConfig : ScriptableObject
{
    public float maxTime;
    public float maxDistance;
    public float dieForce;
    public float maxSightDistance;
    public float stoppingDistance;
    public float stoppingDistanceObjective;
    public float damage;
}
