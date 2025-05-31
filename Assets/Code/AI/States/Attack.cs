using UnityEngine;

public class Attack : IState
{
    private EState _nextState;

    public Attack()
    {

    }

    public void OnEnter()
    {
        this._nextState = EState.Attack;
    }

    public EState OnUpdate()
    {
        return this._nextState;
    }

    public void OnExit()
    {

    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void OnTriggerExit(Collider other)
    {

    }
}
