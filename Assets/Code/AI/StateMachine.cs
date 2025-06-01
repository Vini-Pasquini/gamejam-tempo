using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private float _attackRange = 0.5f;

    private IState[] _states;
    private EState _currentState = EState.Idle;

    private Rigidbody _rigidbody;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        this._states = new IState[]
        {
            new Idle(this.transform),
            new Chase(this.transform),
            new Attack(),
        };

        // PIRASSUNUNGA
        this._rigidbody = this.GetComponent<Rigidbody>();
        this._spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        EState nextState = this._states[(int)this._currentState].OnUpdate();

        if (this._currentState != nextState)
        {
            this._states[(int)this._currentState].OnExit();
            this._currentState = nextState;
            this._states[(int)this._currentState].OnEnter();
        }

        /* Sprites */
        this._spriteRenderer.flipX = this._rigidbody.linearVelocity.x < 0f ? false : (this._rigidbody.linearVelocity.x > 0f ? true : this._spriteRenderer.flipX);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this._states[(int)this._currentState].OnTriggerEnter(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this._states[(int)this._currentState].OnTriggerExit(other);
        }
    }
}
