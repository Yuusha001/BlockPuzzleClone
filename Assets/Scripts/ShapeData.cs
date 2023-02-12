using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockPuzzle
{
    [CreateAssetMenu]
    [System.Serializable]
    public class ShapeData : ScriptableObject
    {
        public int colShape,
            rowShape;
        public Square[] Data;

        public void Clear()
        {
            for (int i = 0; i < rowShape; i++)
            {
                Data[i].ClearShape();
            }
        }

        public void CreateBoard()
        {
            Data = new Square[rowShape];
            for (int i = 0; i < rowShape; i++)
            {
                Data[i] = new Square(colShape);
            }
        }
    }

    [System.Serializable]
    public class Square
    {
        public bool[] col;
        private int size;

        public Square(int _size)
        {
            this.size = _size;
            this.col = new bool[_size];
            ClearShape();
        }

        public void ClearShape()
        {
            for (var i = 0; i < size; i++)
            {
                col[i] = false;
            }
        }
    }
}
