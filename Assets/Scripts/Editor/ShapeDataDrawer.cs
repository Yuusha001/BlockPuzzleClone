using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BlockPuzzle
{
    [CustomEditor(typeof(ShapeData), false)]
    [CanEditMultipleObjects]
    [System.Serializable]
    public class ShapeDataDrawer : Editor
    {
        private ShapeData instance => target as ShapeData;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            ClearBoard();
            EditorGUILayout.Space();
            DrawColumnsInputFields();
            EditorGUILayout.Space();
            if (instance.Data != null && instance.colShape > 0 && instance.rowShape > 0)
            {
                drawBoad();
            }
            serializedObject.ApplyModifiedProperties();
            if (GUI.changed)
            {
                EditorUtility.SetDirty(instance);
            }
        }

        private void ClearBoard()
        {
            if (GUILayout.Button("Clear Board"))
            {
                instance.Clear();
            }
        }

        private void DrawColumnsInputFields()
        {
            var columnsTemp = instance.colShape;
            var rowsTemp = instance.rowShape;

            instance.colShape = EditorGUILayout.IntField(label: "Columns", instance.colShape);
            instance.rowShape = EditorGUILayout.IntField(label: "Rows", instance.rowShape);
            if (
                (instance.colShape != columnsTemp || instance.rowShape != rowsTemp)
                && instance.colShape > 0
                && instance.rowShape > 0
            )
                instance.CreateBoard();
        }

        private void drawBoad()
        {
            var tableStyle = new GUIStyle("box");
            tableStyle.padding = new RectOffset(10, 10, 10, 10);
            tableStyle.margin.left = 32;
            var headerColumnStyle = new GUIStyle();
            headerColumnStyle.fixedWidth = 65;
            headerColumnStyle.alignment = TextAnchor.MiddleCenter;

            var rowStyle = new GUIStyle();
            rowStyle.fixedHeight = 25;
            rowStyle.alignment = TextAnchor.MiddleCenter;
            var dataFieldStyle = new GUIStyle(EditorStyles.miniButtonMid);
            dataFieldStyle.normal.background = Texture2D.grayTexture;

            dataFieldStyle.onNormal.background = Texture2D.whiteTexture;
            for (var row = 0; row < instance.rowShape; row++)
            {
                EditorGUILayout.BeginHorizontal(headerColumnStyle);
                for (var column = 0; column < instance.colShape; column++)
                {
                    EditorGUILayout.BeginHorizontal(rowStyle);
                    var data = EditorGUILayout.Toggle(
                        instance.Data[row].col[column],
                        dataFieldStyle
                    );
                    instance.Data[row].col[column] = data;
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
