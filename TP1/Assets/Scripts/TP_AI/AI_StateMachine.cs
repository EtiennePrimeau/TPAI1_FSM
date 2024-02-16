using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public enum EStates
{
    Patrol,
    Attack,
    Flee
}

public class AI_StateMachine : MonoBehaviour
{
    [SerializeField] private float m_speed;
    [SerializeField] private float m_visionRange;
    [SerializeField] private float m_minPatrolDistance;
    [SerializeField] private float m_maxPatrolDistance;
    [SerializeField] private float m_fleeDistance;
    [SerializeField] private float m_searchTimerReset;

    private float m_searchTimer;
    private Vector3 m_registeredPlayerPosition;
    private Vector3 m_targetPosition;
    private EStates m_currentState;
    private bool m_receivedDamage = false;


    void Start()
    {
        m_currentState = EStates.Patrol;
    }

    void Update()
    {
        if (m_receivedDamage)
        {
            ChangeState(EStates.Flee);
        }

        switch (m_currentState)
        {
            case EStates.Patrol:
                Patrol();
                break;
            case EStates.Attack:
                Attack();
                break;
            case EStates.Flee:
                Flee();
                break;
            default:
                break;
        }

        if (m_searchTimer < 0)
        {
            m_searchTimer = m_searchTimerReset;
            SearchForPlayer();
        }
        m_searchTimer -= Time.deltaTime;
    }

    private void ChangeState(EStates newState)
    {
        Debug.Log("Changed state from " + m_currentState + " to " + newState);

        if (m_currentState == EStates.Flee)
        {
            m_receivedDamage = false;
        }
        if (newState == EStates.Patrol)
        {
            m_registeredPlayerPosition = Vector3.zero;
        }

        m_currentState = newState;
    }

    //Move randomly
    private void Patrol()
    {
        if (Vector3.Distance(m_targetPosition, transform.position) < 1.0f)
        {
            Vector2 targetPosition2D = Random.insideUnitCircle * Random.Range(m_minPatrolDistance, m_maxPatrolDistance);
            m_targetPosition = new Vector3(targetPosition2D.x, transform.position.y, targetPosition2D.y);
        }

        transform.position = Vector3.MoveTowards(transform.position, m_targetPosition, m_speed);
    }

    //Move directly towards player
    private void Attack()
    {
        transform.position = Vector3.MoveTowards(transform.position, m_registeredPlayerPosition, m_speed);
    }

    //Move away from player (if damaged)
    private void Flee()
    {
        if (Vector3.Distance(m_registeredPlayerPosition, transform.position) > m_fleeDistance)
        {
            ChangeState(EStates.Patrol);
            return;
        }

        Vector3 playerDirection = m_registeredPlayerPosition - transform.position;
        m_targetPosition = Vector3.Normalize(playerDirection) * m_fleeDistance;

        transform.position = Vector3.MoveTowards(transform.position, m_targetPosition, m_speed);
    }

    private void SearchForPlayer()
    {
        var surroundingObjects = Physics.OverlapSphere(transform.position, m_visionRange);

        foreach (var obj in surroundingObjects)
        {
            //Find a component unique to the player
            var player = obj.GetComponent<FirstPersonController>();
            if (player != null)
            {
                Debug.Log("Found player ! Attacking !!");

                ChangeState(EStates.Attack);
                m_registeredPlayerPosition = player.transform.position;
                return;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if collision is bullet

        Debug.Log("Getting shot ! Escaping !!");
        m_receivedDamage = true;
    }
}
