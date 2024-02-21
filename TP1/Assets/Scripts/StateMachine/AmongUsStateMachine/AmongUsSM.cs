using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AmongUsSM : StateMachine<AmongUsState>
{
    [field: SerializeField] public Transform StartTransform { get; private set; } = null;
    [field: SerializeField] public GameObject PatrolPointOne { get; private set; } = null;
    [field: SerializeField] public GameObject PatrolPointTwo { get; private set; } = null;
    [field: SerializeField] public GameObject CharacterPlayer { get; private set; } = null;
    [field: SerializeField] public float Speed { get; private set; } = 5.0f;
    [field: SerializeField] public float RotationSpeed { get; private set; } = 5.0f;
    [field: SerializeField] public float DistanceToPlayer { get; private set; } = 0.0f;
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [field: SerializeField] public bool HasBeenShot { get; private set; } = false;
    private GameObject StartPatrolPointObjective { get; set; } = null;


    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<AmongUsState>();
        m_possibleStates.Add(new PatrolState());
        m_possibleStates.Add(new PlayerFollowState());
        m_possibleStates.Add(new EscapingState());
    }

    protected override void Awake()
    {
        base.Awake();

        Agent = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        StartTransform = transform;

        foreach (AmongUsState state in m_possibleStates)
        {

            state.OnStart(this);
        }

        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();
        StartPatrolPointObjective = PatrolPointOne;

    }

    protected override void Update()
    {
        base.Update();

        if (CharacterPlayer == null)
        {
            CharacterPlayer = GameObject.Find("RobotXController(Clone)");
        }

        if (CharacterPlayer == null)
        {
            CharacterPlayer = GameObject.Find("RobotYController(Clone)");
        }

        if (CharacterPlayer == null)
        {
            CharacterPlayer = GameObject.Find("PolicemanController(Clone)");
        }

        if (CharacterPlayer != null)
        {
            DistanceToPlayer = Vector3.Distance(CharacterPlayer.transform.position, transform.position);
            Debug.DrawRay(transform.position, CharacterPlayer.transform.position, Color.red);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void SetHasBeenShot(bool value)
    {
        HasBeenShot = value;
    }
}
