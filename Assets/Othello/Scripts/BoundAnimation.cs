using UnityEngine;

namespace Othello
{
    public class BoundAnimation : MonoBehaviour
    {
        enum Sequence { None, ScaleUp, ScaleDown }

        [SerializeField] float deltaScale = 0.1f;
        [SerializeField] float duration = 0.1f;
        Sequence sq;
        float time;

        public bool IsPlaying => sq != Sequence.None;

        void Update()
        {
            if (sq == Sequence.ScaleUp)
            {
                time += Time.deltaTime;
                if (time < duration)
                {
                    var rate = time / duration;
                    var scale = 1 + deltaScale * rate;
                    transform.localScale = new Vector3(scale, scale);
                }
                else
                {
                    var scale = 1 + deltaScale;
                    transform.localScale = new Vector3(scale, scale);
                    sq = Sequence.ScaleDown;
                    time = 0;
                }
            }
            else if (sq == Sequence.ScaleDown)
            {
                time += Time.deltaTime;
                if (time < duration)
                {
                    var rate = time / duration;
                    var scale = 1 + deltaScale - deltaScale * rate;
                    transform.localScale = new Vector3(scale, scale);
                }
                else
                {
                    transform.localScale = Vector3.one;

                    sq = Sequence.None;
                    time = 0;
                }
            }
        }

        public void Play()
        {
            sq = Sequence.ScaleUp;
        }

        public void Stop()
        {
            sq = Sequence.None;
            time = 0;
            transform.localScale = Vector3.one;
        }

    }
}
