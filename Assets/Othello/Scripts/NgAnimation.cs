using UnityEngine;
using UnityEngine.UI;

namespace Othello
{
    public class NgAnimation : MonoBehaviour
    {
        enum Sequence { None, Display, FadeOut }

        [SerializeField] Image image;
        [SerializeField] float displayDuration = 0.3f;
        [SerializeField] float fadeOutDuration = 0.15f;


        Sequence sq;
        float time;

        public bool IsPlaying => sq != Sequence.None;

        void Update()
        {
            if (sq == Sequence.Display)
            {
                Debug.Log("Display");
                time += Time.deltaTime;
                if (time >= displayDuration)
                {
                    Debug.Log("Display end");
                    sq = Sequence.FadeOut;
                    Debug.Log(sq);
                    time = 0;
                }
            }
            else if (sq == Sequence.FadeOut)
            {
                Debug.Log("FadeOut");
                time += Time.deltaTime;
                if (time < fadeOutDuration)
                {
                    var rate = time / fadeOutDuration;
                    var a = 1 - 1 * rate;
                    SetAlpha(a);
                }
                else
                {
                    SetAlpha(0);
                    gameObject.SetActive(false);

                    sq = Sequence.None;
                    time = 0;
                }
            }
        }

        public void Play()
        {
            sq = Sequence.Display;
            SetAlpha(1);
            gameObject.SetActive(true);
        }

        public void Stop()
        {
            sq = Sequence.None;
            time = 0;
            SetAlpha(0);
            gameObject.SetActive(false);
        }

        void SetAlpha(float a)
        {
            var color = image.color;
            color.a = a;
            image.color = color;
        }

    }
}
