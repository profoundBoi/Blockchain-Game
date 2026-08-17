using UnityEngine;

public class CollectibleCoin : MonoBehaviour
{
    [Header("Coin Settings")]
    public int tokenId = 0;

    private bool hasBeenClaimed = false;

    void OnTriggerEnter(Collider other)
    {
        if (hasBeenClaimed) return;

        if (other.CompareTag("Player"))
        {
            GameUIManager.Instance.ShowClaimPopup(this);
        }
    }

    public void OnClaimed()
    {
        hasBeenClaimed = true;
        gameObject.SetActive(false);
    }
}