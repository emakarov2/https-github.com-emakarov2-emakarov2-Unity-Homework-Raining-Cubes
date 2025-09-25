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

    public event Action<Cube> TimeWasted;

    public bool IsColored { get; private set; } = false;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();

        _defaultColor = _renderer.material.color;

        _timer = new Timer(this);
        _timer.Wasted += OnTimeWasted;
    }

    private void OnDisable()
    {
        _timer.Wasted -= OnTimeWasted;
        _timer?.Stop();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsColored == false && collision.gameObject.TryGetComponent(out Platform platform))
        {
            SetRandomColor();

            _timer.Start();
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

        _timer?.Stop();
    }

    private void OnTimeWasted()
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