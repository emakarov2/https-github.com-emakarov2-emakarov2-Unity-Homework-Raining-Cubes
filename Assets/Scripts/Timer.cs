using System;

using UnityEngine;

public class Timer : MonoBehaviour
{
    public event Action<Timer> Wasted;

    private float _minLifeTime = 2f;
    private float _maxLifeTime = 5f;

    private float _currentLifeTime = 0;
    private float _fullLifeTime;

    private void Awake()
    {
        SetFullLifeTime();
    }

    private void Update()
    {
        Run();
    }

    private void SetFullLifeTime()
    {
        _fullLifeTime = UnityEngine.Random.Range(_minLifeTime, _maxLifeTime);
    }

    private void Run()
    {
        _currentLifeTime += Time.deltaTime;

        if (_currentLifeTime >= _fullLifeTime)
        {
            Wasted?.Invoke(this);
        }
    }
}
