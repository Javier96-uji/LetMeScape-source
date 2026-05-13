using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyAgent : MonoBehaviour
{
    private AiState currentState;
    private Animator _animator;
    private InvisibilitySystem _gatoInv;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        // Empezamos con el estado patrulla
        currentState = new WindigoPatrolState();
        currentState.OnStateStarted(this);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _gatoInv = player.GetComponent<InvisibilitySystem>();
    }

    private void Update()
    {
        float currentSpeed = GetComponent<NavMeshAgent>().velocity.magnitude;
        _animator.SetFloat("Speed", currentSpeed);

        if (currentState != null)
        {
            currentState.UpdateState(this);
            if (currentState.IsFinished())
            {
                currentState.OnStateEnded(this);
                currentState = currentState.GetNextState(this);
                currentState.OnStateStarted(this);
            }
        }
        if (currentState == null)
        {
            Debug.Log("currentState es NULL");
            return;
        }
    }

    // Funcion simple de vision para que el estado la use
    public bool CanSeePlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (!player || _gatoInv.isCurrentlyInvisible) return false;

        float dist = Vector3.Distance(transform.position, player.transform.position);
        return dist < 8f; // Rango de vision de 15 unidades
    }
    public bool IsPlayerCarryingBaby()
    {
        EnemyTracker tracker = UnityEngine.Object.FindFirstObjectByType<EnemyTracker>();
        return tracker != null && tracker.isCarryingBaby;
    }
}
