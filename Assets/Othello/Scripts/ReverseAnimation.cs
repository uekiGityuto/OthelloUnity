using UnityEngine;

namespace Othello
{
    public class ReverseAnimation : MonoBehaviour
    {
        enum Sequence { None, Wait, Rotation90, Rotation180 }

        [SerializeField] Disc disc;
        [SerializeField] float delay = 0.15f;
        [SerializeField] float duration = 0.2f;
        [SerializeField] Vector3 axis = new Vector3(0, 1, 0);
        Sequence sq;
        float time;
        float waitTime;
        DiscType reversedDiscType;

        public bool IsPlaying => sq != Sequence.None;

        void Update()
        {
            if (sq == Sequence.Wait)
            {
                time += Time.deltaTime;
                if (time >= waitTime)
                {
                    sq = Sequence.Rotation90;
                    time = 0;
                }
            }
            else if (sq == Sequence.Rotation90)
            {
                time += Time.deltaTime;
                if (time < duration)
                {
                    var rate = time / duration;
                    var angle = 90 * rate;
                    disc.transform.rotation = Quaternion.AngleAxis(angle, axis);
                }
                else
                {
                    disc.transform.rotation = Quaternion.AngleAxis(90, axis);
                    disc.SetDiscType(reversedDiscType);

                    sq = Sequence.Rotation180;
                    time = 0;
                }
            }
            else if (sq == Sequence.Rotation180)
            {
                time += Time.deltaTime;
                if (time < duration)
                {
                    var rate = time / duration;
                    var angle = 90 + 90 * rate;
                    disc.transform.rotation = Quaternion.AngleAxis(angle, axis);
                }
                else
                {
                    disc.transform.rotation = Quaternion.Euler(0, 0, 0);

                    sq = Sequence.None;
                    time = 0;
                }
            }
        }

        public void Play(DiscType discType)
        {
            sq = Sequence.Wait;
            waitTime = delay * disc.ReverseIdx;
            reversedDiscType = discType;
        }

        public void Stop()
        {
            sq = Sequence.None;
            time = 0;
            disc.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
