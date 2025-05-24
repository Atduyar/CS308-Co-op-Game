using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject sparkleEffect;

    private bool isActivated = false;

    public void ActivateCheckpoint()
    {
        if (isActivated) return;
        isActivated = true;

        if (sparkleEffect != null)
            sparkleEffect.SetActive(true);
    }
}
