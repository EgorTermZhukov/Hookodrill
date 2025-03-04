using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

namespace Assets.Assets.Source
{
    internal class Miner : MonoBehaviour
    {
        public Vector2Int GridPosition;

        private void Awake()
        {
        }
        public void Start()
        {
        }
        public void Update()
        {
            TryMove();
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
    }
}
