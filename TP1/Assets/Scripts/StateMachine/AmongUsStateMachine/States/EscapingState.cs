using Photon.Realtime;
using System;
using UnityEditorInternal;
using UnityEngine;

public class EscapingState : AmongUsState
{
    [SerializeField] private float m_playerDistanceCheckFrequency = 2.0f;
    [SerializeField] private float m_playerDistanceThreshold = 20.0f;
    [SerializeField] private float m_maximumEscapeDestinationDistance = 50.0f;

    private Vector3 m_playerOppositeDirection;
    private float m_timer;
    private bool m_canStopEscaping = false;

    public override void OnEnter()
    {
        Debug.Log("AmongUs entering state: EscapingState\n");

        m_stateMachine.SetHasBeenShot(false);
        m_timer = m_playerDistanceCheckFrequency;
        Vector3 vec = m_stateMachine.CharacterPlayer.transform.position - m_stateMachine.transform.position;
        m_playerOppositeDirection = Vector3.Normalize(vec) * -1.0f;

        SetAgentEscapeDestination();

    }

    private void SetAgentEscapeDestination()
    {
        //vérifier si destination est valide et trouver un point
        Vector3 destination = m_playerOppositeDirection * m_maximumEscapeDestinationDistance;

        m_stateMachine.Agent.SetDestination(destination);
    }

    public override void OnExit()
    {
        Debug.Log("AmongUs exiting state: EscapingState\n");
        m_canStopEscaping = false;
    }

    public override void OnUpdate()
    {
        if (m_timer < 0.0f)
        {
            EvaluatePlayerDistance();
        }
        m_timer -= Time.deltaTime;
    }

    private void EvaluatePlayerDistance()
    {
        if (m_stateMachine.DistanceToPlayer > m_playerDistanceThreshold)
        {
            m_canStopEscaping = true;
        }
    }

    public override void OnFixedUpdate()
    {

    }

    public override bool CanEnter(IState currentState)
    {
        return m_stateMachine.HasBeenShot;
    }

    public override bool CanExit()
    {
        return m_canStopEscaping;
    }
}
