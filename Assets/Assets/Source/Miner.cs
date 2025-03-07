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

        private LineRenderer _lineRenderer;

        private GameObject _currentHook = null;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();   
            FacingDirection = Vector2Int.right;
        }
        public void Start()
        {
        }
        public void Update()
        {
            FacingDirection = ChangeDirection(FacingDirection);

            var worldPos = GridManager.Instance.GridToWorldPosition(GridPosition.x, GridPosition.y);
            var facingDirWorldPos = GridManager.Instance.GridToWorldPosition(GridPosition.x + FacingDirection.x, GridPosition.y + FacingDirection.y);
            _lineRenderer.SetPositions(new[] {new Vector3(worldPos.x, worldPos.y), new Vector3(facingDirWorldPos.x, facingDirWorldPos.y) });
            
            if (IsHookBeingThrown)
                return;

            if (Input.GetKeyDown(KeyCode.X))
            {
                StartCoroutine(ThrowHook());
            }

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
            SoundManager.Instance.Move();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(FacingDirection.x, FacingDirection.y, 0));
        }

        public void OnDestroy()
        {
            if(_currentHook != null)
                Destroy(_currentHook);
        }
        public IEnumerator ThrowHook()
        {
            SoundManager.Instance.HookThrow();
            if (!GameDataManager.Instance.UsePowerup())
                yield break;
            IsHookBeingThrown = true;

            var hook = Instantiate(_hook, GridManager.Instance.GridToWorldPosition(GridPosition.x, GridPosition.y), Quaternion.identity);
            _currentHook  = hook;   
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

                if (GridManager.Instance.IsCellContainingGold(currentPosition))
                {
                    GridManager.Instance.CollectGoldAtPosition(currentPosition);
                    collectedGoldPositions.Add(currentPosition);
                }

                if(GridManager.Instance.IsCellContainingObstacle(currentPosition)) 
                {
                    break;
                }

                if (GridManager.Instance.IsCellContainingWall(currentPosition))
                {
                    GridManager.Instance.DamageWallAtPosition(currentPosition);
                }
                hook.transform.position = GridManager.Instance.GridToWorldPosition(currentPosition.x, currentPosition.y);
                yield return new WaitForSeconds(0.03f);
            }

            while (currentPosition != GridPosition)
            {
                if (currentPosition.x - directionX == GridPosition.x && currentPosition.y - directionY == GridPosition.y)
                {
                    break;
                }
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
            yield break;
        }
    }
}
