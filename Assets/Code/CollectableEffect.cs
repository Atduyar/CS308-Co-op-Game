using UnityEngine;

//[CreateAssetMenu(fileName = "CollectableEffect", menuName = "Scriptable Objects/CollectableEffect")]
public abstract class CollectableEffect : ScriptableObject
{
    public string collectableName;
    public AudioClip collectSound;
    public ParticleSystem collectEffect;

    public abstract void Apply(GameObject collector);
}
