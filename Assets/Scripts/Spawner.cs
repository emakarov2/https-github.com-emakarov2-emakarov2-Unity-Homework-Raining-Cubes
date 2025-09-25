using UnityEngine.Pool;
using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;

    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 20;

    [SerializeField] private float _spawnRadius = 25f;
    [SerializeField] private float _spawnCooldownInSeconds = 0.5f;

    private Coroutine _spawnCoroutine;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => CreateCube(),
            actionOnGet: (cube) => GetFromPool(cube),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => Destroy(cube),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void OnEnable()
    {
        _spawnCoroutine = StartCoroutine(SpawnCubesPerCooldown());
    }

    private void OnDisable()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }

        _pool?.Clear();
    }

  private IEnumerator SpawnCubesPerCooldown()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnCooldownInSeconds);

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
        
        cube.TimeWasted += OnCubeTimeWasted;

        return cube;
    }

    private void GetFromPool(Cube cube)
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

    private void OnCubeTimeWasted(Cube cube)
    {
        _pool.Release(cube);
    }

    private void Destroy(Cube cube)
    {
        if (cube != null)
        {
            cube.TimeWasted -= OnCubeTimeWasted;

            Destroy(cube.gameObject);
        }
    }
}