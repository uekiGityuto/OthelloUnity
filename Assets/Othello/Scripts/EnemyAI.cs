using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Othello
{

    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] Board board;
        [SerializeField] Enemy enemy;
        List<Cell> foundCells = new List<Cell>();

        public bool CalculateNormalAI(out Cell selectedCell, out List<Disc> selectedReverseDiscs)
        {
            foundCells.Clear();
            foreach (var cell in board.Cells)
            {
                if (board.CanPlaceDisc(cell, enemy.DiscType))
                {
                    foundCells.Add(cell);
                }
            }

            selectedCell = foundCells[Random.Range(0, foundCells.Count)];
            selectedReverseDiscs = board.GetReverseDiscs(selectedCell, enemy.DiscType);
            return selectedReverseDiscs.Count > 0;
        }

        public bool CalculateHardAI(out Cell selectedCell, out List<Disc> selectedReverseDiscs)
        {
            foundCells.Clear();
            var maxReverseCount = 0;
            var hasEdge = false;
            var hasCorner = false;

            foreach (var cell in board.Cells)
            {
                var reverseDiscs = board.GetReverseDiscs(cell, enemy.DiscType);
                var reverseCount = reverseDiscs.Count;
                if (reverseCount == 0) continue;

                if (cell.IsCorner || hasCorner)
                {
                    if (cell.IsCorner)
                    {
                        if (!hasCorner || reverseCount > maxReverseCount)
                        {
                            hasCorner = true;
                            maxReverseCount = reverseCount;
                            foundCells.Clear();
                            foundCells.Add(cell);
                        }
                        else if (reverseCount == maxReverseCount)
                        {
                            foundCells.Add(cell);
                        }
                    }
                }
                else if (cell.IsEdge || hasEdge)
                {
                    if (cell.IsEdge)
                    {
                        if (!hasEdge || reverseCount > maxReverseCount)
                        {
                            hasEdge = true;
                            maxReverseCount = reverseCount;
                            foundCells.Clear();
                            foundCells.Add(cell);
                        }
                        else if (reverseCount == maxReverseCount)
                        {
                            foundCells.Add(cell);
                        }
                    }
                }
                else
                {

                    if (reverseCount > maxReverseCount)
                    {
                        maxReverseCount = reverseCount;
                        foundCells.Clear();
                        foundCells.Add(cell);
                    }
                    else if (reverseCount == maxReverseCount)
                    {
                        foundCells.Add(cell);
                    }
                }
            }

            selectedCell = foundCells[Random.Range(0, foundCells.Count)];
            selectedReverseDiscs = board.GetReverseDiscs(selectedCell, enemy.DiscType);
            return selectedReverseDiscs.Count > 0;
        }
    }
}
