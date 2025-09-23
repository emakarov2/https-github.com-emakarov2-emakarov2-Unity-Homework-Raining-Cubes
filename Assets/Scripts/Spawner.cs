using UnityEngine.Pool;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;

    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 20;

    [SerializeField] private float _spawnRadius = 25f;
    [SerializeField] private float _spawnCooldown = 1f;


    private ObjectPool<Cube> _pool;

    private float _spawnTimer;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => CreateCube(),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => Destroy(cube),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Update()
    {
        SpawnCubes();
    }

    private void OnDisable()
    {
        _pool?.Clear();
    }

    private void SpawnCubes()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= _spawnCooldown)
        {
            _spawnTimer = 0f;

            SpawnCube();
        }
    }

    private void SpawnCube()
    {
        _pool.Get();
    }

    private Cube CreateCube()
    {
        Cube cube = Instantiate(_prefab);
        
        cube.TimeWasted += CubeTimeWasted;

        return cube;
    }

    private void ActionOnGet(Cube cube)
    {
        cube.transform.position = CalculateSpawnPosition();

        cube.ResetCube();

        cube.gameObject.SetActive(true);
    }

    private Vector3 CalculateSpawnPosition()
    {
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * _spawnRadius;

        return transform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
    }

    private void CubeTimeWasted(Cube cube)
    {
        _pool.Release(cube);
    }

    private void Destroy(Cube cube)
    {
        if (cube != null)
        {
            cube.TimeWasted -= CubeTimeWasted;

            Destroy(cube.gameObject);
        }
    }
}