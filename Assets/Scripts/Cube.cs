using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]

public class Cube : MonoBehaviour
{
    private Timer _timer;

    private Renderer _renderer;
    private Rigidbody _rigidbody;

    private Color _defaultColor;

    public bool IsColored { get; private set; } = false;

    public event Action<Cube> TimeWasted;

    private void Awake()
    {
        TryGetComponent(out _renderer);
        TryGetComponent(out _rigidbody);

        _defaultColor = _renderer.material.color;
    }

    private void Update()
    {
        _timer?.Run();
    }

    private void OnDisable()
    {
        _timer = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Platform platform) && IsColored == false)
        {
            SetRandomColor();

            _timer = new Timer();
            _timer.Wasted += TimeWastedAlarm;
        }
    }

    public void ResetCube()
    {
        _renderer.material.color = _defaultColor;
        IsColored = false;

        if (_rigidbody != null)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.rotation = Quaternion.identity;
        }

        _timer = null;
    }

    private void TimeWastedAlarm()
    {
        TimeWasted?.Invoke(this);
    }

    private void SetRandomColor()
    {
        if (_renderer != null)
        {
            _renderer.material.color = UnityEngine.Random.ColorHSV();
            IsColored = true;
        }
    }
}