using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Assets.Source
{
    public enum Tiles
    {
        Empty,
        Floor,
        Wall,
        Character
    }
    public enum LayerType
    {
        Floor,
        Wall,
        Character,
        Interactable
    }
    internal class GridManager : MonoBehaviour
    {
        public bool GridInitiated { get; private set; } = false;
        public bool PlayerAllowedToMove { get; private set; } = false;
        public int Width { get; private set; }
        public int Height { get; private set; }

        [SerializeField] private GameObject _miner;
        [SerializeField] private GameObject _wall;
        [SerializeField] private GameObject _floor;
        [SerializeField] private GameObject _enemy;
        [SerializeField] private GameObject _gold;

        public static GridManager Instance { get; private set; }

        private Grid _grid;

        private GameObject[,] _floorLayer;
        private GameObject[,] _wallLayer;
        private GameObject[,] _characterLayer;
        private GameObject[,] _interactableLayer;

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
        }
        private void Start()
        {
        }
        private void Update()
        {
        }
        public void CreateLevel(int width, int height)
        {
            Width = width;
            Height = height;

            CreateFloor(width, height);
            CreateWalls(width, height, _floorLayer);
            CreateCharacters(width, height, _wallLayer);
            CreateInteractables(width, height, _characterLayer);
            GridInitiated = true;
            Debug.Log("Map generation complete");
        }
        private void CreateFloor(int width, int height)
        {
            var grid = gameObject.GetComponent<Grid>();
            _floorLayer = new GameObject[width, height];
            for(int x = 0; x < width; x++) 
            {
                for (int y = 0; y < height; y++)
                {
                    var worldPosition = grid.CellToWorld(new(x, y, 0));
                    var floor = Instantiate(_floor, worldPosition, Quaternion.identity);
                    _floorLayer[x, y] = floor;
                }
            }
        }
        private void CreateWalls(int width, int height, GameObject[,] previousLayer)
        {
            if(previousLayer == null)
                throw new ArgumentNullException(nameof(previousLayer));

            var grid = gameObject.GetComponent<Grid>();
            _wallLayer = new GameObject[width, height];

            var random = new System.Random();
            for(int x = 0; x < width; x++) 
            {
                for (int y = 0; y < height; y++)
                {
                    var worldPosition = grid.CellToWorld(new(x, y, 0));
                    var hasWall = random.Next(2) == 0;
                    if (!hasWall)
                        continue;

                    var hasGold = random.Next(2) == 0;

                    var wall = Instantiate(_wall, worldPosition, Quaternion.identity);
                    var wallComponent = wall.GetComponent<Wall>();

                    wallComponent.AssignGoldAndAddAction(new(x, y), hasGold, OnWallDestroyed);

                    _wallLayer[x, y] = wall;
                }
            }
        }

        private void OnWallDestroyed(object sender, WallEventArgs e)
        {
            var wallGO = _wallLayer[e.GridPosition.x, e.GridPosition.y];
            Destroy(wallGO);
            _wallLayer[e.GridPosition.x, e.GridPosition.y] = null;
            if (!e.HasGold)
                return;
            CreateGoldAtPosition(e.GridPosition);
        }

        private void CreateGoldAtPosition(Vector2Int gridPosition)
        {
            var worldPosition = GridToWorldPosition(gridPosition.x, gridPosition.y);
            var gold = Instantiate(_gold, worldPosition, Quaternion.identity);
            _interactableLayer[gridPosition.x, gridPosition.y] = gold;
        }

        private void CreateCharacters(int width, int height, GameObject[,] previousLayer)
        {
            if(previousLayer == null)
                throw new ArgumentNullException(nameof(previousLayer));
            
            var grid = gameObject.GetComponent<Grid>();

            _characterLayer = new GameObject[width, height];

            for(int x = 0; x < width; x++) 
            {
                for (int y = 0; y < height; y++)
                {
                    if (previousLayer[x, y] != null)
                        continue;
                    var worldPosition = grid.CellToWorld(new(x, y, 0));
                    var minerGO = Instantiate(_miner, worldPosition, Quaternion.identity);
                    var minerComponent = minerGO.GetComponent<Miner>();

                    minerComponent.GridPosition = new Vector2Int(x, y);

                    _characterLayer[x, y] = minerGO;
                    Debug.Log("Created miner at " + worldPosition);
                    return;
                }
            }
        }
        private void CreateInteractables(int width, int height, GameObject[,] previousLayer)
        {
            if(previousLayer == null)
                throw new ArgumentNullException(nameof(previousLayer));

            _interactableLayer = new GameObject[width, height];
        }
        public Dictionary<LayerType, GameObject> GetTilesAtPosInAllLayers(int x, int y)
        {
            if (!GridInitiated)
                throw new Exception("Game map wasn't initiated");
            if (x < 0 || y < 0)
                throw new ArgumentOutOfRangeException();
            if(x >= Width || y >= Height)
                throw new ArgumentOutOfRangeException();

            var tiles = new Dictionary<LayerType, GameObject>();
            tiles.Add(LayerType.Floor, _floorLayer[x, y]);
            tiles.Add(LayerType.Wall, _wallLayer[x, y]);
            tiles.Add(LayerType.Character, _characterLayer[x, y]);
            tiles.Add(LayerType.Interactable, _interactableLayer[x, y]);
            return tiles;
        }
        public Vector2 GridToWorldPosition(int x, int y)
        {
            var grid = GetComponent<Grid>();
            return grid.CellToWorld(new(x, y, 0));
        }

        public void CollectGoldAtPosition(Vector2Int position)
        {
            GameDataManager.Instance.AmountOfGoldInInventory++;
            var goldGO = _interactableLayer[position.x, position.y];
            Destroy(goldGO);
            _interactableLayer[position.x, position.y] = null;
        }
        public bool AskForMove(GameObject goToMove, Vector2Int currentPosition, Vector2Int movementPosition)
        {
            if (movementPosition.x < 0 || movementPosition.y < 0)
                return false;
            if(movementPosition.x >=  Width || movementPosition.y >= Height)
                return false;

            var wallGO = _wallLayer[movementPosition.x, movementPosition.y];
            var goldGO = _interactableLayer[movementPosition.x, movementPosition.y];

            if (goldGO != null)
                CollectGoldAtPosition(movementPosition);

            if(wallGO == null)
            {
                _characterLayer[movementPosition.x, movementPosition.y] = goToMove;
                _characterLayer[currentPosition.x, currentPosition.y] = null;
                return true;
            }
            var wallComponent = wallGO.GetComponent<Wall>();
            wallComponent.DamageWall(1);
            return false;
        }
    }
}
