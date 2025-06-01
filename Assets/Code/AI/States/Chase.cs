using UnityEngine;

public class Chase : IState
{
    private EState _nextState;

    private Transform _self;
    private Rigidbody _rigidbody;
    private SphereCollider _sphereCollider;

    private Transform _target;

    private Vector3 _targetPosition;

    private bool _targetInSight;
    private float _speed = 1.5f;

    public Chase(Transform self)
    {
        this._self = self;
        this._rigidbody = this._self.GetComponent<Rigidbody>();
        this._sphereCollider = this._self.GetComponent<SphereCollider>();

        this._target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void OnEnter()
    {
        this._nextState = EState.Chase;
        this._targetPosition = this._target.position;
        this._targetInSight = (this._targetPosition - this._self.transform.position).magnitude < this._sphereCollider.radius;
    }

    public EState OnUpdate()
    {
        if (!this._targetInSight && (this._targetPosition - this._self.transform.position).magnitude < .5f) return EState.Idle;

        if (this._targetInSight) this._targetPosition = this._target.position;

        //if ((this._targetPosition - this._self.transform.position).magnitude < .5f) return EState.Attack;

        this._rigidbody.linearVelocity = (this._targetPosition - this._self.transform.position).normalized * this._speed;

        return EState.Chase;
    }

    public void OnExit()
    {

    }

    public void OnTriggerEnter(Collider other) { this._targetInSight = true; }
    public void OnTriggerExit(Collider other) { this._targetInSight = false; }
}
