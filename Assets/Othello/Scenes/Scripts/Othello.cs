using UnityEngine;
using Random = UnityEngine.Random;

namespace Othello
{

    public enum Difficulty { Normal, Hard }
    public enum PlayFirst { Player, Enemy, Random }
    public enum DiscType { Black, White }
    public enum Turn { Player, Enemy }


    public class Othello : MonoBehaviour
    {

        [SerializeField] Difficulty difficulty;
        [SerializeField] PlayFirst playFirst;
        [SerializeField] bool isAssist;
        [SerializeField] StartMenu startMenu;

        public Difficulty Difficulty { get => difficulty; set => difficulty = value; }
        public PlayFirst PlayFirst { get => playFirst; set => playFirst = value; }
        public bool IsAssist { get => isAssist; set => isAssist = value; }

        public void OnGameStartClick()
        {
            Turn firstTurn;
            var r = Random.Range(0, 2);
            Debug.Log(r);
            if (playFirst == PlayFirst.Player || (playFirst == PlayFirst.Random && r == 0))
            {
                firstTurn = Turn.Player;
            }
            else
            {
                firstTurn = Turn.Enemy;
            }

            startMenu.gameObject.SetActive(false);
        }

    }
}
