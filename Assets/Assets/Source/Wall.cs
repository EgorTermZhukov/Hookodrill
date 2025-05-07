using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Assets.Source
{
    internal class WallEventArgs : EventArgs
    {
        public Vector2Int GridPosition { get; set; }
        public bool HasGold { get; set; }

        public WallEventArgs(Vector2Int wallPosition, bool hasGold) 
        {
            GridPosition = wallPosition;
            HasGold = hasGold;
        }
    }
    internal class Wall : MonoBehaviour
    {
        public event EventHandler<WallEventArgs> OnWallDestroyed;

        [SerializeField] private GameObject _wallDestroyParticle;
        [SerializeField] private GameObject _goldWallDestroyParticle;
        [SerializeField] private GameObject _obstacleClearParticle;

        private bool _isObstacle;

        public bool IsObstacle
        {
            get => _isObstacle;
            set
            {
                if(value == true)
                {
                    _isObstacle = true;
                    if (HasGold)
                    {
                        _renderer.sprite = _obstacleGoldWallSprite;
                    }
                    else
                    {
                        _renderer.sprite = _obstacleWallSprite;
                    }
                }
                else
                {
                    _isObstacle = false;
                    if (HasGold)
                    {
                        _renderer.sprite = _normalGoldWallSprite;
                    }
                    else
                    {
                        _renderer.sprite = _normalWallSprite;
                    }
                }
            }
        }

        [SerializeField] private Sprite _normalWallSprite;
        [FormerlySerializedAs("_goldWallSprite")] [SerializeField] private Sprite _normalGoldWallSprite;
        [SerializeField] private Sprite _obstacleWallSprite;
        [SerializeField] private Sprite _obstacleGoldWallSprite;

        [SerializeField] private Animator _wallAnimator;

        private Vector2Int _gridPosition;

        private int _durability = 1;
        private SpriteRenderer _renderer;
        
        private void Awake()
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
        }
        private bool _hasGold;
        public bool HasGold
        {
            get => _hasGold;
            set
            {
                if(value)
                {
                    _hasGold = true;
                    if(!IsObstacle)
                        _renderer.sprite = _normalGoldWallSprite;
                    else
                        _renderer.sprite = _obstacleGoldWallSprite;
                }
                else
                {
                    _hasGold = false;
                    if(!IsObstacle)
                        _renderer.sprite = _normalWallSprite;
                    else
                        _renderer.sprite = _obstacleWallSprite;
                }
            }
        }
        public void AssignGoldAndAddAction(Vector2Int gridPosition, bool hasGold, EventHandler<WallEventArgs> wallDestroyedAction)
        {
            _gridPosition = gridPosition;
            HasGold = hasGold;
            OnWallDestroyed += wallDestroyedAction;
        }
        public void DamageWall(int damage)
        {
            _durability = _durability - damage > 0 ? _durability - damage : 0;
            if (_durability == 0)
            {
                _wallAnimator.SetBool("WasDamaged", true);
                OnWallDestroyed?.Invoke(this, new WallEventArgs(_gridPosition, HasGold));
                DestroyWall();
            }
            SoundManager.Instance.WallHit();
        }

        public void ShowShine()
        {
            _wallAnimator.SetBool("IsShining", true);
            Debug.Log("Shine is showing at pos: " + _gridPosition);
        }
        private void OnDestroy()
        {
        }
        public void DestroyWall()
        {
            StartCoroutine(PlayWallDestroyingSequence());
        }

        public void CarveOutObstacle()
        {
            Instantiate(_obstacleClearParticle, GridManager.Instance.GridToWorldPosition(_gridPosition.x, _gridPosition.y), Quaternion.identity);
            IsObstacle = false;
            SoundManager.Instance.ObstacleHit();
        }

        private IEnumerator PlayWallDestroyingSequence()
        {
            if (!HasGold)
            {
                Instantiate(_wallDestroyParticle, GridManager.Instance.GridToWorldPosition(_gridPosition.x, _gridPosition.y), Quaternion.identity);
            }
            else
            {
                Instantiate(_goldWallDestroyParticle, GridManager.Instance.GridToWorldPosition(_gridPosition.x, _gridPosition.y), Quaternion.identity);
            }
            yield return new WaitForSeconds(0.4f);
            Destroy(this.gameObject);
            yield break;
        }
    }
}
