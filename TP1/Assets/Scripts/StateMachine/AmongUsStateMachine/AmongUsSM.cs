using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AmongUsSM : StateMachine<AmongUsState>
{
    


    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<AmongUsState>();
        //m_possibleStates.Add(new PatrolState());
        //m_possibleStates.Add(new PlayerFollowState());
        


    }

    protected override void Awake()
    {
        base.Awake();
        
    }

    protected override void Start()
    {        

        foreach (AmongUsState state in m_possibleStates)
        {
            
            state.OnStart(this);
        }

        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();
        
        
    }

    protected override void Update()
    {
        base.Update();
                
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();        
    }
    
}




