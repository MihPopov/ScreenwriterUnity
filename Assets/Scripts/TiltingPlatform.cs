using UnityEngine;

public class TiltingPlatform : MonoBehaviour
{
    [SerializeField] private Transform platform;
    [SerializeField] private Transform tiltCenter;
    [SerializeField] private float maxTiltAngle = 10f;
    [SerializeField] private float tiltSpeed = 1f;

    private Transform playerOnPlatform;

    void Update()
    {
        if (playerOnPlatform != null)
        {
            Vector3 localPos = platform.InverseTransformPoint(playerOnPlatform.position);
            float tiltX = Mathf.Clamp(localPos.z, -1f, 1f) * maxTiltAngle;
            float tiltZ = -Mathf.Clamp(localPos.x, -1f, 1f) * maxTiltAngle;
            Quaternion targetRotation = Quaternion.Euler(tiltX, 0f, tiltZ);
            platform.localRotation = Quaternion.Lerp(platform.localRotation, targetRotation, Time.deltaTime * tiltSpeed);
            Vector3 tiltNormal = platform.up;
            Vector3 slideDir = Vector3.ProjectOnPlane(Vector3.down, tiltNormal).normalized;
            float slopeAngle = Vector3.Angle(tiltNormal, Vector3.up);
            if (slopeAngle > 5f)
            {
                float slideStrength = Mathf.InverseLerp(0f, 30f, slopeAngle);
                playerOnPlatform.GetComponent<PlayerController>()?.ApplySlide(slideDir * slideStrength * 3f);
            }

        }
        else platform.localRotation = Quaternion.Lerp(platform.localRotation, Quaternion.identity, Time.deltaTime * tiltSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerOnPlatform = other.transform;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.transform == playerOnPlatform) playerOnPlatform = null;
    }
}