using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockPuzzle
{
    public class ShapeManager : MonoBehaviour
    {
        public static ShapeManager instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        [SerializeField]
        List<ShapeDataType> shapeDataTypes;

        [SerializeField]
        List<Shape> shapeList;

        [SerializeField]
        List<int> _rd = new List<int>();

        [SerializeField]
        List<Vector3> startPost;

        [SerializeField]
        RectTransform spawnSpot;

        [SerializeField]
        Canvas mainCanvas;
        bool isSpawn;
        public Canvas MainCanvas
        {
            get => mainCanvas;
            set => mainCanvas = value;
        }

        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.instance.currentState != GameState.Playing)
            {
                return;
            }
            if (!isSpawn)
                New3Shape();
        }

        public Shape CurrentSelected()
        {
            for (var i = 0; i < shapeList.Count; i++)
            {
                if (!shapeList[i].isOnStartPos())
                    return shapeList[i];
            }
            return null;
        }

        public void CleanList()
        {
            foreach (var item in shapeList)
            {
                ObjPooling.SharedInstance.Kill_GameObj("Shape", item.gameObject);
            }
            shapeList.Clear();
        }

        public void RemoveShape(Shape me)
        {
            shapeList.Remove(me);
            if (shapeList.Count == 0)
                New3Shape();
        }

        public void New3Shape()
        {
            if (shapeList.Count != 0)
                CleanList();
            for (var i = 0; i < 3; i++)
            {
                var go = ObjPooling.SharedInstance.GetPooledObject("Shape");
                go.transform.SetParent(spawnSpot);
                go.transform.localPosition = startPost[i];
                go.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                go.GetComponent<Shape>().startPos = startPost[i];
                shapeList.Add(go.GetComponent<Shape>());
            }
            isSpawn = false;
            Random3Shape();
        }

        public void Random3Shape()
        {
            RandomType();
            for (var i = 0; i < shapeList.Count; i++)
            {
                shapeList[i].CreateShape(RandomData(_rd[i]));
            }
            isSpawn = true;
        }

        void RandomType()
        {
            _rd[0] = Random.Range(0, shapeDataTypes.Count);
            _rd[1] = Random.Range(0, shapeDataTypes.Count);
            _rd[2] = Random.Range(0, shapeDataTypes.Count);
            while (_rd[0] == _rd[1])
            {
                _rd[1] = Random.Range(0, shapeDataTypes.Count);
            }
            while (_rd[2] == _rd[0] || _rd[2] == _rd[1])
            {
                _rd[2] = Random.Range(0, shapeDataTypes.Count);
            }
        }

        ShapeData RandomData(int type)
        {
            int _random = Random.Range(0, shapeDataTypes[type].data.Count);
            return shapeDataTypes[type].data[_random];
        }
    }

    [System.Serializable]
    public class ShapeDataType
    {
        public EnumShapeDataType type;
        public List<ShapeData> data;
    }

    public enum EnumShapeDataType
    {
        One,
        One_N,
        N_One,
        Two_Two,
        Three_Three,
        T_Block,
        L_Block,
        N_Block
    }
}
