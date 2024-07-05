using UnityEngine;
using System.Collections.Generic;

namespace Othello
{
    public class Board : MonoBehaviour
    {
        public const int Column = 8;
        public const int Row = 8;

        [SerializeField] Othello othello;
        [SerializeField] Transform offset;
        Cell[,] cells = new Cell[Column, Row];
        List<Disc> reverseDiscs = new List<Disc>();
        List<Disc> foundReverseDiscs = new List<Disc>();

        public Cell[,] Cells => cells;

        public void Init()
        {
            for (var y = 0; y < Row; y++)
            {
                for (var x = 0; x < Column; x++)
                {
                    var cell = Instantiate(Resources.Load<Cell>("Prefabs/Cell"), offset);
                    cell.Init(othello, x, y);
                    cells[x, y] = cell;
                }
            }
        }

        public void PlaceDiscDirect(int x, int y, DiscType discType)
        {
            var cell = cells[x, y];
            var disc = Instantiate(Resources.Load<Disc>("Prefabs/Disc"), cell.DiscSpace);
            disc.Init(discType);
            cell.Disc = disc;
        }

        public void PlaceDisc(Cell cell, Disc disc)
        {
            disc.transform.SetParent(cell.DiscSpace);
            disc.transform.localPosition = Vector3.zero;
            disc.transform.eulerAngles = Vector3.zero;
            disc.gameObject.SetActive(true);

            cell.Disc = disc;
        }

        public bool CanPlaceDisc(DiscType discType)
        {
            foreach (var cell in cells)
            {
                if (CanPlaceDisc(cell, discType)) return true;
            }
            return false;
        }

        public bool CanPlaceDisc(Cell cell, DiscType discType)
        {
            var reverseDiscs = GetReverseDiscs(cell, discType);
            return (reverseDiscs.Count > 0);
        }

        public List<Disc> GetReverseDiscs(Cell cell, DiscType discType)
        {
            reverseDiscs.Clear();

            if (cell.Disc != null) return reverseDiscs;

            foreach (var c in cells)
            {
                if (c.Disc == null) continue;
                c.Disc.ReverseIdx = 0;
            }

            foundReverseDiscs.Clear();
            for (var x = cell.X + 1; x < Column; x++)
            {
                if (SearchReverseDiscs(cells[x, cell.Y], discType)) break;
            }

            foundReverseDiscs.Clear();
            for (var x = cell.X - 1; x >= 0; x--)
            {
                if (SearchReverseDiscs(cells[x, cell.Y], discType)) break;
            }

            foundReverseDiscs.Clear();
            for (var y = cell.Y + 1; y < Row; y++)
            {
                if (SearchReverseDiscs(cells[cell.X, y], discType)) break;
            }

            foundReverseDiscs.Clear();
            for (var y = cell.Y - 1; y >= 0; y--)
            {
                if (SearchReverseDiscs(cells[cell.X, y], discType)) break;
            }

            foundReverseDiscs.Clear();
            {
                var x = cell.X - 1;
                var y = cell.Y - 1;
                while (x >= 0 && y >= 0)
                {
                    if (SearchReverseDiscs(cells[x, y], discType)) break;
                    x--;
                    y--;
                }
            }

            foundReverseDiscs.Clear();
            {
                var x = cell.X + 1;
                var y = cell.Y - 1;
                while (x < Column && y >= 0)
                {
                    if (SearchReverseDiscs(cells[x, y], discType)) break;
                    x++;
                    y--;
                }
            }

            foundReverseDiscs.Clear();
            {
                var x = cell.X - 1;
                var y = cell.Y + 1;
                while (x >= 0 && y < Row)
                {
                    if (SearchReverseDiscs(cells[x, y], discType)) break;
                    x--;
                    y++;
                }
            }

            foundReverseDiscs.Clear();
            {
                var x = cell.X + 1;
                var y = cell.Y + 1;
                while (x < Column && y < Row)
                {
                    if (SearchReverseDiscs(cells[x, y], discType)) break;
                    x++;
                    y++;
                }
            }

            return reverseDiscs;
        }

        bool SearchReverseDiscs(Cell cell, DiscType discType)
        {
            if (cell.Disc == null)
            {
                return true;
            }
            else if (cell.Disc.DiscType != discType)
            {
                cell.Disc.ReverseIdx = foundReverseDiscs.Count;
                foundReverseDiscs.Add(cell.Disc);
                return false;
            }
            else
            {
                if (foundReverseDiscs.Count > 0)
                {
                    reverseDiscs.AddRange(foundReverseDiscs);
                }
                return true;
            }
        }

        public void UpdateAssist(bool isAssist, DiscType discType)
        {
            foreach (var cell in cells)
            {
                if (isAssist)
                {
                    var active = CanPlaceDisc(cell, discType);
                    cell.UpdateAssist(active, discType);
                }
                else
                {
                    cell.UpdateAssist(false, discType);
                }
            }
        }
    }
}
