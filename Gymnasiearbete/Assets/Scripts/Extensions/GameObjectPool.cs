using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArenaShooter.Extensions
{

    sealed class GameObjectPool<T> where T : Component
    {

        #region Public properties

        public T[] PooledItems
        {
            get
            {
                return pooledItems.ToArray();
            }
        }

        public int ItemCount
        {
            get
            {
                return itemCount;
            }
        }

        public int ActiveItemCount
        {
            get
            {
                return activeItems.Count;
            }
        }

        public int PooledItemCount
        {
            get
            {
                return pooledItems.Count;
            }
        }

        #endregion

        #region Private variables

        private int itemCount;

        private List<T>  activeItems;
        private Queue<T> pooledItems;

        #endregion

        #region Constructors

        public GameObjectPool(Transform parent, GameObject prefab, int prefabInstances)
        {
            activeItems = new List<T>(prefabInstances + 5);
            pooledItems = new Queue<T>(prefabInstances + 5);

            itemCount = prefabInstances;

            for (int i = 0; i < prefabInstances; i++)
            {
                GameObject go = GameObject.Instantiate(prefab, parent);
                go.SetActive(false);
                pooledItems.Enqueue(go.GetComponent<T>());
            }
        }

        public GameObjectPool(params GameObject[] gameObjects)
        {
            activeItems = new List<T>(gameObjects.Length + 5);
            pooledItems = new Queue<T>(gameObjects.Length + 5);

            itemCount = gameObjects.Length;

            foreach (var item in gameObjects)
            {
                item.SetActive(false);
            }
        }

        #endregion

        #region Pooling

        public void PoolItem(T item)
        {
            activeItems.Remove(item);
            pooledItems.Enqueue(item);
            item.gameObject.SetActive(false);
        }

        public T GetItem()
        {
            if (pooledItems.Count > 0)
            {
                T item = pooledItems.Dequeue();
                activeItems.Add(item);
                item.gameObject.SetActive(true);

                return item;
            }

            return null;
        }

        public void PoolAllItems()
        {
            foreach (var item in activeItems.ToList())
            {
                pooledItems.Enqueue(item);
                item.gameObject.SetActive(false);
            }

            activeItems.Clear();
        }

        #endregion

    }

}
