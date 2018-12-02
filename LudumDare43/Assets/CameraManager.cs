using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance = null; // Reference to instance which allows it to be accessed by any other script
    public float shakeDuration = 0f;    // How long the object should shake for
    public float shakeAmount = 0.7f;    // Amplitude of the shake. A larger value shakes the camera harder
    public float decreaseFactor = 1.0f; // Shake decrease time

    private Vector3 originalPos;                // Stores camera's coordinates before shakes and follows
    private Vector3 offset;             // Offset distance between the player and camera     
    public float smoothSpeed = 0.125f;  // How smoothly camera follows Player
    public int cameraZ = -10;           // Distance of the camera from the world

    void Awake()
    {
        // Create static instance of this manager and control singleton pattern
        if (instance == null)
            instance = this;
        // This enforces our singleton pattern, meaning there can only ever be one instance of a this manager
        else if (instance != this)
            Destroy(gameObject);
        // Store default pos
        originalPos = transform.localPosition;
    }

    void Update()
    {
        // Shake
        if (shakeDuration > 0)
        {
            // Randomize new position inside sphere shape, which size is calculated using shakeAmount
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount * Time.deltaTime;
            // Calculate time to stop shaking
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            transform.localPosition = originalPos;
        }
    }

}
