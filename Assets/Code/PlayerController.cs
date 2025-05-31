using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const string MECHA_TAG = "Mecha";

    [SerializeField] private float _speed;
    
    private Rigidbody _rigidbody;
    private BoxCollider _collider;
    private SpriteRenderer _spriteRenderer;

    Vector3 _movementDirection = Vector3.zero;

    private GameObject _mecha = null;
    private bool _usingMecha = false;

    private void Start()
    {
        this._rigidbody = this.GetComponent<Rigidbody>();
        this._collider = this.GetComponent<BoxCollider>();
        this._spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        /* Input */

        if (Input.GetKeyDown(KeyCode.F) && this._mecha != null)
        {
            this._usingMecha = GameManager.Instance.UseMecha(this._mecha);
            this._spriteRenderer.enabled = !this._usingMecha;
            this._collider.enabled = !this._usingMecha;
        }
        
        if (!this._usingMecha)
        {
            this._movementDirection.x = Input.GetAxis("Horizontal") * 1.2f;
            this._movementDirection.y = Input.GetAxis("Vertical");

            this._rigidbody.linearVelocity = this._movementDirection * this._speed;
        }
        else
        {
            this.transform.position = this._mecha.transform.position;
        }

        /* Sprites */
        this._spriteRenderer.flipX = this._rigidbody.linearVelocity.x < 0f ? false : (this._rigidbody.linearVelocity.x > 0f ? true : this._spriteRenderer.flipX);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(MECHA_TAG) && this._mecha == null)
        {
            this._mecha = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(MECHA_TAG) && this._mecha == other)
        {
            this._mecha = null;
        }
    }
}
