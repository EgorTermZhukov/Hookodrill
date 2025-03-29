using System;
using System.Collections;
using Assets.Source;
using UnityEngine;
using DG.Tweening;

namespace Assets.Assets.Source
{
    internal class GridManager : MonoBehaviour
    {
        public bool TutorialMode { get; set; } = false;
        public bool GameStopped { get; set; } = false;
        public bool ObstaclesSpawning { get; set; } = false;
        public int LevelRequirement { get; private set; } =  5;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public static GridManager Instance { get; private set; }
        public event Action<int> OnGameEnded;
        public event Action OnLevelLoaded;

        [SerializeField] private GameObject _miner;
        [SerializeField] private GameObject _wall;
        [SerializeField] private GameObject _floor;
        [SerializeField] private GameObject _enemy;
        [SerializeField] private GameObject _gold;

        private int _goldOnTheLevel = 0;

        public int LevelCount = 0;
        private int CycleLevelCount = 0;

        private GameObject[,] _floorLayer;
        private GameObject[,] _wallLayer;
        private GameObject[,] _characterLayer;
        private GameObject[,] _interactableLayer;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(this);
            }
        }
        private void Update()
        {
        }
        public void CreateLevel(int width, int height)
        {
            GameDataManager.Instance.LevelCount++;
            if(TimerManager.Instance != null)
                TimerManager.Instance.IsLevelStarted = false;
            if(LevelCount >= 10 && !TutorialMode)
                ObstaclesSpawning = true;
            LevelCount++;
            CycleLevelCount++;
            //SoundManager.Instance.LevelComplete();
            Width = width;
            Height = height;

            CreateFloor(width, height);
            CreateWalls(width, height, _floorLayer);
            CreateCharacters(width, height, _wallLayer);
            CreateInteractables(width, height, _characterLayer);
            AdjustCamera();
            OnLevelLoaded?.Invoke();
        }
        private void CreateFloor(int width, int height)
        {
            var grid = gameObject.GetComponent<Grid>();
            _floorLayer = new GameObject[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var worldPosition = grid.CellToWorld(new(x, y, 0));
                    var floor = Instantiate(_floor, worldPosition, Quaternion.identity);
                    _floorLayer[x, y] = floor;
                }
            }
        }
        private void CreateWallAtPosition(Vector2Int gridPosition, bool hasGold, bool isObstacle)
        {
            var grid = gameObject.GetComponent<Grid>();
            var worldPosition = grid.CellToWorld(new(gridPosition.x, gridPosition.y, 0));
            var wall = Instantiate(_wall, worldPosition, Quaternion.identity);
            var wallComponent = wall.GetComponent<Wall>();
            wallComponent.IsObstacle = isObstacle;

            if (hasGold)
                _goldOnTheLevel++;

            wallComponent.AssignGoldAndAddAction(new(gridPosition.x, gridPosition.y), hasGold, CreateGoldOnWallDestroyed);

            _wallLayer[gridPosition.x, gridPosition.y] = wall;
        }
        private void GenerateFirstWallRow(int width, int height)
        {
            int startX = UnityEngine.Random.Range(0, width - 2);
            int startY = UnityEngine.Random.Range(0, height - 2);

            bool isHorizontalSlice = UnityEngine.Random.Range(0, 1) == 0 ? false : true;

            var slicePositions = _wallLayer.GetSliceIndices(startX, startY, isHorizontalSlice);

            int goldCount = 0;

            foreach(var position in slicePositions)
            {
                if (goldCount >= 3)
                    break;
                goldCount++;
                CreateWallAtPosition(position, true, false);
            }
        }
        private void CreateWalls(int width, int height, GameObject[,] previousLayer)
        {
            if (previousLayer == null)
                throw new ArgumentNullException(nameof(previousLayer));

            _wallLayer = new GameObject[width, height];

            var random = new System.Random();


            GenerateFirstWallRow(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (_wallLayer[x, y] != null)
                        continue;
                    var hasWall = random.Next(2) == 0;
                    if (!hasWall)
                        continue;

                    var hasGold = random.Next(2) == 0;
                    var isAnObstacle = random.Next(4) == 0;

                    if (!ObstaclesSpawning)
                        isAnObstacle = false;

                    CreateWallAtPosition(new(x, y), hasGold, isAnObstacle);
                }
            }
        }

        private void CreateGoldOnWallDestroyed(object sender, WallEventArgs e)
        {
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
            if (previousLayer == null)
                throw new ArgumentNullException(nameof(previousLayer));

            var grid = gameObject.GetComponent<Grid>();

            _characterLayer = new GameObject[width, height];

            for (int x = 0; x < width; x++)
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
                    return;
                }
            }
        }
        private void CreateInteractables(int width, int height, GameObject[,] previousLayer)
        {
            if (previousLayer == null)
                throw new ArgumentNullException(nameof(previousLayer));

            _interactableLayer = new GameObject[width, height];
        }
        public Vector2 GridToWorldPosition(int x, int y)
        {
            var grid = GetComponent<Grid>();
            return grid.CellToWorld(new(x, y, 0));
        }
        public Vector2 GridToWorldPosition(Vector2Int position)
        {
            return GridToWorldPosition(position.x, position.y);
        }

        public Vector2Int WorldToGridPosition(Vector3 worldPosition)
        {
            var grid = GetComponent<Grid>();
            return new(grid.WorldToCell(worldPosition).x, grid.WorldToCell(worldPosition).y);
        }
        public void CollectGoldAtPosition(Vector2Int position)
        {
            SoundManager.Instance.GoldCollect();
            _goldOnTheLevel--;
            int goldValue = 1;
            if (GameDataManager.Instance != null && TimerManager.Instance != null)
            {
                if (TimerManager.Instance.CountdownTime > TimerManager.ScoreMultiplicationBarrierTime)
                    goldValue = 2;
                GameDataManager.Instance.AmountOfGoldInInventory += goldValue;
            }
            var goldGO = _interactableLayer[position.x, position.y];
            Destroy(goldGO);
            _interactableLayer[position.x, position.y] = null;
            //if(TimerManager.Instance.CountdownTime > TimerManager.ScoreMultiplicationBarrierTime)
            //    UIManager.Instance.ShowPopup(GridToWorldPosition(position.x, position.y), Color.yellow, goldValue.ToString() + "$");
            if (_goldOnTheLevel <= 0)
                GridManager.Instance.ReloadLevel();
        }
        private void ReloadLevel()
        {
            DestroyMap();
            if(!TutorialMode && CycleLevelCount % LevelRequirement == 0)
            {
                CycleLevelCount = 0;
                Width++;
                Height++;
                var popupPosition = GridToWorldPosition((Width - 1) / 2, (Height - 1) / 2);
                //IncreaseTimer(5, popupPosition);
            }
            if(!TutorialMode)
                UIManager.Instance.UpdateLevelText(CycleLevelCount % LevelRequirement);
            CreateLevel(Width, Height);
        }
        private void DestroyMap()
        {
            foreach (var floor in _floorLayer)
            {
                if (floor != null)
                    Destroy(floor);
            }
            foreach (var wall in _wallLayer)
            {
                if (wall != null)
                    Destroy(wall);
            }
            foreach (var character in _characterLayer)
            {
                if (character != null)
                    Destroy(character);
            }
            foreach (var interactable in _interactableLayer)
            {
                if (interactable != null)
                    Destroy(interactable);
            }
        }

        public void DamageWallAtPosition(Vector2Int position)
        {
            var wallToDamage = _wallLayer[position.x, position.y];
            if(wallToDamage == null)
                return;
            wallToDamage.GetComponent<Wall>().DamageWall(1);
        }
        public bool AskForMove(GameObject goToMove, Vector2Int currentPosition, Vector2Int movementPosition)
        {
            if (!IsInBounds(movementPosition))
                return false;

            if(TimerManager.Instance != null && !TimerManager.Instance.IsLevelStarted) 
                TimerManager.Instance.IsLevelStarted = true;

            var wallGO = _wallLayer[movementPosition.x, movementPosition.y];

            if (wallGO != null)
            {
                var wallComponent = wallGO.GetComponent<Wall>();

                if (wallComponent.IsObstacle)
                {
                    wallComponent.CarveOutObstacle();
                    return false;
                }

                wallComponent.DamageWall(1);
                if (IsCellContainingGold(movementPosition))
                    CollectGoldAtPosition(movementPosition);
            }

            _characterLayer[movementPosition.x, movementPosition.y] = goToMove;
            _characterLayer[currentPosition.x, currentPosition.y] = null; 

            return true;
        }
        public bool IsInBounds(Vector2Int position)
        {
            if (position.x < 0 || position.y < 0)
                return false;
            if(position.x >=  Width || position.y >= Height)
                return false;
            return true;
        }
        public bool IsCellContainingGold(Vector2Int cellPosition)
        {
            return _interactableLayer[cellPosition.x, cellPosition.y] != null;
        }
        public bool IsCellContainingWall(Vector2Int cellPosition)
        {
            return _wallLayer[cellPosition.x, cellPosition.y] != null;
        }
        public bool IsCellContainingObstacle(Vector2Int cellPosition)
        {
            var go = _wallLayer[cellPosition.x, cellPosition.y];

            if (go == null)
                return false;

            var obstacle = go.GetComponent<Wall>();
            return obstacle.IsObstacle;
        }
        public void FinishGame()
        {
            DestroyMap();
            UIManager.Instance.PlayGameEndSequence();
            OnGameEnded?.Invoke(GameDataManager.Instance.AmountOfGoldInInventory);
        }
        private void AdjustCamera()
        {
            Camera cam = Camera.main;
            if (cam == null)
            {
                Debug.LogError("Main camera not found.");
                return;
            }

            Grid grid = GetComponent<Grid>();
            if (grid == null)
            {
                Debug.LogError("Grid component not found.");
                return;
            }

            Vector3 cellSize = grid.cellSize;

            // Calculate grid bounds in world space
            Vector3 minPos = grid.CellToWorld(new Vector3Int(0, 0, 0));
            Vector3 maxPos = grid.CellToWorld(new Vector3Int(Width - 1, Height - 1, 0)) + new Vector3(cellSize.x, cellSize.y, 0);

            // Center the camera
            Vector3 center = (minPos + maxPos) / 2f;

            // Adjust the center to account for the off-center issue

            // Use DoTween to smoothly move the camera to the center
            cam.transform.DOMove(new Vector3(center.x, center.y, cam.transform.position.z), 1f).SetEase(Ease.OutQuad);

            // Calculate required orthographic size to fit the grid
            float gridWidth = maxPos.x - minPos.x;
            float gridHeight = maxPos.y - minPos.y;
            float aspectRatio = cam.aspect;
            float targetSizeHeight = gridHeight / 2f;
            float targetSizeWidth = (gridWidth / 2f) / aspectRatio;

            float targetOrthoSize = Mathf.Max(targetSizeHeight, targetSizeWidth);

            // Add padding for text and update camera
            float padding = 2f; // Increase padding to fit text
            float finalOrthoSize = targetOrthoSize + padding;

            // Use DoTween to smoothly adjust the orthographic size
            cam.DOOrthoSize(finalOrthoSize, 1f).SetEase(Ease.OutQuad);
        }

        public void WinGame()
        {
            DOTween.KillAll();
            DestroyMap();
            TimerManager.Instance.IsTimerRunning = false;
        }
    }
}
