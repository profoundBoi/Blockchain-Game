using System.Collections.Generic;
using UnityEngine;

public class KeyInventory : MonoBehaviour
{
    private HashSet<string> collectedKeys = new HashSet<string>();

    public void CollectKey(string keyID)
    {
        if (string.IsNullOrEmpty(keyID))
            return;

        if (collectedKeys.Contains(keyID))
            return;

        collectedKeys.Add(keyID);

        Debug.Log("Collected Key: " + keyID);
    }

    public bool HasKey(string keyID)
    {
        return collectedKeys.Contains(keyID);
    }

    public int GetCollectedKeyCount(List<string> requiredKeys)
    {
        int count = 0;

        foreach (string key in requiredKeys)
        {
            if (collectedKeys.Contains(key))
            {
                count++;
            }
        }

        return count;
    }
}