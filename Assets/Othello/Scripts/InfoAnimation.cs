using TMPro;
using UnityEngine;

namespace Othello
{
    public class InfoAnimation : MonoBehaviour
    {
        const string Pass = "パス";
        const string GameEnd = "ゲーム終了";

        enum Sequence { None, Delay, FallFadeIn, Display, FallFadeOut }

        [SerializeField] GameObject back;
        [SerializeField] TextMeshProUGUI message;
        [SerializeField] float delay = 0.5f;
        [SerializeField] int fallHeight = 70;
        [SerializeField] float fallDuration = 0.1f;
        [SerializeField] float displayDuration = 1;


        Sequence sq;
        float time;
        float startY;

        public bool IsPlaying => sq != Sequence.None;

        void Start()
        {
            startY = message.transform.localPosition.y;
        }

        void Update()
        {
            if (sq == Sequence.Delay)
            {
                time += Time.deltaTime;
                if (time >= delay)
                {
                    SetAlpha(0);
                    back.SetActive(true);
                    message.gameObject.SetActive(true);

                    sq = Sequence.FallFadeIn;
                    time = 0;
                }
            }
            else if (sq == Sequence.FallFadeIn)
            {
                time += Time.deltaTime;
                if (time < fallDuration)
                {
                    var rate = time / fallDuration;
                    var a = 1 * rate;
                    var y = startY + fallHeight - fallHeight * rate;
                    SetAlpha(a);
                    SetY(y);
                }
                else
                {
                    SetAlpha(1);
                    SetY(startY);

                    sq = Sequence.Display;
                    time = 0;
                }
            }
            else if (sq == Sequence.Display)
            {
                time += Time.deltaTime;
                if (time >= displayDuration)
                {
                    sq = Sequence.FallFadeOut;
                    time = 0;
                }
            }
            else if (sq == Sequence.FallFadeOut)
            {
                time += Time.deltaTime;
                if (time < fallDuration)
                {
                    var rate = time / fallDuration;
                    var a = 1 - 1 * rate;
                    var y = startY - fallHeight * rate;
                    SetAlpha(a);
                    SetY(y);
                }
                else
                {
                    SetAlpha(0);
                    SetY(fallHeight);
                    back.SetActive(false);
                    message.gameObject.SetActive(false);

                    sq = Sequence.None;
                    time = 0;
                }
            }
        }

        public void Play(string mes)
        {
            sq = Sequence.Delay;
            message.text = mes;
        }

        public void Stop()
        {
            sq = Sequence.None;
            time = 0;
            SetAlpha(0);
            SetY(fallHeight);
            back.SetActive(false);
            message.gameObject.SetActive(false);
        }

        void SetAlpha(float a)
        {
            var color = message.color;
            color.a = a;
            message.color = color;
        }

        void SetY(float y)
        {
            var pos = message.transform.localPosition;
            pos.y = y;
            message.transform.localPosition = pos;
        }

        public void PlayPass()
        {
            Play(Pass);
        }

        public void PlayGameEnd()
        {
            Play(GameEnd);
        }

    }
}
