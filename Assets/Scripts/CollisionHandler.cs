
using Unity.VisualScripting;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private CubeCollisionDetecter _collisionDetecter;
    [SerializeField] private Spawner _spawner;

    private void OnEnable()
    {
        _collisionDetecter.Collided += HandleFirstCollide;
    }

    private void OnDisable()
    {
        _collisionDetecter.Collided -= HandleFirstCollide;
    }

    private void HandleFirstCollide(Cube cube)
    {
        if (cube.IsColored == false)
        {
            Timer timer = cube.AddComponent<Timer>();
            
            _spawner.RegisterNewTimer(timer);

            cube.SetRandomColor();
        }
    }
}
