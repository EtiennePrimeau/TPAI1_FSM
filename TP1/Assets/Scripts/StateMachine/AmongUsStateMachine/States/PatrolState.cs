using UnityEngine;

public class PatrolState : AmongUsState
{
    private Transform m_currentPatrolObjective;
    private float m_movementThreshold = 1.0f;

    public override void OnEnter()
    {
        Debug.Log("AmongUs entering state: PatrolState\n");

        SetNextPatrolObjective();
    }

    public override void OnExit()
    {
        Debug.Log("AmongUs exiting state: PatrolState\n");
    }

    public override void OnUpdate()
    {
        MoveTowardsObjective();
    }

    public override void OnFixedUpdate()
    {

    }

    public override bool CanEnter(IState currentState)
    {
        return true;
    }

    public override bool CanExit()
    {
        if (m_stateMachine.CharacterPlayer == null)
        {
            return false;
        }

        return m_stateMachine.DistanceToPlayer < 20;
    }

    private void SetNextPatrolObjective()
    {
        if (m_currentPatrolObjective == m_stateMachine.PatrolPointOne.transform)
        {
            m_currentPatrolObjective = m_stateMachine.PatrolPointTwo.transform;
        }
        else
        {
            m_currentPatrolObjective = m_stateMachine.PatrolPointOne.transform;
        }
    }

    private void MoveTowardsObjective()
    {
        if (Vector3.Distance(m_stateMachine.transform.position, m_currentPatrolObjective.position) < m_movementThreshold)
        {
            SetNextPatrolObjective();
        }

        m_stateMachine.Agent.SetDestination(m_currentPatrolObjective.position);
    }
}
