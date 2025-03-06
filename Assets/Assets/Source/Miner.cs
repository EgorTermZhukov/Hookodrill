using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Assets.Source
{
    internal class Miner : MonoBehaviour
    {
        public Vector2Int GridPosition;

        public Vector2Int FacingDirection;

        public bool IsHookBeingThrown = false;
        public bool IsAllowedToMove = true;

        [SerializeField] private GameObject _hook;
        [SerializeField] private GameObject _squareSelectPrefab;

        private GameObject _squareSelect;

        private void Awake()
        {
            _squareSelect = Instantiate(_squareSelectPrefab);
            FacingDirection = Vector2Int.right;
        }
        public void Start()
        {
        }
        public void Update()
        {
            if (IsHookBeingThrown)
                return;
            if (Input.GetKeyDown(KeyCode.X))
            {
                StartCoroutine(ThrowHook());
            }

            FacingDirection = ChangeDirection(FacingDirection);
            if(Input.GetKeyDown(KeyCode.Z))
                TryMove();
        }
        public Vector2Int ChangeDirection(Vector2Int currentDirection)
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
            return currentDirection;
        }
        public void TryMove()
        {
            Vector2Int movementPosition = GridPosition + FacingDirection;

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
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(FacingDirection.x, FacingDirection.y, 0));
        }

        public IEnumerator ThrowHook()
        {
            IsHookBeingThrown = true;

            var hook = Instantiate(_hook, GridManager.Instance.GridToWorldPosition(GridPosition.x, GridPosition.y), Quaternion.identity);

            int directionX = FacingDirection.x;
            int directionY = FacingDirection.y;

            Vector2Int currentPosition = GridPosition;

            List<Vector2Int> collectedGoldPositions = new List<Vector2Int>();

            while (true)
            {
                currentPosition.x += directionX;
                currentPosition.y += directionY;

                if (!GridManager.Instance.IsInBounds(currentPosition))
                {
                    break;
                }

                hook.transform.position = GridManager.Instance.GridToWorldPosition(currentPosition.x, currentPosition.y);
                yield return new WaitForSeconds(0.03f);

                if (GridManager.Instance.IsCellContainingGold(currentPosition))
                {
                    GridManager.Instance.CollectGoldAtPosition(currentPosition);
                    collectedGoldPositions.Add(currentPosition);
                }

                if (GridManager.Instance.IsCellContainingObstacle(currentPosition))
                {
                    GridManager.Instance.DamageWallAtPosition(currentPosition);
                }
            }

            while (currentPosition != GridPosition)
            {
                currentPosition.x -= directionX;
                currentPosition.y -= directionY;

                if (GridManager.Instance.IsCellContainingGold(currentPosition))
                {
                    GridManager.Instance.CollectGoldAtPosition(currentPosition);
                    collectedGoldPositions.Add(currentPosition);
                }

                hook.transform.position = GridManager.Instance.GridToWorldPosition(currentPosition.x, currentPosition.y);
                yield return new WaitForSeconds(0.03f);
            }

            IsHookBeingThrown = false;
            Destroy(hook);
        }
    }
}
