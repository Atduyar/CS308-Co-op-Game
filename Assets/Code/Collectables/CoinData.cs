using UnityEngine;

[CreateAssetMenu(fileName = "CoinData", menuName = "Scriptable Objects/CoinData")]
public class CoinData : CollectableEffect
{
    public int scoreAmount = 1;
    public override void Apply(GameObject collector)
    {
        Debug.Log("+1 Coin");
    }
}
