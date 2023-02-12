using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace BlockPuzzle
{
    public class Shape
        : MonoBehaviour,
            IPointerClickHandler,
            IPointerDownHandler,
            IPointerUpHandler,
            IBeginDragHandler,
            IEndDragHandler,
            IDragHandler
    {
        public ShapeData currentShapeData { get; private set; }
        List<GameObject> currentShape = new List<GameObject>();
        public int totalSquare { get; private set; }
        bool draggAble = true;
        public Vector3 startPos { get; set; }
        Vector3 ShapeSelectedScale = Vector3.one;
        Vector3 OriginalScale = new Vector3(0.6f, 0.6f, 0.6f);
        Vector2 offset = new Vector2(0, 100);

        [SerializeField]
        RectTransform _transform;

        public bool isOnStartPos()
        {
            return _transform.localPosition == startPos;
        }

        private void OnEnable()
        {
            GameEvent.ReturnShapeToStartPosition += MoveToStartPosion;
        }

        private void OnDisable()
        {
            GameEvent.ReturnShapeToStartPosition -= MoveToStartPosion;
        }

        private void MoveToStartPosion()
        {
            _transform.localPosition = startPos;
            _transform.localScale = OriginalScale;
        }

        public void CreateShape(ShapeData shapeData)
        {
            currentShapeData = shapeData;
            totalSquare = GetNumberOfSquares(currentShapeData);
            for (var i = 0; i < totalSquare; i++)
            {
                var go = ObjPooling.SharedInstance.GetPooledObject("Square");
                go.transform.SetParent(this.transform);
                go.transform.localScale = Vector3.one;
                currentShape.Add(go);
            }

            RectTransform squareRect =
                ObjPooling.SharedInstance.squarePrefab.GetComponent<RectTransform>();
            Vector2 moveDistance = new Vector2(
                squareRect.rect.width * squareRect.localScale.x,
                squareRect.rect.height * squareRect.localScale.y
            );
            int currentIndex = 0;
            for (int row = 0; row < shapeData.rowShape; row++)
            {
                for (var col = 0; col < shapeData.colShape; col++)
                {
                    if (shapeData.Data[row].col[col])
                    {
                        currentShape[currentIndex].GetComponent<RectTransform>().localPosition =
                            newPos(shapeData, col, row, moveDistance);
                        currentIndex++;
                    }
                }
            }
        }

        Vector2 newPos(ShapeData shapeData, int column, int row, Vector2 moveDistance)
        {
            return new Vector2(
                GetXPositionForShapeSquare(shapeData, column, moveDistance),
                GetYPositionForShapeSquare(shapeData, row, moveDistance)
            );
        }

        private float GetXPositionForShapeSquare(
            ShapeData shapeData,
            int column,
            Vector2 moveDistance
        )
        {
            float shiftOnX = 0f;
            if (shapeData.colShape > 1)
            {
                float startXPos;
                if (shapeData.colShape % 2 != 0)
                    startXPos = (shapeData.colShape / 2) * moveDistance.x * -1;
                else
                    startXPos =
                        ((shapeData.colShape / 2) - 1) * moveDistance.x * -1 - moveDistance.x / 2;
                shiftOnX = startXPos + column * moveDistance.x;
            }
            return shiftOnX;
        }

        private float GetYPositionForShapeSquare(ShapeData shapeData, int row, Vector2 moveDistance)
        {
            float shiftOnY = 0f;
            if (shapeData.rowShape > 1)
            {
                float startYPos;
                if (shapeData.rowShape % 2 != 0)
                    startYPos = (shapeData.rowShape / 2) * moveDistance.y;
                else
                    startYPos =
                        ((shapeData.rowShape / 2) - 1) * moveDistance.y + moveDistance.y / 2;
                shiftOnY = startYPos - row * moveDistance.y;
            }
            return shiftOnY;
        }

        private int GetNumberOfSquares(ShapeData shapeData)
        {
            int number = 0;
            foreach (var row in shapeData.Data)
            {
                foreach (bool active in row.col)
                {
                    if (active)
                        number++;
                }
            }
            return number;
        }

        public void RemoveShape()
        {
            ShapeManager.instance.RemoveShape(this);
            foreach (var item in currentShape)
            {
                ObjPooling.SharedInstance.Kill_GameObj("Square", item);
            }
            currentShape.Clear();
            ObjPooling.SharedInstance.Kill_GameObj("Shape", this.gameObject);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _transform.localScale = ShapeSelectedScale;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GameEvent.CheckIfShapeCanPlaced();
        }

        public void OnDrag(PointerEventData eventData)
        {
            _transform.anchorMax = Vector2.zero;
            _transform.anchorMin = Vector2.zero;
            _transform.pivot = Vector2.zero;
            Vector2 Pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                ShapeManager.instance.MainCanvas.transform as RectTransform,
                eventData.position,
                Camera.main,
                out Pos
            );
            _transform.localPosition = Pos + offset;
        }
    }
}
