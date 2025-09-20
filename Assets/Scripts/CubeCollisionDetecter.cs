using System;
using UnityEngine;

public class CubeCollisionDetecter : MonoBehaviour
{
    public event Action<Cube> Collided;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out Cube cube))
        {
            Collided?.Invoke(cube);
        }
    }
}
