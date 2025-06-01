using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject _groundBlockPrefab;

    [SerializeField] private float _minCameraDistance;
    [SerializeField] private float _maxCameraDistance;

    private Transform _playerTransform;

    float _zoomTime = 0f;
    
    private void Awake()
    {
        if (Instance != null)
        {
            GameObject.Destroy(this.gameObject);
            return;
        }
        
        Instance = this;
    }

    private int _gridHeight = 5;
    private int _gridWidth = 5;
    private Transform[,] _groundGrid;

    private void Start()
    {
        this._playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        this._groundGrid = new Transform[this._gridWidth, this._gridHeight];

        for (int y = 0; y < this._gridHeight; y++)
        {
            for (int x = 0; x < this._gridWidth; x++)
            {
                this._groundGrid[x, y] = GameObject.Instantiate(this._groundBlockPrefab, new Vector3((x - (int)(this._gridWidth / 2)) * 10f, (y - (int)(this._gridHeight / 2)) * 10f, 1f), Quaternion.identity).transform;
            }
        }
    }

    private void Update()
    {
        this._zoomTime += Time.deltaTime * (this._usingMecha ? 1 : -1);
        if (this._zoomTime > 1f) this._zoomTime = 1f;
        if (this._zoomTime < 0f) this._zoomTime = 0f;

        Camera.main.orthographicSize = Mathf.Lerp(this._minCameraDistance, this._maxCameraDistance, _zoomTime);

        if (this._playerTransform == null) { this._playerTransform = GameObject.FindGameObjectWithTag("Player").transform; }

        Vector3 vel = Vector3.zero;
        Vector3 buffer = Camera.main.transform.position;
        buffer.x = this._playerTransform.position.x;
        buffer.y = this._playerTransform.position.y + .5f;

        Camera.main.transform.position = transform.position = Vector3.SmoothDamp(Camera.main.transform.position, buffer, ref vel, 25f * Time.deltaTime);

        for (int y = 0; y < this._gridHeight; y++)
        {
            for (int x = 0; x < this._gridWidth; x++)
            {
                Vector3 pos = this._groundGrid[x, y].position;
                Vector3 direction = (this._playerTransform.position - this._groundGrid[x, y].position);

                if (Mathf.Abs(this._playerTransform.position.x - this._groundGrid[x, y].position.x) > 25)
                {
                    pos.x += (direction.x / Mathf.Abs(direction.x)) * 50;
                }

                if (Mathf.Abs(this._playerTransform.position.y - this._groundGrid[x, y].position.y) > 25)
                {
                    pos.y += (direction.y / Mathf.Abs(direction.y)) * 50;
                }

                this._groundGrid[x, y].position = pos;
            }
        }
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
