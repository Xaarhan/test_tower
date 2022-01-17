using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void ApplyForce(Vector2 force)
    {
        _rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    public void DrawLine(Vector3 from, Vector3 to)
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPositions(new[] {from, to});
    }

    public void ClearLine()
    {
        _lineRenderer.positionCount = 0;
    }
}
