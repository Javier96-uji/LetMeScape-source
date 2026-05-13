using UnityEngine;
using UnityEngine.UI;

public class InvisibilitySystem : MonoBehaviour
{
    private Renderer[] renderers;
    private Animator animator;

    [SerializeField] private Material invisibleMaterial;
    [SerializeField] private Material normalMaterial;

    [SerializeField] private float timeToInvisible = 1.5f;
    private float quietTimer = 0f;
    public bool isCurrentlyInvisible = false;

    [SerializeField] private Image invisibilityIcon;
    [SerializeField] private float alphaVisible = 0.2f;  // Alpha cuando eres visible
    [SerializeField] private float alphaInvisible = 1.0f;

    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        animator = GetComponentInChildren<Animator>();
        SetIconAlpha(alphaVisible);
    }

    private void Update()
    {
        float speed = animator.GetFloat("Speed");
        EnemyAgent windigoAgent = Object.FindFirstObjectByType<EnemyAgent>(); // Encuentra tu Windigo

        // NO nos volvemos invisibles si estamos cerca del Windigo o si nos esta viendo
        bool isVisibleToEnemy = (windigoAgent != null && Vector3.Distance(transform.position, windigoAgent.transform.position) < 15f && isCurrentlyInvisible == false);
        bool isStill = speed <= 0.1f;

        // Solo contamos tiempo si estamos quietos Y no nos ve el Windigo
        if (isStill && !isVisibleToEnemy)
        {
            quietTimer += Time.deltaTime;
        }
        else
        {
            quietTimer = 0f;
        }

        bool shouldBeInvisible = quietTimer >= timeToInvisible;

        if (shouldBeInvisible && !isCurrentlyInvisible)
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.material = invisibleMaterial;
            }
            isCurrentlyInvisible = true;
            SetIconAlpha(alphaInvisible);
            Debug.Log("Gato invisible");
        }
        else if (!shouldBeInvisible && isCurrentlyInvisible)
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.material = normalMaterial;
            }
            isCurrentlyInvisible = false;
            SetIconAlpha(alphaVisible);
            Debug.Log("Gato visible");
        }
    }
    private void SetIconAlpha(float alpha)
    {
        if (invisibilityIcon == null) return;
        Color c = invisibilityIcon.color;
        c.a = alpha;
        invisibilityIcon.color = c;
    }
}
