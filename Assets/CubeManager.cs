using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public GameObject cubePrefab;           // Prefab of the cube to spawn
    public Transform playerHand;            // Position where the object should return (your hand position)
    private GameObject currentCube;         // Reference to the latest created cube

    void Update()
    {
        // Check if the player wants to spawn a new cube (Left controller A button)
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            SpawnCube();
        }

        // Check if the player wants to pull the latest cube to them (Right controller B button)
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            ThrowableObject throwable = currentCube.GetComponent<ThrowableObject>();
            if (throwable != null)
            {
                StartCoroutine(throwable.PullBackToHand());
            }
        }
    }

    private void SpawnCube()
    {
        // Create a new cube at the player's hand position
        if (cubePrefab != null)
        {
            currentCube = Instantiate(cubePrefab, playerHand.position, Quaternion.identity);
            ThrowableObject throwable = currentCube.GetComponent<ThrowableObject>();
            if (throwable != null)
            {
                throwable.playerHand = playerHand;  // Set the hand position for the new cube
            }
        }
    }
}
