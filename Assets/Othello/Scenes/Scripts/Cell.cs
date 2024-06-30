using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Othello
{
    public class Cell : MonoBehaviour
    {
        Othello othello;
        int x;
        int y;

        public int X => x;
        public int Y => y;

        public bool IsEdge => x == 0 || x == Board.Column - 1 || y == 0 || y == Board.Row - 1;
        public bool IsCorner => (x == 0 || x == Board.Column - 1) && (y == 0 || y == Board.Row - 1);

        public void Init(Othello othello, int x, int y)
        {
            this.othello = othello;
            this.x = x;
            this.y = y;

            name = $"{x}_{y}";
        }

        public void OnClick()
        {
            Debug.Log($"{x}_{y}");
        }

    }
}
