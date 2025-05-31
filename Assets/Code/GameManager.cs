using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private float _minCameraDistance = 5f;
    [SerializeField] private float _maxCameraDistance = 7f;

    private Transform _playerTransform;

    private float _zoomTime = 0f;
    
    private void Awake()
    {
        if (Instance != null)
        {
            GameObject.Destroy(this.gameObject);
            return;
        }
        
        Instance = this;
    }

    private void Start()
    {
        this._playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        this._zoomTime += Time.deltaTime * (this._usingMecha ? 1 : -1);
        if (this._zoomTime > 1f) this._zoomTime = 1f;
        if (this._zoomTime < 0f) this._zoomTime = 0f;

        Camera.main.orthographicSize = Mathf.Lerp(this._minCameraDistance, this._maxCameraDistance, _zoomTime);

        if (this._playerTransform == null) { this._playerTransform = GameObject.FindGameObjectWithTag("Player").transform; }

        Vector3 buffer = Camera.main.transform.position;

        buffer.x = this._playerTransform.position.x;
        buffer.y = this._playerTransform.position.y + .5f;

        Camera.main.transform.position = buffer;
    }

    /* Mecha */
    private bool _usingMecha = false;
    
    public bool UseMecha(GameObject mecha)
    {
        this._usingMecha = !this._usingMecha;
        mecha.GetComponent<MechaController>().ToggleMecha(this._usingMecha);

        return this._usingMecha;
    }
}
