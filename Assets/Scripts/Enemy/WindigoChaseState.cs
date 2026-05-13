using UnityEngine;
using UnityEngine.AI;
using static AiState;

public class WindigoChaseState : AiState
{
    private NavMeshAgent _navAgent;
    private InvisibilitySystem _gatoInv;
    private Vector3 _lastKnownPosition;

    public override void OnStateStarted(EnemyAgent agent)
    {
        _navAgent = agent.GetComponent<NavMeshAgent>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _gatoInv = player.GetComponent<InvisibilitySystem>();
            _lastKnownPosition = player.transform.position; // Primera posicion
        }

        SoundManager.instance.PlayWindigoRun();
        _navAgent.speed = 4.0f;
    }

    public override void UpdateState(EnemyAgent agent)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        // El Windigo va a la ultima posicion conocida y luego vuelve a patrullar.
        if (_gatoInv != null && _gatoInv.isCurrentlyInvisible)
        {
            _navAgent.SetDestination(_lastKnownPosition);

            if (!_navAgent.pathPending && _navAgent.remainingDistance < 1.0f)
            {
                // Llego a la ultima posicion conocida y vuelve a patrullar
                CurrentInterruptionReason = InterruptionType.PlayerSighted;
            }
            return;
        }

        // Actualizamos la ultima posicion conocida y asignamos el destino una sola vez.
        _lastKnownPosition = player.transform.position;
        _navAgent.SetDestination(_lastKnownPosition);

        float dist = Vector3.Distance(agent.transform.position, player.transform.position);
        if (dist < 2.5f)
        {
            CurrentInterruptionReason = InterruptionType.PlayerDefeat;
        }
    }

    public override bool IsFinished() => CurrentInterruptionReason != InterruptionType.None;
    public override AiState GetNextState(EnemyAgent agent)
    {
        if (CurrentInterruptionReason == InterruptionType.PlayerDefeat)
        {
            return new WindigoAttackState();
        }

        // Si la interrupcion fue por otra cosa, vuelve a patrullar
        return new WindigoPatrolState();
    }
    public override void OnStateEnded(EnemyAgent agent) {
        _navAgent.ResetPath();
        SoundManager.instance.StopLoop();
    }
    public override void OnPlayerSighted(EnemyAgent agent) { }
}
