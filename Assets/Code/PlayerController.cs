using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const string MECHA_TAG = "Mecha";

    [SerializeField] private float _speed;
    [SerializeField] private float _shootDelay;

    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _leftBulletSpawn;
    [SerializeField] private Transform _rightBulletSpawn;

    [SerializeField] private Transform _mechaIndicator;

    private Rigidbody _rigidbody;
    private BoxCollider _collider;
    private SpriteRenderer _playerSpriteRenderer;
    private SpriteRenderer _feetSpriteRenderer;

    Vector3 _movementDirection = Vector3.zero;

    private GameObject _mecha;
    private BoxCollider _mechaCollider;
    private bool _mechaInRange;
    private bool _usingMecha = false;

    private float _timer;

    private void Start()
    {
        this._rigidbody = this.GetComponent<Rigidbody>();
        this._collider = this.GetComponent<BoxCollider>();
        this._playerSpriteRenderer = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        this._feetSpriteRenderer = this.transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        this._timer += Time.deltaTime;
        if (this._timer >= this._shootDelay) { this._timer = this._shootDelay; }

        /* Input */

        if (Input.GetKeyDown(KeyCode.F) && this._mechaInRange)
        {
            this._usingMecha = GameManager.Instance.UseMecha(this._mecha);
            this._playerSpriteRenderer.enabled = !this._usingMecha;
            this._feetSpriteRenderer.enabled = !this._usingMecha;
            Physics.IgnoreCollision(this._collider, this._mechaCollider, this._usingMecha);
            this._mechaIndicator.gameObject.SetActive(!this._usingMecha);
            //this._collider.enabled = !this._usingMecha;
        }
        
        if (!this._usingMecha)
        {
            this._movementDirection.x = Input.GetAxis("Horizontal") * 1.2f;
            this._movementDirection.y = Input.GetAxis("Vertical");

            this._rigidbody.linearVelocity = this._movementDirection * this._speed;

            if (Input.GetKey(KeyCode.Mouse0) && this._timer >= this._shootDelay)
            {
                GameObject newBullet = GameObject.Instantiate(this._bullet, (this._playerSpriteRenderer.flipX ? this._rightBulletSpawn.position : this._leftBulletSpawn.position), Quaternion.identity);
                GameObject.Destroy(newBullet, 2f);
                this._timer = 0;
            }
        }
        else
        {
            this.transform.position = this._mecha.transform.position;
        }

        /* Sprites */
        bool flipFlag = this._rigidbody.linearVelocity.x < 0f ? false : (this._rigidbody.linearVelocity.x > 0f ? true : this._playerSpriteRenderer.flipX);
        this._playerSpriteRenderer.flipX = flipFlag;
        this._feetSpriteRenderer.flipX = flipFlag;
        this._playerSpriteRenderer.sortingOrder = -(int)(this.transform.position.y * 10f);
        this._feetSpriteRenderer.sortingOrder = -(int)(this.transform.position.y * 10f);

        if (this._mecha !=  null)
        {
            this._mechaIndicator.LookAt(this._mecha.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(MECHA_TAG))
        {
            this._mecha = other.gameObject;
            this._mechaCollider = this._mecha.GetComponent<BoxCollider>();
            this._mechaInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(MECHA_TAG))
        {
            this._mechaInRange = false;
        }
    }
}
