using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace Assets.Assets.Source
{
    internal class Miner : MonoBehaviour
    {

        public Vector2Int GridPosition;
        public Vector2Int MousePosition;

        public bool IsHookBeingThrown = false;
        public bool IsAllowedToMove = true;

        [SerializeField] private GameObject _hook;

        private void Awake()
        {
        }
        public void Start()
        {
        }
        public void Update()
        {
            var mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var mouseGridPosition = GridManager.Instance.WorldToGridPosition(mouseWorldPosition);

            MousePosition = RestrictedMousePosition(mouseGridPosition);

            if (IsHookBeingThrown)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(IsHookBeingThrown);
                StartCoroutine(ThrowHook());
            }
            TryMove();
        }

        private Vector2Int RestrictedMousePosition(Vector2Int mouseGridPosition)
        {
            Vector2Int result = mouseGridPosition;

            // Clamp the mouse position to the grid boundaries
            result.x = Math.Clamp(result.x, 0, GridManager.Instance.Width - 1);
            result.y = Math.Clamp(result.y, 0, GridManager.Instance.Height - 1);

            // Calculate the absolute differences
            var differenceX = Math.Abs(GridPosition.x - result.x);
            var differenceY = Math.Abs(GridPosition.y - result.y);

            // Restrict to the same row or column as GridPosition
            if (differenceX > 0 && differenceY > 0)
            {
                // If the mouse is not on the same row or column, restrict it to the same column
                result.x = GridPosition.x;
            }

            return result;
        }


        public Vector2Int GetDirection()
        {
            // Check for immediate key presses
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                return Vector2Int.up; // Move up
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                return Vector2Int.down; // Move down
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                return Vector2Int.left; // Move left
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                return Vector2Int.right; // Move right
            }

            // If no key was pressed, return zero (no movement)
            return Vector2Int.zero;
        }
        public void TryMove()
        {
            Vector2Int movementDirection = GetDirection();

            if (GetDirection() == Vector2Int.zero)
                return;

            Vector2Int movementPosition = GridPosition + movementDirection;

            bool isAllowedToMove = GridManager.Instance.AskForMove(gameObject, GridPosition, movementPosition);
            if (!isAllowedToMove)
                return;

            GridPosition = movementPosition;

            var worldPosition = GridManager.Instance.GridToWorldPosition(movementPosition.x, movementPosition.y);
            transform.position = worldPosition;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(GridManager.Instance.GridToWorldPosition(MousePosition.x, MousePosition.y), new Vector3(1, 1, 0));
        }

        public IEnumerator ThrowHook()
        {
            IsHookBeingThrown = true;
            var hook = Instantiate(_hook, GridManager.Instance.GridToWorldPosition(GridPosition.x, GridPosition.y), Quaternion.identity);
            // Determine the direction of the hook
            int directionX = Math.Sign(MousePosition.x - GridPosition.x);
            int directionY = Math.Sign(MousePosition.y - GridPosition.y);

            // Start from the GridPosition
            Vector2Int currentPosition = GridPosition;

            // Iterate in the direction of the mouse
            while (true)
            {
                // Move to the next position
                currentPosition.x += directionX;
                currentPosition.y += directionY;

                // Check if the current position is outside the grid boundaries
                if (currentPosition.x < 0 || currentPosition.x >= GridManager.Instance.Width ||
                    currentPosition.y < 0 || currentPosition.y >= GridManager.Instance.Height)
                {
                    IsHookBeingThrown = false;
                    Destroy(hook);
                    yield break; // Stop if outside the grid
                }

                // Check if the current cell contains gold
                if (GridManager.Instance.IsCellContainingGold(currentPosition))
                {
                    // Collect the gold
                    GridManager.Instance.CollectGoldAtPosition(currentPosition);
                    yield return currentPosition; // Return the position where gold was collected
                }

                // Optional: Stop if an obstacle is encountered
                if (GridManager.Instance.IsCellContainingObstacle(currentPosition))
                {
                    IsHookBeingThrown = false;
                    Destroy(hook);
                    yield break;
                }

                hook.transform.position = GridManager.Instance.GridToWorldPosition(currentPosition.x, currentPosition.y);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
