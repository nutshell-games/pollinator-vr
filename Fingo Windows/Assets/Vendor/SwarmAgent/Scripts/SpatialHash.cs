using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Swarm
{
    public class SpatialHash<T>
    {
        Dictionary<int, List<T>> idx = new Dictionary<int, List<T>>();
        List<int> keys = new List<int>();

        int cellSize, count;
        public int maxPerCell;
        bool addNeighbourCells;

        public SpatialHash(int cellSize, int count, int maxPerCell, bool addNeighbourCells = true)
        {
            this.cellSize = cellSize;
            this.count = count;
            this.maxPerCell = maxPerCell;
            this.addNeighbourCells = addNeighbourCells;
        }

        public void Insert(Vector3 v, T obj)
        {
            _Insert(Key(v), obj);
            if (this.addNeighbourCells)
            {
                _Insert(Key(v + Vector3.right * cellSize), obj);
                _Insert(Key(v + Vector3.forward * cellSize), obj);
                _Insert(Key(v + Vector3.back * cellSize), obj);
                _Insert(Key(v + Vector3.left * cellSize), obj);
                _Insert(Key(v + Vector3.up * cellSize), obj);
                _Insert(Key(v + Vector3.down * cellSize), obj);
            }
        }

        void _Insert(int key, T obj)
        {
            List<T> cell;
            if (!idx.TryGetValue(key, out cell))
            {
                cell = idx[key] = new List<T>(maxPerCell);
                keys.Add(key);
            }
            cell.Add(obj);
        }

        public List<T> Query(Vector3 v)
        {
            List<T> cell;
            var key = Key(v);
            if (idx.TryGetValue(key, out cell))
            {
                return cell;
            }
            return null;
        }

        public void Clear()
        {
            for (var i = 0; i < keys.Count; i++)
            {
                idx[keys[i]].Clear();
            }
        }

        int Key(Vector3 v)
        {
            int x = (int)(((v.x / cellSize) * cellSize) + 10000) - 10000;
            int y = (int)(((v.y / cellSize) * cellSize) + 10000) - 10000;
            int z = (int)(((v.z / cellSize) * cellSize) + 10000) - 10000;
            return (x * 73856093 ^ y * 19349663 ^ z * 83492791) % count;
        }

    }
}