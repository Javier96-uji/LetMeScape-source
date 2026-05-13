using UnityEngine;
using UnityEngine.AI;
using static AiState;

public class WindigoPatrolState : AiState
{
    private NavMeshAgent _navAgent;
    private GameObject[] _patrolPoints;
    private int _currentIndex = 0;

    public override void OnStateStarted(EnemyAgent agent)
    {
        _navAgent = agent.GetComponent<NavMeshAgent>();
        _patrolPoints = GameObject.FindGameObjectsWithTag("PatrolPoint");
        SoundManager.instance.PlayWindigoWalk();
        if (_navAgent) _navAgent.speed = 2.0f;
    }

    public override void UpdateState(EnemyAgent agent)
    {
        if (_patrolPoints.Length == 0) return;

        _navAgent.SetDestination(_patrolPoints[_currentIndex].transform.position);

        if (!_navAgent.pathPending && _navAgent.remainingDistance < 1.0f)
        {
            _currentIndex = (_currentIndex + 1) % _patrolPoints.Length;
        }

        // Deteccion: Si ve al jugador, salta al estado Chase
        if (agent.CanSeePlayer() || agent.IsPlayerCarryingBaby())
            CurrentInterruptionReason = InterruptionType.PlayerSighted;
    }

    public override void OnStateEnded(EnemyAgent agent) { SoundManager.instance.StopLoop(); }
    public override AiState GetNextState(EnemyAgent agent) => new WindigoChaseState();
    public override bool IsFinished() => CurrentInterruptionReason != InterruptionType.None;
    public override void OnPlayerSighted(EnemyAgent agent) { CurrentInterruptionReason = InterruptionType.PlayerSighted; }
}
