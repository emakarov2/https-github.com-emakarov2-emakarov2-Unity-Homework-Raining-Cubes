using UnityEngine;

public class ClickReader : MonoBehaviour
{
    private const int ClickNumber = 0;

    public event System.Action ClickAccepted;

    private void Update()
    {
        if (Input.GetMouseButtonDown(ClickNumber))
        {
            Vector2 clickPosition = Input.mousePosition;

            ClickAccepted?.Invoke();
        }
    }
}