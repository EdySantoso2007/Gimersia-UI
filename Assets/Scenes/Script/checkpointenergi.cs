using UnityEngine;

public class CheckpointEnergi : MonoBehaviour
{
    public PlayerRespawn playerRespawn;
    private bool canCheckpoint = false;

    void Update()
    {
        if (canCheckpoint && Input.GetKeyDown(KeyCode.C))
        {
            if (playerRespawn != null)
            {
                // ? Sekarang tidak reset darah
                playerRespawn.SaveCheckpointOnly(transform.position);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            canCheckpoint = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            canCheckpoint = false;
    }
}





