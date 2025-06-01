using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private Enemy _enemyData;
    [SerializeField] private float _attackRange = 0.5f;

    private IState[] _states;
    private EState _currentState = EState.Idle;

    private Rigidbody _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private bool _isActive = false;

    public void InitStateMachine(Enemy enemyData)
    {
        this._isActive = true;
        this._enemyData = enemyData;
        this._animator = this.GetComponentInChildren<Animator>();
        this._animator.runtimeAnimatorController = this._enemyData._animatorController;
    }

    private Transform _target;

    private void Start()
    {
        this._states = new IState[]
        {
            new Idle(this.transform),
            new Chase(this.transform),
            //new Attack(),
        };

        // PIRASSUNUNGA
        this._rigidbody = this.GetComponent<Rigidbody>();
        this._spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();

        this._target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (!this._isActive) return;

        EState nextState = this._states[(int)this._currentState].OnUpdate();

        if (this._currentState != nextState)
        {
            this._states[(int)this._currentState].OnExit();
            this._currentState = nextState;
            this._states[(int)this._currentState].OnEnter();
        }

        /* Sprites */
        this._spriteRenderer.flipX = this._rigidbody.linearVelocity.x < 0f ? false : (this._rigidbody.linearVelocity.x > 0f ? true : this._spriteRenderer.flipX);
        this._spriteRenderer.sortingOrder = -(int)(this.transform.position.y * 10f);

        this._animator.SetBool("isWalking", this._rigidbody.linearVelocity.magnitude > 0f);

        if ((this._target.position - this.transform.position).magnitude > 60f)
        {
            this.KillEnemy();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!this._isActive) return;

        if (other.CompareTag("Player"))
        {
            this._states[(int)this._currentState].OnTriggerEnter(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!this._isActive) return;

        if (other.CompareTag("Player"))
        {
            this._states[(int)this._currentState].OnTriggerExit(other);
        }
    }

    private int _enemyLife = 5;

    public void TakeDamage()
    {
        this._enemyLife--;
        if (this._enemyLife <= 0)
        {
            this.KillEnemy();
        }
    }

    public void KillEnemy()
    {
        GameManager.Instance.DecrementEnemyCount(this._enemyData._isBig);
        GameObject newBlood = GameObject.Instantiate(GameManager.Instance.BloodPrefab, this.transform.position, Quaternion.identity);
        Destroy(newBlood, 20f);
        Destroy(this.gameObject);
    }
}
