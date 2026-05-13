using Unity.VisualScripting;
using UnityEngine;

public abstract class AiState
{
    public enum InterruptionType { None, PlayerSighted, PlayerDefeat }

    // public para que sea accesible desde otros estados
    public InterruptionType CurrentInterruptionReason { get; set; }

    public abstract void UpdateState(EnemyAgent agent);
    public abstract void OnStateStarted(EnemyAgent agent);
    public abstract void OnStateEnded(EnemyAgent agent);
    public abstract void OnPlayerSighted(EnemyAgent agent);
    public abstract AiState GetNextState(EnemyAgent agent);
    public abstract bool IsFinished();
}
