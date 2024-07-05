using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Othello
{

    public enum Difficulty { Normal, Hard }
    public enum PlayFirst { Player, Enemy, Random }
    public enum DiscType { Black, White }
    public enum Turn { Player, Enemy }

    public class Othello : MonoBehaviour
    {
        enum Sequence { None, DiscPlacement, DiscReversing }

        [SerializeField] Difficulty difficulty;
        [SerializeField] PlayFirst playFirst;
        [SerializeField] bool isAssist;
        [SerializeField] StartMenu startMenu;
        [SerializeField] Player player;
        [SerializeField] Enemy enemy;
        [SerializeField] Board board;
        [SerializeField] Button restart;

        Turn turn;
        Sequence sq;
        Disc selectedDisc;
        Cell selectedCell;
        List<Disc> selectedReverseDiscs;

        public Difficulty Difficulty { get => difficulty; set => difficulty = value; }
        public PlayFirst PlayFirst { get => playFirst; set => playFirst = value; }
        public bool IsAssist { get => isAssist; set => isAssist = value; }

        void Update()
        {
            if (sq == Sequence.DiscPlacement)
            {
                board.PlaceDisc(selectedCell, selectedDisc);
                foreach (var disc in selectedReverseDiscs)
                {
                    disc.Reverse.Play(selectedDisc.DiscType);
                }
                sq = Sequence.DiscReversing;
            }
            else if (sq == Sequence.DiscReversing)
            {
                var isPlaying = false;
                foreach (var disc in selectedReverseDiscs)
                {
                    if (disc.Reverse.IsPlaying)
                    {
                        isPlaying = true;
                        break;
                    }
                }

                if (!isPlaying)
                {
                    sq = Sequence.None;
                    restart.interactable = true;
                    ChangeTurn();
                }
            }
        }

        public void OnGameStartClick()
        {
            Turn firstTurn;
            var r = Random.Range(0, 2);
            Debug.Log(r);
            if (playFirst == PlayFirst.Player || (playFirst == PlayFirst.Random && r == 0))
            {
                firstTurn = Turn.Player;
                player.Init(DiscType.Black, true);
                enemy.Init(DiscType.White, false, difficulty);
            }
            else
            {
                firstTurn = Turn.Enemy;
                player.Init(DiscType.White, false);
                enemy.Init(DiscType.Black, true, difficulty);
            }

            startMenu.gameObject.SetActive(false);
            ChangeTurn(firstTurn);
        }

        public void OnCellClick(Cell cell)
        {
            if (turn != Turn.Player) return;
            if (sq != Sequence.None) return;

            var reverseDiscs = board.GetReverseDiscs(cell, player.DiscType);
            ExecuteDiscPlacement(cell, reverseDiscs);

        }

        public void OnRestartClick()
        {
            SceneManager.LoadScene("Othello");
        }

        void ExecuteDiscPlacement(Cell cell, List<Disc> reverseDiscs)
        {
            if (reverseDiscs.Count > 0)
            {
                sq = Sequence.DiscPlacement;
                selectedCell = cell;
                selectedReverseDiscs = reverseDiscs;
                restart.interactable = false;
            }
            else
            {
                cell.Ng.Play();
            }
        }

        void ChangeTurn()
        {
            var nextTurn = (turn == Turn.Player) ? Turn.Enemy : Turn.Player;
            ChangeTurn(nextTurn);
        }

        void ChangeTurn(Turn nextTurn)
        {
            turn = nextTurn;
            turn = nextTurn;
            if (turn == Turn.Player)
            {
                board.UpdateAssist(isAssist, player.DiscType);
                player.Bound.Play();

                selectedDisc = player.GetNextDisc();
            }
            else
            {
                board.UpdateAssist(isAssist, enemy.DiscType);
                enemy.Bound.Play();

                selectedDisc = enemy.GetNextDisc();
            }
        }
    }
}
