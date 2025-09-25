using System;
using System.Collections;
using UnityEngine;

public class Timer
{
    private float _minLifeTime = 2f;
    private float _maxLifeTime = 5f;

    private MonoBehaviour _coroutineRunner;
    private Coroutine _timerCoroutine;

    public event Action Wasted;

    public Timer(MonoBehaviour coroutineRunner)
    {
        _coroutineRunner = coroutineRunner;
    }

    public void Start()
    {
        float lifeTime = UnityEngine.Random.Range(_minLifeTime, _maxLifeTime);

        Stop();

        _timerCoroutine = _coroutineRunner.StartCoroutine(CountTime(lifeTime));
    }

    public void Stop()
    {
        if (_timerCoroutine != null)
        {
            _coroutineRunner.StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }
    }

    private IEnumerator CountTime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);

        Wasted?.Invoke();

        _timerCoroutine = null;
    }
}
