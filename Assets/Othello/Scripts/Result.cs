using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Othello
{
    public class Result : MonoBehaviour
    {
        enum Sequence { None, Delay, DiscBound, DiscBounding, End }


        [SerializeField] Player player;
        [SerializeField] Enemy enemy;
        [SerializeField] Board board;
        [SerializeField] Disc playerDisc;
        [SerializeField] TextMeshProUGUI playerDiscCount;
        [SerializeField] Disc enemyDisc;
        [SerializeField] TextMeshProUGUI enemyDiscCount;
        [SerializeField] GameObject back;
        [SerializeField] GameObject[] results;
        [SerializeField] float startDelay = 0.5f;
        [SerializeField] float lastDelay = 1;

        Sequence sq;
        float time;
        List<Disc> discs = new List<Disc>();
        int discIdx;
        int nowPlayerDiscCount;
        int nowEnemyDiscCount;
        ResultType resultType;

        public bool IsPlaying => sq != Sequence.None;

        void Update()
        {
            if (sq == Sequence.Delay)
            {
                time += Time.deltaTime;
                if (time >= startDelay)
                {
                    sq = Sequence.DiscBound;
                }
            }
            else if (sq == Sequence.DiscBound)
            {
                discs[discIdx].Bound.Play();
                sq = Sequence.DiscBounding;
            }
            else if (sq == Sequence.DiscBounding)
            {
                if (!discs[discIdx].Bound.IsPlaying)
                {
                    discs[discIdx].gameObject.SetActive(false);

                    if (discs[discIdx].DiscType == player.DiscType)
                    {
                        nowPlayerDiscCount++;
                        playerDiscCount.text = nowPlayerDiscCount.ToString();
                        playerDisc.Bound.Stop();
                        playerDisc.Bound.Play();
                    }
                    else
                    {
                        nowEnemyDiscCount++;
                        enemyDiscCount.text = nowEnemyDiscCount.ToString();
                        enemyDisc.Bound.Stop();
                        enemyDisc.Bound.Play();
                    }

                    discIdx++;
                    if (discIdx < discs.Count)
                    {
                        sq = Sequence.DiscBound;
                    }
                    else
                    {
                        if (nowPlayerDiscCount > nowEnemyDiscCount)
                        {
                            resultType = ResultType.Win;
                        }
                        else if (nowPlayerDiscCount < nowEnemyDiscCount)
                        {
                            resultType = ResultType.Lose;
                        }
                        else
                        {
                            resultType = ResultType.Draw;
                        }

                        sq = Sequence.End;
                        time = 0;
                    }
                }
            }
            else if (sq == Sequence.End)
            {
                time += Time.deltaTime;
                if (time >= lastDelay)
                {
                    back.SetActive(true);
                    results[(int)resultType].SetActive(true);

                    sq = Sequence.None;
                    time = 0;
                }
            }
        }

        public void Play()
        {
            player.DiscSpace.gameObject.SetActive(false);
            playerDisc.SetDiscType(player.DiscType);
            playerDisc.gameObject.SetActive(true);

            enemy.DiscSpace.gameObject.SetActive(false);
            enemyDisc.SetDiscType(enemy.DiscType);
            enemyDisc.gameObject.SetActive(true);

            for (var y = 0; y < Board.Row; y++)
            {
                for (var x = 0; x < Board.Column; x++)
                {
                    var cell = board.Cells[x, y];
                    if (cell.Disc != null)
                    {
                        discs.Add(cell.Disc);
                    }
                }
            }

            sq = Sequence.Delay;
        }

        public void Stop()
        {
        }

    }
}
