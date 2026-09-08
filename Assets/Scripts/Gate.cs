using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [Header("Required Keys")]
    [SerializeField] private List<string> requiredKeys = new List<string>();

    [Header("Gate Movement")]
    [SerializeField] private Transform gateObject;
    [SerializeField] private float openHeight = 3f;
    [SerializeField] private float openSpeed = 3f;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private bool isOpen = false;

    public int RequiredKeyCount
    {
        get { return requiredKeys.Count; }
    }

    private void Start()
    {
        if (gateObject == null)
            gateObject = transform;

        closedPosition = gateObject.position;
        openPosition = closedPosition + Vector3.up * openHeight;
    }

    private void Update()
    {
        CheckForPlayerKeys();

        if (isOpen)
        {
            gateObject.position = Vector3.MoveTowards(
                gateObject.position,
                openPosition,
                openSpeed * Time.deltaTime
            );
        }
    }

    private void CheckForPlayerKeys()
    {
        if (isOpen)
            return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            KeyInventory inventory =
                player.GetComponent<KeyInventory>();

            if (inventory == null)
                continue;

            if (HasAllKeys(inventory))
            {
                OpenGate();
                return;
            }
        }
    }

    public int GetCollectedKeyCount(KeyInventory inventory)
    {
        if (inventory == null)
            return 0;

        return inventory.GetCollectedKeyCount(requiredKeys);
    }

    public bool HasAllKeys(KeyInventory inventory)
    {
        if (inventory == null)
            return false;

        return GetCollectedKeyCount(inventory) >= requiredKeys.Count;
    }

    public void OpenGate()
    {
        if (isOpen)
            return;

        isOpen = true;

        Collider gateCollider = GetComponent<Collider>();

        if (gateCollider != null)
        {
            gateCollider.enabled = false;
        }

        Debug.Log("Gate opened!");
    }
}