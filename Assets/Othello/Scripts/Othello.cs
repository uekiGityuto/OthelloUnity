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
        enum Sequence { None, DiscPlacement, DiscReversing, Pass, PassPlaying }

        [SerializeField] Difficulty difficulty;
        [SerializeField] PlayFirst playFirst;
        [SerializeField] bool isAssist;
        [SerializeField] StartMenu startMenu;
        [SerializeField] Player player;
        [SerializeField] Enemy enemy;
        [SerializeField] Board board;
        [SerializeField] Button restart;
        [SerializeField] InfoAnimation info;

        Turn turn;
        Sequence sq;
        float time;
        float waitTime;
        Disc selectedDisc;
        Cell selectedCell;
        List<Disc> selectedReverseDiscs;
        int passCount;


        public Difficulty Difficulty { get => difficulty; set => difficulty = value; }
        public PlayFirst PlayFirst { get => playFirst; set => playFirst = value; }
        public bool IsAssist { get => isAssist; set => isAssist = value; }

        void Update()
        {
            if (sq == Sequence.DiscPlacement)
            {
                time += Time.deltaTime;
                if (time >= waitTime)
                {
                    time = 0;
                    board.PlaceDisc(selectedCell, selectedDisc);
                    foreach (var disc in selectedReverseDiscs)
                    {
                        disc.Reverse.Play(selectedDisc.DiscType);
                    }
                    sq = Sequence.DiscReversing;
                }
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
            else if (sq == Sequence.Pass)
            {
                info.PlayPass();
                sq = Sequence.PassPlaying;
            }
            else if (sq == Sequence.PassPlaying)
            {
                if (!info.IsPlaying)
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
                waitTime = (turn == Turn.Enemy) ? Enemy.DiscPlacementWaitTime : 0;
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
            if ((player.Discs.Count == 0 && enemy.Discs.Count == 0) || passCount >= 2)
            {
                // ゲーム終了
            }
            else if (turn == Turn.Player)
            {
                board.UpdateAssist(isAssist, player.DiscType);
                player.Bound.Play();

                if (board.CanPlaceDisc(player.DiscType))
                {
                    passCount = 0;
                    if (enemy.Discs.Count > player.Discs.Count)
                    {
                        selectedDisc = enemy.GetNextDisc();
                        selectedDisc.SetDiscType(player.DiscType);
                    }
                    else
                    {
                        selectedDisc = player.GetNextDisc();
                    }
                }
                else
                {
                    passCount++;
                    restart.interactable = false;
                    sq = Sequence.Pass;
                }
            }
            else
            {
                board.UpdateAssist(isAssist, enemy.DiscType);
                enemy.Bound.Play();

                if (board.CanPlaceDisc(enemy.DiscType))
                {
                    passCount = 0;
                    if (player.Discs.Count > enemy.Discs.Count)
                    {
                        selectedDisc = player.GetNextDisc();
                        selectedDisc.SetDiscType(enemy.DiscType);
                    }
                    else
                    {
                        selectedDisc = enemy.GetNextDisc();
                    }

                    enemy.TryGetReverseDiscs(out var selectedCell, out var selectedReverseDiscs);
                    ExecuteDiscPlacement(selectedCell, selectedReverseDiscs);
                }
                else
                {
                    passCount++;
                    restart.interactable = false;
                    sq = Sequence.Pass;
                }
            }
        }
    }
}
