using UnityEngine;

public class MechaController : MonoBehaviour
{
    [SerializeField] private float _speed;
    private bool _hasPilot = false;

    private Rigidbody _rigidbody;

    Vector3 _movementDirection = Vector3.zero;

    private void Start()
    {
        this._rigidbody = this.GetComponent<Rigidbody>();
        this._rigidbody.mass = int.MaxValue;
    }

    private void Update()
    {
        if (!_hasPilot) return;

        this._movementDirection.x = Input.GetAxis("Horizontal") * 1.1f;
        this._movementDirection.y = Input.GetAxis("Vertical");

        this._rigidbody.linearVelocity = this._movementDirection * this._speed;
    }

    public void ToggleMecha(bool active)
    {
        this._hasPilot = active;

        this._rigidbody.mass = this._hasPilot ? 1 : int.MaxValue;
        this._rigidbody.linearVelocity = Vector3.zero;
    }
}
