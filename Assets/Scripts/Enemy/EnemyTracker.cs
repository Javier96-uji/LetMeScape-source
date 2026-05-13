using UnityEngine;
using UnityEngine.UI;

public class EnemyTracker : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform windigo; // El Windigo
    [SerializeField] private float detectionRange = 5f;
    private Outline windigoOutline;

    public bool isCarryingBaby = false;

    private void Start()
    {
        if (windigo != null)
        {
            // Buscamos el componente Outline. Si no existe, se lo pone)
            windigoOutline = windigo.GetComponent<Outline>();
            if (windigoOutline == null) windigoOutline = windigo.gameObject.AddComponent<Outline>();

            windigoOutline.enabled = false; // Empieza apagado
        }
    }

    private void Update()
    {
        if (!isCarryingBaby)
        {
            windigoOutline.enabled = false;
            return;
        }
        if (windigo == null) return;

        float distance = Vector3.Distance(transform.position, windigo.position);

        // Si el Windigo esta cerca, encendemos el Outline
        if (distance < detectionRange)
        {
            windigoOutline.enabled = true;
        }
        else
        {
            windigoOutline.enabled = false;
        }
    }
}
