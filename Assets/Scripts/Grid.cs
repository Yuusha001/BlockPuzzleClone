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
        LineDetecter lineDetecter;

        [SerializeField]
        List<GridSquare> gameField = new List<GridSquare>();

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
                if (item.CanUseThisSquare())
                {
                    squareIndex.Add(item.SquareID);
                    item.SetisSelected(false);
                }
            }

            if (ShapeManager.instance.CurrentSelected().totalSquare == squareIndex.Count)
            {
                foreach (var item in squareIndex)
                {
                    var square = gameField[item];
                    square.ActiveSquare();
                    square.SetImage(ShapeManager.instance.CurrentSelected().image);
                }
                ShapeManager.instance.CurrentSelected().RemoveShape();
                CheckIfLineIsCompleted();
            }
            else
                GameEvent.ReturnShapeToStartPosition();
        }

        public void createField()
        {
            spawnField();
            lineDetecter.Setup(8);
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
                    gameField.Add(go.GetComponent<GridSquare>());
                    count++;
                }
            }
        }

        void CheckIfLineIsCompleted()
        {
            List<List<int>> CompletedLines = new List<List<int>>();
            //columns
            foreach (var item in lineDetecter.column)
            {
                CompletedLines.Add(item.data);
            }

            //rows
            foreach (var item in lineDetecter.lineData)
            {
                CompletedLines.Add(item.data);
            }

            var totalLines = CheckIfSquareIsCompleted(CompletedLines);
            switch (totalLines)
            {
                case 1:
                    Debug.Log("Good");
                    GameEvent.AddScore(100);
                    break;
                case 2:
                    GameEvent.AddScore(300);
                    Debug.Log("Cool");
                    break;
                case 3:
                    GameEvent.AddScore(500);
                    Debug.Log("Amazing");
                    break;
                case 4:
                    GameEvent.AddScore(700);
                    Debug.Log("Amazing");
                    break;
                default:
                    break;
            }
        }

        int CheckIfSquareIsCompleted(List<List<int>> data)
        {
            List<List<int>> CompletedLines = new List<List<int>>();

            foreach (var line in data)
            {
                var lineCompleted = true;
                foreach (var item in line)
                {
                    var square = gameField[item];
                    if (!square.GetisActive())
                    {
                        lineCompleted = false;
                    }
                }
                if (lineCompleted)
                {
                    CompletedLines.Add(line);
                }
            }

            ClearLine(CompletedLines);
            return CompletedLines.Count;
        }

        void ClearLine(List<List<int>> CompletedLines)
        {
            foreach (var Line in CompletedLines)
            {
                foreach (var item in Line)
                {
                    var square = gameField[item];
                    square.DeActiveSquare();
                    square.SetOriginalImage();
                }
            }
        }
    }
}
