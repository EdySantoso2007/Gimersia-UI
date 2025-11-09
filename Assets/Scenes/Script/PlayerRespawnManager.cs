using UnityEngine;

public class PlayerRespawnManager : MonoBehaviour
{
    private Vector3 lastCheckpointPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastCheckpointPosition = transform.position;
    }

    public void UpdateCheckpoint(Vector3 newPosition)
    {
        lastCheckpointPosition = newPosition;
        Debug.Log("Checkpoint Diperbarui ke: " + lastCheckpointPosition);
    }

    public void RespawnPlayer()
    {
        transform.position = lastCheckpointPosition;
        Debug.Log("Pemain di-respawn.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
