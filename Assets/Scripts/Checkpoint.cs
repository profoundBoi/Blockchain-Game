using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();

        if (respawn == null)
            return;

        respawn.SetSpawnPoint(spawnPoint);

        Debug.Log("Checkpoint activated!");
    }
}