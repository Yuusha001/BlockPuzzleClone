using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockPuzzle
{
    public class LineDetecter : MonoBehaviour
    {
        public List<Line> lineData = new List<Line>();
        public List<Line> column = new List<Line>();
        public List<Line> squareData = new List<Line>();

        public void Setup(int number)
        {
            //Column
            for (var rows = 0; rows < number; rows++)
            {
                for (var cols = 0; cols < number; cols++)
                {
                    column[rows].data.Add(rows + cols * number);
                }
            }

            //Row
            for (var rows = 0; rows < number; rows++)
            {
                for (var cols = 0; cols < number; cols++)
                {
                    lineData[rows].data.Add(rows * number + cols);
                }
            }
        }
    }

    [System.Serializable]
    public class Line
    {
        public List<int> data = new List<int>();
    }
}
