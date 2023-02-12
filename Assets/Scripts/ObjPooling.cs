using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace BlockPuzzle
{
    public class ObjPooling : MonoBehaviour
    {
        public static ObjPooling SharedInstance;

        [Header("Obj pooling elemets")]
        public Dictionary<string, ObjectPool<GameObject>> ListObjPooling;
        public GameObject gridPrefab;
        public GameObject squarePrefab;
        public GameObject shapePrefab;

        void Awake()
        {
            SharedInstance = this;
            ListObjPooling = new Dictionary<string, ObjectPool<GameObject>>();
        }

        void Start()
        {
            ObjPooling.SharedInstance.CreatePool("Grid", gridPrefab, this.transform, 64, 64);
            ObjPooling.SharedInstance.CreatePool("Square", squarePrefab, this.transform, 64, 64);
            ObjPooling.SharedInstance.CreatePool("Shape", shapePrefab, this.transform, 32, 32);
        }

        public void CreatePool(
            string keyName,
            GameObject go,
            Transform par,
            int amountToPool,
            int maxiumAmount
        )
        {
            ObjectPool<GameObject> tempList = new ObjectPool<GameObject>(
                () =>
                {
                    return Instantiate(go, par);
                },
                item =>
                {
                    item.gameObject.SetActive(true);
                },
                item =>
                {
                    item.gameObject.SetActive(false);
                },
                item =>
                {
                    Destroy(item.gameObject);
                },
                false,
                amountToPool,
                maxiumAmount
            );
            ListObjPooling.Add(keyName, tempList);
        }

        public GameObject GetPooledObject(string keyName)
        {
            return ListObjPooling[keyName].Get();
        }

        public void Kill_GameObj(string keyName, GameObject go)
        {
            go.transform.SetParent(this.transform);
            go.transform.position = Vector3.zero;
            ListObjPooling[keyName].Release(go);
        }
    }
}
