using UnityEngine;

public class WindigoAttackState : AiState
{
    private float _attackTimer = 0f;
    private const float ATTACK_DURATION = 1.5f;
    private const float KILL_DISTANCE = 2.5f;

    public override void OnStateStarted(EnemyAgent agent)
    {
        Debug.Log("Windigo ataca");
        _attackTimer = 0f;
        agent.GetComponent<Animator>().SetTrigger("AttackTrigger");
        SoundManager.instance.PlayWindigoAttack();
    }

    public override void UpdateState(EnemyAgent agent)
    {
        _attackTimer += Time.deltaTime;
    }

    public override bool IsFinished() => _attackTimer >= ATTACK_DURATION;

    public override AiState GetNextState(EnemyAgent agent)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Logica de "Muerte"
        if (player != null && Vector3.Distance(agent.transform.position, player.transform.position) <= KILL_DISTANCE)
        {
            GameManager.instance.LoseGame(); // Aqui termina el juego
        }

        return new WindigoChaseState(); // Si fallo el ataque vuelve a perseguir
    }

    public override void OnStateEnded(EnemyAgent agent) { }
    public override void OnPlayerSighted(EnemyAgent agent) { }
}
