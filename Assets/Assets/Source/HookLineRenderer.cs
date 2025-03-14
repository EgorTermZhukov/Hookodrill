using UnityEngine;

public class HookLineRenderer : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private Vector3 _startingPosition;
    private bool _positionAssigned = false;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_positionAssigned)
        {
            return;
        }

        _lineRenderer.SetPositions(new[] { _startingPosition, transform.position } );
    }
    public void AssignStartingPosition(Vector3 startingPosition)
    {
        _startingPosition = startingPosition;
        _positionAssigned = true;
    }
}
