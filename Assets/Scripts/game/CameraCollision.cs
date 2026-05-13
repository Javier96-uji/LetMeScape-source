using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    [Header("Colisión")]
    [SerializeField] private Transform targetObject; // El gato
    [SerializeField] private float minDistance = 0.3f; // Distancia mínima a la cámara
    [SerializeField] private LayerMask collisionLayer;

    private Vector3 originalLocalPos;

    void Start()
    {
        originalLocalPos = transform.localPosition;
    }

    void LateUpdate()
    {
        if (targetObject == null) return;

        Vector3 cameraDirection = (transform.position - targetObject.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, targetObject.position);

        // Raycast desde el gato hacia la cámara
        if (Physics.Raycast(targetObject.position, cameraDirection, out RaycastHit hit, distanceToTarget, collisionLayer))
        {
            // Si choca con algo, mueve la cámara hacia el gato
            float distanceToCollision = hit.distance;
            float newDistance = distanceToCollision - minDistance;

            transform.position = targetObject.position + cameraDirection * newDistance;
        }
    }
}
