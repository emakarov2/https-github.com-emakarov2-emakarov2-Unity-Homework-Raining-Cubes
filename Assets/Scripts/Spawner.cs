using UnityEngine.Pool;
using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;

    [SerializeField] private ClickReader _clickReader;

    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 20;

    [SerializeField] private float _spawnRadius = 100f;

    private List<Timer> _activeTimers = new List<Timer>();

    private ObjectPool<GameObject> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void OnEnable()
    {
        _clickReader.ClickAccepted += SpawnObject;
    }

    private void OnDisable()
    {
        _clickReader.ClickAccepted -= SpawnObject;

        foreach (Timer timer in _activeTimers)
        {
            if (timer != null)
            {
                timer.Wasted -= HandleTimerWasted;
            }

            _activeTimers.Clear();

            _pool?.Clear(); 
        }
    }

    public void RegisterNewTimer(Timer timer)
    {
        if(timer != null && _activeTimers.Contains(timer) == false)
        {
            timer.Wasted += HandleTimerWasted;

            _activeTimers.Add(timer);
        }
    }    

    private void SpawnObject()
    {
        _pool.Get();
    }

    private void HandleTimerWasted(Timer timer)
    {
        _pool.Release(timer.gameObject);

        timer.Wasted -= HandleTimerWasted;

        _activeTimers.Remove(timer);
    }

    private void ActionOnGet(GameObject obj)
    {
        obj.transform.position = CalculateSpawnPosition();

        if (obj.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;

            rigidbody.rotation = Quaternion.identity;
        }

        if (obj.TryGetComponent(out Renderer renderer)) 
        {
        renderer.material.color = Color.white;
        }

        if (obj.TryGetComponent(out Cube cube))
        {
            cube.ResetColorTag();
        }

        obj.SetActive(true);
    }

    private Vector3 CalculateSpawnPosition()
    {
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * _spawnRadius;

        return transform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
    }
}
