using UnityEngine;
using System.Collections;

public class ThrowableObject : MonoBehaviour
{
    public Transform playerHand;       // Position where the object returns (your hand position)
    public float pullSpeed = 5f;       // Speed at which object returns to you
    public bool isBeingHeld = false;   // Check if object is currently held
    private Rigidbody rb;              // Rigidbody component for physics
    private Vector3 lastPosition;      // Track the object's position to calculate fling velocity

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Check if the player wants to "force pull" the object back to them
        if (OVRInput.GetDown(OVRInput.Button.Two) && !isBeingHeld)
        {
            StartCoroutine(PullBackToHand());
        }

        // Update the last known position if the object is being held
        if (isBeingHeld)
        {
            lastPosition = transform.position;
        }
    }

    // Called when the object is grabbed
    public void OnGrab()
    {
        isBeingHeld = true;
        rb.isKinematic = true;  // Disable physics while holding
        lastPosition = transform.position;
    }

    // Called when the object is released
    public void OnRelease()
    {
        isBeingHeld = false;
        rb.isKinematic = false;  // Re-enable physics

        // Calculate the fling velocity based on the movement of the hand
        Vector3 flingVelocity = (transform.position - lastPosition) / Time.deltaTime;
        rb.velocity = flingVelocity;
    }

    public IEnumerator PullBackToHand()
    {
        while (!isBeingHeld && Vector3.Distance(transform.position, playerHand.position) > 0.1f)
        {
            // Move the object back towards the player hand
            transform.position = Vector3.MoveTowards(transform.position, playerHand.position, pullSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
