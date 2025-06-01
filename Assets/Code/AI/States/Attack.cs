using UnityEngine;

public class Attack : IState
{
    private EState _nextState;

    public Attack()
    {

    }

    public void OnEnter()
    {
        //this._nextState = EState.Attack;
    }

    public EState OnUpdate()
    {
        return EState.Chase;
    }

    public void OnExit()
    {

    }

    public void OnTriggerEnter(Collider other)
    {

    }
//CAVALINHA
    public void OnTriggerExit(Collider other)
    {

    }
}
