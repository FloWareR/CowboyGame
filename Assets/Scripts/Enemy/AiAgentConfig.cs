using UnityEngine;

[CreateAssetMenu()]
public class AiAgentConfig : ScriptableObject
{
    public float maxTime;
    public float maxDistance;
    public float dieForce;
    public float maxSightDistance;
}
