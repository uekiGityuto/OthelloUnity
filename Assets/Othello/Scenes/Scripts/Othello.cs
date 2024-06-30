using UnityEngine;

namespace Othello
{

    public enum Difficulty { Normal, Hard }
    public enum PlayFirst { Player, Enemy, Random }
    public enum DiscType { Black, White }


    public class Othello : MonoBehaviour
    {

        [SerializeField] Difficulty difficulty;
        [SerializeField] PlayFirst playFirst;
        [SerializeField] bool isAssist;

        public Difficulty Difficulty { get => difficulty; set => difficulty = value; }
        public PlayFirst PlayFirst { get => playFirst; set => playFirst = value; }
        public bool IsAssist { get => isAssist; set => isAssist = value; }

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("ハローてんぷら！");
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
