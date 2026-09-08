using UnityEngine;

public class ElectricFence : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float damagePerSecond = 25f;

    [Header("Movement")]
    [SerializeField] private Transform fenceVisual;
    [SerializeField] private float moveDistance = 2f;
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 startPosition;

    private void Start()
    {
        if (fenceVisual == null)
        {
            fenceVisual = transform;
        }

        startPosition = fenceVisual.localPosition;
    }

    private void Update()
    {
        MoveFence();
    }

    private void MoveFence()
    {
        float movement =
            Mathf.Sin(Time.time * moveSpeed) * moveDistance;

        fenceVisual.localPosition =
            startPosition + Vector3.up * movement;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerHealth health =
            other.GetComponent<PlayerHealth>();

        if (health == null)
            return;

        health.TakeDamage(
            damagePerSecond * Time.deltaTime
        );
    }
}