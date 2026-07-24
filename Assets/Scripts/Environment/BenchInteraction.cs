using UnityEngine;

public class BenchInteraction : MonoBehaviour
{
    private Bench bench;

    private void Start()
    {
        bench = GetComponentInParent<Bench>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.SetCurrentBench(bench);
            }

            if (bench.interactText != null)
            {
                bench.interactText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.ClearCurrentBench();
            }

            if (bench.interactText != null)
            {
                bench.interactText.SetActive(false);
            }
        }
    }
}