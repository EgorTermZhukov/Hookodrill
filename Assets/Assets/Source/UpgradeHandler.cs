using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Source
{
    public class UpgradeHandler : MonoBehaviour
    {
        public static UpgradeHandler Instance;
        private List<IOnBlockDestroyed> _onBlockDestroyed;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _onBlockDestroyed = new List<IOnBlockDestroyed>();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public void OnBlockDestroyed(WallDestroyData wallDestroyData)
        {
            foreach (var wallDestroyUpgrade in _onBlockDestroyed)
            {
                wallDestroyUpgrade.OnBlockDestroyed(wallDestroyData);
            }
        }
        public void OnLevelStarted()
        {
            
        }
        public void OnLevelCompleted()
        {
            
        }
        public void OnDeath()
        {
            
        }
    }
    internal interface IOnBlockDestroyed
    {
        void OnBlockDestroyed(WallDestroyData destroyData);
    }

    public class WallDestroyData
    {
        public Vector2Int destroyDirection;
    }

    public class Upgrade
    {
        public string Description;
    }
}