using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]

public class Cube : MonoBehaviour
{
    public bool IsColored { get; private set; } = false;

    public void SetRandomColor()
    {
        if (TryGetComponent(out Renderer renderer))
        {
            renderer.material.color = UnityEngine.Random.ColorHSV();
            IsColored = true;
        }
    }

    public void ResetColorTag()
    {
        IsColored = false;
    }
}