using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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

        private bool _isObstacle;
        public bool IsObstacle
        {
            get => _isObstacle;
            set
            {
                if(value == true)
                {
                    _isObstacle = true;
                    _renderer.color = Color.gray;
                }
                else
                {
                    _isObstacle = false;
                }
            }
        }

        [SerializeField] private Sprite _normalWallSprite;
        [SerializeField] private Sprite _goldWallSprite;

        private Vector2Int _gridPosition;

        private int _durability = 1;
        private SpriteRenderer _renderer;
        
        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();   
        }
        private bool _hasGold;
        public bool HasGold
        {
            get => _hasGold;
            set
            {
                if(value == true)
                {
                    _hasGold = true;
                    _renderer.sprite = _goldWallSprite;
                }
                else
                {
                    _hasGold = false;
                    _renderer.sprite = _normalWallSprite;
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
            Debug.Log("Damaged wall");
            _durability = _durability - damage > 0 ? _durability - damage : 0;
            if (_durability == 0)
            {
                OnWallDestroyed?.Invoke(this, new WallEventArgs(_gridPosition, HasGold));
            }
            SoundManager.Instance.WallHit();
        }
    }
}
