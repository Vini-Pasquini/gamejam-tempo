using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject _groundBlockPrefab;
    [SerializeField] private GameObject _smallEnemyPrefab;
    [SerializeField] private GameObject _bigEnemyPrefab;

    [SerializeField] private Enemy[] _smallEnemies;
    [SerializeField] private Enemy[] _bigEnemies;

    [SerializeField] private float _minCameraDistance;
    [SerializeField] private float _maxCameraDistance;

    [SerializeField] private TextMeshProUGUI _pontosNegocio;

    [SerializeField] private GameObject _bloodPrefab;

    public GameObject BloodPrefab { get { return this._bloodPrefab; } }

    private GameObject[] _buildingsPrefab;
    private GameObject[,] _buildings;

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
        this._buildingsPrefab = Resources.LoadAll<GameObject>("Buildings");

        this._groundGrid = new Transform[this._gridWidth, this._gridHeight];
        this._buildings = new GameObject[this._gridWidth, this._gridHeight];

        for (int y = 0; y < this._gridHeight; y++)
        {
            for (int x = 0; x < this._gridWidth; x++)
            {
                this._groundGrid[x, y] = GameObject.Instantiate(this._groundBlockPrefab, new Vector3((x - (int)(this._gridWidth / 2)) * 10f, (y - (int)(this._gridHeight / 2)) * 10f, 1f), Quaternion.identity).transform;
                this.SpawnBuilding(x, y);
            }
        }
    }

    private float _spawnTimer = 0f;
    private int _enemyCount = 0;

    private int _pontos = 0;

    public void DecrementEnemyCount(bool isBig)
    {
        this._enemyCount--;
        if (this._enemyCount < 0) this._enemyCount = 0;

        this._pontos += isBig ? 13 : 6;

        this._pontosNegocio.text = $"Pontos: {this._pontos}";
    }

    private void Update()
    {
        this._spawnTimer += Time.deltaTime;
        if (this._enemyCount < 500 && this._spawnTimer > .1f)
        {
            this._enemyCount++;
            this._spawnTimer = 0f;
            bool isBig = Random.Range(0f, 100f) <= 33f;
            GameObject newEnemy = GameObject.Instantiate(isBig ? this._bigEnemyPrefab : this._smallEnemyPrefab, this._playerTransform.position + (new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized * 20f), Quaternion.identity);
            newEnemy.GetComponent<StateMachine>().InitStateMachine(isBig ? this._bigEnemies[Random.Range(0, this._bigEnemies.Length)] : this._smallEnemies[Random.Range(0, this._smallEnemies.Length)]);
        }


        this._zoomTime += Time.deltaTime * (this._usingMecha ? 1 : -1);
        if (this._zoomTime > 1f) this._zoomTime = 1f;
        if (this._zoomTime < 0f) this._zoomTime = 0f;

        Camera.main.orthographicSize = Mathf.Lerp(this._minCameraDistance, this._maxCameraDistance, _zoomTime);

        if (this._playerTransform == null) { this._playerTransform = GameObject.FindGameObjectWithTag("Player").transform; }

        Vector3 buffer = Camera.main.transform.position;
        buffer.x = this._playerTransform.position.x;
        buffer.y = this._playerTransform.position.y + .5f;
        Camera.main.transform.position = buffer;

        for (int y = 0; y < this._gridHeight; y++)
        {
            for (int x = 0; x < this._gridWidth; x++)
            {
                Vector3 pos = this._groundGrid[x, y].position;
                Vector3 direction = (this._playerTransform.position - this._groundGrid[x, y].position);
                bool buildingFlag = false;

                if (Mathf.Abs(this._playerTransform.position.x - this._groundGrid[x, y].position.x) > 25)
                {
                    pos.x += (direction.x / Mathf.Abs(direction.x)) * 50;
                    buildingFlag = true;
                }

                if (Mathf.Abs(this._playerTransform.position.y - this._groundGrid[x, y].position.y) > 25)
                {
                    pos.y += (direction.y / Mathf.Abs(direction.y)) * 50;
                    buildingFlag = true;
                }

                this._groundGrid[x, y].position = pos;

                if (buildingFlag)
                {
                    this.SpawnBuilding(x, y);
                }
            }
        }
    }

    private void SpawnBuilding(int x, int y)
    {
        if (this._groundGrid[x, y].position.x == 0 && this._groundGrid[x, y].position.y == 0) return;

        if (this._buildings[x, y] != null) Destroy(this._buildings[x, y]);
        this._buildings[x, y] = GameObject.Instantiate(this._buildingsPrefab[Random.Range(0, this._buildingsPrefab.Length)], this._groundGrid[x, y].position, Quaternion.identity);
        this._buildings[x, y].GetComponent<SpriteRenderer>().sortingOrder = -(int)(this._buildings[x, y].transform.position.y * 10f); ;
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
