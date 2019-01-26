using UnityEngine;

[System.Serializable]
public struct StructureConfig
{
    public float structureHP;
    public float structureGravityMultiplier;

    public bool damageable; // Floor can't be damaged
    public bool triggerByDefault;
    public RigidbodyType2D defaultRBType;
}