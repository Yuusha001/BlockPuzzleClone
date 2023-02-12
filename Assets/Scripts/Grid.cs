using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockPuzzle
{
    public class Grid : MonoBehaviour
    {
        public bool isCreated;

        [SerializeField]
        int col,
            row;

        [SerializeField]
        Vector2 startPos;

        [SerializeField]
        List<GameObject> gameField = new List<GameObject>();

        private void OnEnable()
        {
            GameEvent.CheckIfShapeCanPlaced += CheckIfShapeCanPlaced;
        }

        private void OnDisable()
        {
            GameEvent.CheckIfShapeCanPlaced -= CheckIfShapeCanPlaced;
        }

        private void CheckIfShapeCanPlaced()
        {
            var squareIndex = new List<int>();
            foreach (var item in gameField)
            {
                var square = item.GetComponent<GridSquare>();
                if (square.CanUseThisSquare())
                {
                    squareIndex.Add(square.SquareID);
                    square.isSelected = false;
                }
            }

            if (ShapeManager.instance.CurrentSelected().totalSquare == squareIndex.Count)
            {
                foreach (var item in squareIndex)
                {
                    gameField[item].GetComponent<GridSquare>().ActiveSquare();
                }
                ShapeManager.instance.CurrentSelected().RemoveShape();
            }
            else
                GameEvent.ReturnShapeToStartPosition();
        }

        public void createField()
        {
            spawnField();
            isCreated = true;
        }

        void spawnField()
        {
            int count = 0;
            for (var i = 0; i < row; i++)
            {
                for (var j = 0; j < col; j++)
                {
                    var go = ObjPooling.SharedInstance.GetPooledObject("Grid");
                    go.transform.SetParent(this.transform);
                    go.transform.localScale = Vector3.one;
                    go.name = count.ToString();
                    go.GetComponent<GridSquare>().SquareID = count;
                    gameField.Add(go);
                    count++;
                }
            }
        }
    }
}
