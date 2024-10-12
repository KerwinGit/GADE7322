using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    //public float shakeDuration = 0.5f;  // Duration of the shake
    //public float shakeMagnitude = 0.2f; // Magnitude of the shake
    //public float dampingSpeed = 1.0f;   // Speed at which the shake dampens

    private Vector3 initialPosition;

    private void Start()
    {
        // Store the initial camera position
        initialPosition = transform.localPosition;
    }

    public void TriggerShake(float shakeDuration, float shakeMagnitude, float dampingSpeed)
    {
        
        StartCoroutine(ShakeCoroutine(shakeDuration, shakeMagnitude, dampingSpeed));
    }

    private IEnumerator ShakeCoroutine(float sD, float sM, float dS)
    {
        float currentShakeDuration = sD;

        while (currentShakeDuration > 0)
        {
            
            Vector3 shakeOffset = new Vector3(
                Random.Range(-1f, 1f) * sM,
                0f,  
                Random.Range(-1f, 1f) * sM
            );

            
            transform.localPosition = initialPosition + shakeOffset;

            
            currentShakeDuration -= Time.deltaTime * dS;

            
            yield return null;
        }

        
        transform.localPosition = initialPosition;
    }
}
