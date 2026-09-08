using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [Header("Key ID")]
    [SerializeField] private string keyID;

    public string KeyID => keyID;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        KeyInventory inventory = other.GetComponent<KeyInventory>();

        if (inventory == null)
            return;

        inventory.CollectKey(keyID);

        Destroy(gameObject);
    }
}