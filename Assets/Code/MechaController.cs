using UnityEngine;

public class MechaController : MonoBehaviour
{
    [SerializeField] private float _speed;
    private bool _hasPilot = false;

    private Rigidbody _rigidbody;
    private SpriteRenderer[] _spriteRenderer;

    Vector3 _movementDirection = Vector3.zero;

    private void Start()
    {
        this._rigidbody = this.GetComponent<Rigidbody>();
        this._spriteRenderer = this.GetComponentsInChildren<SpriteRenderer>();

        this._rigidbody.mass = int.MaxValue;
    }

    private void Update()
    {
        if (!_hasPilot) return;

        this._movementDirection.x = Input.GetAxis("Horizontal") * 1.1f;
        this._movementDirection.y = Input.GetAxis("Vertical");

        this._rigidbody.linearVelocity = this._movementDirection * this._speed;

        /* Sprites */
        bool flipFlag = this._rigidbody.linearVelocity.x < 0f ? false : (this._rigidbody.linearVelocity.x > 0f ? true : this._spriteRenderer[0].flipX);
        for (int i = 0; i < this._spriteRenderer.Length; i++) { this._spriteRenderer[i].flipX = flipFlag; }
    }

    public void ToggleMecha(bool active)
    {
        this._hasPilot = active;

        this._rigidbody.mass = this._hasPilot ? 1 : int.MaxValue;
        this._rigidbody.linearVelocity = Vector3.zero;
    }
}
