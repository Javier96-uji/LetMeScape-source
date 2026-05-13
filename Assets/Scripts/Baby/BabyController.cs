using UnityEngine;

public class BabyController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Avisamos al Windigo
            EnemyTracker tracker = Object.FindFirstObjectByType<EnemyTracker>();
            if (tracker != null)
            {
                tracker.isCarryingBaby = true;
                Debug.Log("Cria recogida");
                GameManager.instance.SetCarryingText(true);
                SoundManager.instance.PlayBabyPickup();
            }

            //Desactivamos la cria
            gameObject.SetActive(false);
        }
    }
}
