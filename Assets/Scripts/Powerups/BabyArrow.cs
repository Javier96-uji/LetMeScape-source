using System.Collections;
using UnityEngine;

public class BabyArrow : MonoBehaviour
{
    [Tooltip("El personaje encima del que flotara la flecha")]
    public Transform player;

    [Tooltip("Cuanto sube la flecha sobre la cabeza del jugador")]
    public float heightOffset = 2f;
    public Vector3 rotationOffset = Vector3.zero;

    private void Update()
    {
        // 1. Seguimos al jugador
        if (player != null)
            transform.position = player.position + Vector3.up * heightOffset;

        // 2. Buscamos la cria mas cercana
        GameObject[] babies = GameObject.FindGameObjectsWithTag("Baby");

        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject baby in babies)
        {
            if (!baby.activeInHierarchy) continue;

            float d = Vector3.Distance(transform.position, baby.transform.position);
            if (d < minDist)
            {
                minDist = d;
                closest = baby;
            }
        }

        // 3. Si no hay crias, ocultamos la flecha
        if (closest == null)
        {
            gameObject.SetActive(false);
            return;
        }

        // 4. Rotamos hacia la cria en el plano horizontal
        Vector3 dir = closest.transform.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.01f) return;

        transform.rotation = Quaternion.LookRotation(dir) * Quaternion.Euler(rotationOffset);
    }
}