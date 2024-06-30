using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Othello
{
    public class Board : MonoBehaviour
    {
        public const int Column = 8;
        public const int Row = 8;

        [SerializeField] Othello othello;
        [SerializeField] Transform offset;
        Cell[,] cells = new Cell[Column, Row];

        public Cell[,] Cells => cells;

        public void Init()
        {
            for (var x = 0; x < Row; x++)
            {
                for (var y = 0; y < Column; y++)
                {
                    var cell = Instantiate(Resources.Load<Cell>("Prefabs/Cell"), offset);
                    cell.Init(othello, x, y);
                    cells[x, y] = cell;
                }
            }
        }
    }
}
