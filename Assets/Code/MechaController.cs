using UnityEngine;

public class MechaController : MonoBehaviour
{
    [SerializeField] private float _speed;
    private bool _hasPilot = false;

    private Rigidbody _rigidbody;
    private SpriteRenderer[] _spriteRenderer;
    private Animator[] _animator;

    Vector3 _movementDirection = Vector3.zero;

    private void Start()
    {
        this._rigidbody = this.GetComponent<Rigidbody>();
        this._spriteRenderer = this.GetComponentsInChildren<SpriteRenderer>();
        this._animator = this.GetComponentsInChildren<Animator>();

        this._rigidbody.mass = int.MaxValue;
    }

    private void Update()
    {
        this._movementDirection.x = Input.GetAxis("Horizontal") * 1.1f;
        this._movementDirection.y = Input.GetAxis("Vertical");

        if (this._hasPilot) { this._rigidbody.linearVelocity = this._movementDirection * this._speed; }

        /* Sprites */
        bool flipFlag = this._rigidbody.linearVelocity.x < 0f ? false : (this._rigidbody.linearVelocity.x > 0f ? true : this._spriteRenderer[0].flipX);
        for (int i = 0; i < this._spriteRenderer.Length; i++)
        {
            this._spriteRenderer[i].flipX = flipFlag;
            this._spriteRenderer[i].sortingOrder = -(int)(this.transform.position.y * 10f);
        }
        for (int i = 0; i < this._animator.Length; i++) { this._animator[i].SetBool("isWalking", this._rigidbody.linearVelocity.magnitude > 0f); }
    }

    public void ToggleMecha(bool active)
    {
        this._hasPilot = active;

        this._rigidbody.mass = this._hasPilot ? 1 : int.MaxValue;
        this._rigidbody.linearVelocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && this._rigidbody.linearVelocity.magnitude > 0f)
        {
            collision.gameObject.GetComponent<StateMachine>().KillEnemy();
        }
    }
}
