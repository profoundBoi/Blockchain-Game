using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Starting Spawn")]
    [SerializeField] private Transform startingSpawnPoint;

    private Transform currentSpawnPoint;

    private Rigidbody rb;
    private PlayerHealth health;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        currentSpawnPoint = startingSpawnPoint;
    }

    public void SetSpawnPoint(Transform newSpawnPoint)
    {
        if (newSpawnPoint == null)
            return;

        currentSpawnPoint = newSpawnPoint;

        Debug.Log("New spawn point set: " + newSpawnPoint.name);
    }

    public void Respawn()
    {
        if (currentSpawnPoint == null)
        {
            Debug.LogWarning("No spawn point assigned!");
            return;
        }

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.position = currentSpawnPoint.position;

        rb.rotation = currentSpawnPoint.rotation;

        health.ResetHealth();

        Debug.Log("Player respawned at: " + currentSpawnPoint.name);
    }
}