using UnityEngine;

public class Chase : IState
{
    private EState _nextState;

    private Transform _self;
    private Rigidbody _rigidbody;
    private Transform _target;

    private Vector3 _targetPosition;

    private bool _targetInSight;
    private float _speed = 1.5f;

    public Chase(Transform self)
    {
        this._self = self;
        this._rigidbody = this._self.GetComponent<Rigidbody>();

        this._target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void OnEnter()
    {
        this._nextState = EState.Chase;
        this._targetInSight = true;
    }

    public EState OnUpdate()
    {
        if (!this._targetInSight && (this._targetPosition - this._self.transform.position).magnitude < .5f) return EState.Idle;

        if (this._targetInSight) this._targetPosition = this._target.position;

        this._rigidbody.linearVelocity = (this._targetPosition - this._self.transform.position).normalized * this._speed;

        return this._nextState;
    }

    public void OnExit()
    {

    }

    public void OnTriggerEnter(Collider other) { this._targetInSight = true; }
    public void OnTriggerExit(Collider other) { this._targetInSight = false; }
}
