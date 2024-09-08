using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;  // Duration of the shake
    public float shakeMagnitude = 0.2f; // Magnitude of the shake
    public float dampingSpeed = 1.0f;   // Speed at which the shake dampens

    private Vector3 initialPosition;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            TriggerShake();
        }
    }
    private void Start()
    {
        // Store the initial camera position
        initialPosition = transform.localPosition;
    }

    public void TriggerShake()
    {
        
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float currentShakeDuration = shakeDuration;

        while (currentShakeDuration > 0)
        {
            
            Vector3 shakeOffset = new Vector3(
                Random.Range(-1f, 1f) * shakeMagnitude,
                0f,  
                Random.Range(-1f, 1f) * shakeMagnitude
            );

            
            transform.localPosition = initialPosition + shakeOffset;

            
            currentShakeDuration -= Time.deltaTime * dampingSpeed;

            
            yield return null;
        }

        
        transform.localPosition = initialPosition;
    }
}
