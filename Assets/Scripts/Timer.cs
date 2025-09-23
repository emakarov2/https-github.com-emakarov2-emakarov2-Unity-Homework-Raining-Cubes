using System;

using UnityEngine;

public class Timer
{
    private float _minLifeTime = 2f;
    private float _maxLifeTime = 5f;

    private float _currentLifeTime;
    private float _fullLifeTime;

    public event Action Wasted;

    public Timer()
    {
        _currentLifeTime = 0;

        SetFullLifeTime();
    }

    public void Run()
    {
        _currentLifeTime += Time.deltaTime;

        if (_currentLifeTime >= _fullLifeTime)
        {
            Wasted?.Invoke();
        }
    }

    private void SetFullLifeTime()
    {
        _fullLifeTime = UnityEngine.Random.Range(_minLifeTime, _maxLifeTime);
    }
}
