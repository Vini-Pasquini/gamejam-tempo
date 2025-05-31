using UnityEngine;

public class Idle : IState
{
    private EState _nextState;

    private Transform _self;
    private Rigidbody _rigidbody;

    public Idle(Transform self)
    {
        this._self = self;
        this._rigidbody = this._self.GetComponent<Rigidbody>();
    }

    public void OnEnter()
    {
        this._nextState = EState.Idle;
        this._rigidbody.linearVelocity = Vector3.zero;
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
        this._nextState = EState.Chase;
    }
    
    public void OnTriggerExit(Collider other)
    {

    }
}
