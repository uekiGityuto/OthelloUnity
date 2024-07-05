using UnityEngine;

namespace Othello
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] Disc assist;
        [SerializeField] Transform discSpace;
        Othello othello;
        int x;
        int y;
        Disc disc;

        public int X => x;
        public int Y => y;
        public Transform DiscSpace => discSpace;
        public Disc Disc { get => disc; set => disc = value; }

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
            othello.OnCellClick(this);
        }

        public void UpdateAssist(bool active, DiscType discType)
        {
            if (assist.DiscType != discType) assist.SetDiscType(discType);
            assist.gameObject.SetActive(active);
        }

    }
}
