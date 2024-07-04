using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

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
        [SerializeField] Player player;
        [SerializeField] Enemy enemy;
        [SerializeField] Board board;


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
        }

        public void OnCellClick(Cell cell)
        {
            if (board.CanPlaceDisc(cell, player.DiscType))
            {
                var disc = player.GetNextDisc();
                board.PlaceDisc(cell, disc);
            }

        }

        public void OnRestartClick()
        {
            SceneManager.LoadScene("Othello");
        }
    }
}
