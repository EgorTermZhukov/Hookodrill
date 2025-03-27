using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Assets.Source;
using NUnit.Framework.Constraints;

namespace Assets.Assets.Source
{
    internal class Miner : MonoBehaviour
    {
        public Vector2Int GridPosition;
        public Vector2Int FacingDirection;
        public bool IsHookBeingThrown = false;
        public bool IsAllowedToMove = true;

        [SerializeField] private Sprite _withDrillSprite;
        [SerializeField] private Sprite _withoutDrillSprite;

        [SerializeField] private Animator _animator;

        [SerializeField] private GameObject _hook;
        [SerializeField] private float zMoveCooldown = 0.2f;
        [SerializeField] private float inputBufferWindow = 0.2f;

        private float _lastZMoveTime;
        private float _bufferedZPressTime = -1f;

        private bool _hookCancelled = false;

        private GameObject _currentHook = null;

        private SpriteRenderer _spriteRenderer;
        private int _timeBonus = 3;

        private void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            FacingDirection = Vector2Int.right;
            _spriteRenderer.sprite = _withDrillSprite;
        }

        public void Start()
        {
            _lastZMoveTime = -zMoveCooldown;
        }

        public void Update()
        {
            if(TimerManager.Instance != null && TimerManager.Instance.CountdownTime < 5)
            {
                _animator.SetBool("TimerLow", true);
            }
            else
            {
                _animator.SetBool("TimerLow", false);
            }
            if (GridManager.Instance.GameStopped)
                return;
            Vector2Int newDirection = ChangeDirection(FacingDirection);
            if (newDirection != FacingDirection)
            {
                FacingDirection = newDirection;
                RotateSprite(FacingDirection);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                _bufferedZPressTime = Time.time;
            }

            if (IsHookBeingThrown)
                return;

            if (_bufferedZPressTime > 0 && Time.time - _bufferedZPressTime <= inputBufferWindow)
            {
                TryMove();
                _bufferedZPressTime = -1f; // Consume the buffered input
                _lastZMoveTime = Time.time; // Sync with cooldown system
            }

            // Existing cooldown-based movement
            if (Input.GetKey(KeyCode.Z) && Time.time - _lastZMoveTime >= zMoveCooldown)
            {
                TryMove();
                _lastZMoveTime = Time.time;
            }

            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.K))
            {
                StartCoroutine(ThrowHook());
            }
        }

        public Vector2Int ChangeDirection(Vector2Int currentDirection)
        {
            var directionToReturn = currentDirection;
            // Check for immediate key presses
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                directionToReturn = Vector2Int.up; // Move up
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                directionToReturn = Vector2Int.down; // Move down
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                directionToReturn = Vector2Int.left; // Move left
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                directionToReturn = Vector2Int.right; // Move right
            }

            if (directionToReturn != currentDirection)
            {
                _lastZMoveTime = Time.time - zMoveCooldown / 1.75f;
            }
            return directionToReturn;
        }

        private void RotateSprite(Vector2Int direction)
        {
            _spriteRenderer.flipY = false;

            if (direction == Vector2Int.left)
            {
                _spriteRenderer.flipY = true; // Flip vertically
                transform.DORotate(new Vector3(0, 0, 180f), 0.1f); // Reset rotation
            }
            else if (direction == Vector2Int.right)
            {
                transform.DORotate(Vector3.zero, 0.1f); // Face right
            }
            else if (direction == Vector2Int.up)
            {
                transform.DORotate(new Vector3(0, 0, 90f), 0.1f); // Rotate up
            }
            else if (direction == Vector2Int.down)
            {
                transform.DORotate(new Vector3(0, 0, -90f), 0.1f); // Rotate down
            }
        }
        private void RotateHook(GameObject hook, Vector2Int direction)
        {
            float targetAngle = 0f;

            if (direction == Vector2Int.up)
            {
                targetAngle = 90f;
            }
            else if (direction == Vector2Int.down)
            {
                targetAngle = -90f;
            }
            else if (direction == Vector2Int.left)
            {
                targetAngle = 180f;
            }
            else if (direction == Vector2Int.right)
            {
                targetAngle = 0f;
            }
            hook.transform.eulerAngles = new Vector3(0, 0, targetAngle);
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
            if (_currentHook != null)
                _currentHook.transform.DOKill();
            Destroy(_currentHook);
            transform.DOKill();
        }

        public IEnumerator ThrowHook()
        {
            _spriteRenderer.sprite = _withoutDrillSprite;
            SoundManager.Instance.HookThrow();
            IsHookBeingThrown = true;

            var hook = Instantiate(_hook, GridManager.Instance.GridToWorldPosition(GridPosition.x, GridPosition.y), Quaternion.identity);
            RotateHook(hook, FacingDirection);
            hook.GetComponent<HookLineRenderer>().AssignStartingPosition(GridManager.Instance.GridToWorldPosition(GridPosition.x, GridPosition.y));
            _currentHook = hook;
            int directionX = FacingDirection.x;
            int directionY = FacingDirection.y;

            Vector2Int currentPosition = GridPosition;

            List<Vector2Int> collectedGoldPositions = new List<Vector2Int>();

            int collectedGold = 0;
            while (true)
            {
                if (_hookCancelled)
                    break;
                currentPosition.x += directionX;
                currentPosition.y += directionY;

                if (!GridManager.Instance.IsInBounds(currentPosition))
                {
                    break;
                }

                if (GridManager.Instance.IsCellContainingObstacle(currentPosition))
                {
                    SoundManager.Instance.ObstacleHit();
                    break;
                }

                if (GridManager.Instance.IsCellContainingWall(currentPosition))
                {
                    yield return new WaitForSeconds(0.02f);
                    GridManager.Instance.DamageWallAtPosition(currentPosition);
                }
                if (GridManager.Instance.IsCellContainingGold(currentPosition))
                {
                    GridManager.Instance.CollectGoldAtPosition(currentPosition);
                    collectedGold++;
                    if(collectedGold % 3 == 0)
                    {
                        GameDataManager.Instance.HookPowerCount++;
                        if(TimerManager.Instance != null)
                            TimerManager.Instance.IncreaseTimer(_timeBonus, GridManager.Instance.GridToWorldPosition(currentPosition));
                    }
                }
                var targetPosition = GridManager.Instance.GridToWorldPosition(currentPosition.x, currentPosition.y);
                var tween = hook.transform.DOMove(targetPosition, 0.02f);
                yield return new WaitForSeconds(0.02f);
            }

            yield return new WaitForSeconds(0.05f);

            while (currentPosition != GridPosition)
            {
                currentPosition.x -= directionX;
                currentPosition.y -= directionY;

                var targetPosition = GridManager.Instance.GridToWorldPosition(currentPosition.x, currentPosition.y);
                var tween = hook.transform.DOMove(targetPosition, 0.01f);
                yield return new WaitForSeconds(0.01f);
            }
            IsHookBeingThrown = false;
            _spriteRenderer.sprite = _withDrillSprite;
            Destroy(hook);
            yield break;
        }
    }
}