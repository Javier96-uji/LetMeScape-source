using UnityEngine;

public class DropZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        EnemyTracker tracker = Object.FindFirstObjectByType<EnemyTracker>();
        // Si el jugador entra en esta zona, el Windigo se calma
        if (other.CompareTag("Player") && tracker.isCarryingBaby == true)
        {
            if (tracker != null)
            {
                tracker.isCarryingBaby = false; // El Windigo ya no detecta al gato
                Debug.Log("Cria a salvo: El Windigo se ha calmado.");
            }
            if (GameManager.instance != null)
            {
                GameManager.instance.BabyRescued();
                SoundManager.instance.PlayBabyDelivery();
                GameManager.instance.SetCarryingText(false);
            }
        }
    }
}
