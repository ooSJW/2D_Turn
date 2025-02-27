/*
	* Coder :
	* Last Update :
	* Information
*/
namespace project02
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public partial class PoolManager : MonoBehaviour // Inner Class
    {
        public class Pool
        {
            private Transform parent;
            private GameObject originPrefab;
            private List<GameObject> poolingObjectList;
            private int initialCount;

            public Pool(GameObject originPrefabValue, int initialCountValue = 0)
            {
                poolingObjectList = new List<GameObject>();
                originPrefab = originPrefabValue;
                parent = new GameObject() { name = $"Root_{originPrefabValue.name}" }.transform;
                initialCount = initialCountValue;
            }

            public void Register()
            {
                for (int i = 0; i < initialCount; i++)
                {
                    GameObject poolableObject = Instantiate(originPrefab, parent.transform);
                    poolableObject.name = originPrefab.name;
                    poolableObject.SetActive(false);
                    poolingObjectList.Add(poolableObject);
                }
            }

            public GameObject Spawn(Transform parentValue = null, Vector3 spawnPosition = default)
            {
                GameObject poolableObject = null;
                if (poolingObjectList.Count > 0)
                {
                    poolableObject = poolingObjectList[0];
                    poolableObject.transform.SetParent(parentValue);

                    if (spawnPosition != default)
                        poolableObject.transform.position = spawnPosition;

                    poolingObjectList.Remove(poolableObject);
                    poolableObject.SetActive(true);
                }
                else
                {
                    if (spawnPosition != default)
                    {
                        Quaternion originRotation = originPrefab.transform.rotation;
                        poolableObject = Instantiate(originPrefab, spawnPosition, originRotation, parentValue);
                    }
                    else
                        poolableObject = Instantiate(originPrefab, parentValue);

                    poolableObject.name = originPrefab.name;
                    poolableObject.transform.SetParent(parentValue);
                }
                return poolableObject;
            }

            public void Despawn(GameObject poolObject)
            {
                poolObject.SetActive(false);
                poolingObjectList.Add(poolObject);
                poolObject.transform.SetParent(parent);
            }
        }
    }

    public partial class PoolManager : MonoBehaviour // Data Field
    {
        private Dictionary<string, Pool> poolDict = default;
    }

    public partial class PoolManager : MonoBehaviour // Initialize
    {
        private void Allocate()
        {
            poolDict = new Dictionary<string, Pool>();
        }
        public void Initialize()
        {
            Allocate();
            Setup();
        }
        private void Setup()
        {

        }
    }

    public partial class PoolManager : MonoBehaviour // Property
    {
        public void Register()
        {
            poolDict.Clear();
            List<GameObject> poolableList = MainSystem.Instance.SceneManager.ActiveScene.poolableObject;
            for (int i = 0; i < poolableList.Count; i++)
            {
                Pool pool = new Pool(poolableList[i]);
                pool.Register();
                poolDict.Add(poolableList[i].name, pool);
            }
        }

        public GameObject Spawn(string name, Transform parent = null, Vector3 spawnPosition = default)
        {
            return poolDict[name].Spawn(parent, spawnPosition);
        }

        public void Despawn(GameObject poolableObject)
        {
            poolDict[poolableObject.name].Despawn(poolableObject);
        }
    }
}
