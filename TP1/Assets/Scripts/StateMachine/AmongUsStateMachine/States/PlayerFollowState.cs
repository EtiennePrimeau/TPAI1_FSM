using UnityEngine;

public class PlayerFollowState : AmongUsState
{

    public override void OnEnter()
    {
        Debug.Log("AmongUs entering state: PlayerFollowState\n");



    }

    public override void OnExit()
    {
        Debug.Log("AmongUs exiting state: PlayerFollowState\n");

    }

    public override void OnUpdate()
    {
        if (m_stateMachine.CharacterPlayer != null)
        {
            MoveTowardsPlayer();
        }

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
        return m_stateMachine.DistanceToPlayer >= 10 || m_stateMachine.HasBeenShot;
    }

    private void MoveTowardsPlayer()
    {
        m_stateMachine.Agent.SetDestination(m_stateMachine.CharacterPlayer.transform.position);
    }
}
