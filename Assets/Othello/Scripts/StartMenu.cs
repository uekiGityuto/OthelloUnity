using UnityEngine;
using UnityEngine.UI;

namespace Othello
{
    public class StartMenu : MonoBehaviour
    {
        [SerializeField] Othello othello;
        [SerializeField] Sprite[] charaSprites;
        [SerializeField] Sprite[] difficultySprites;
        [SerializeField] Sprite[] playFirstSprites;
        [SerializeField] Sprite[] assistSprites;
        [SerializeField] Image charaImage;
        [SerializeField] Image difficultyImage;
        [SerializeField] Image playFirstImage;
        [SerializeField] Image assistImage;
        [SerializeField] Board board;

        void Start()
        {
            board.Init();
            board.PlaceDiscDirect(3, 3, DiscType.White);
            board.PlaceDiscDirect(4, 3, DiscType.Black);
            board.PlaceDiscDirect(3, 4, DiscType.Black);
            board.PlaceDiscDirect(4, 4, DiscType.White);

            SetDifficulty(othello.Difficulty);
            SetPlayFist(othello.PlayFirst);
            SetAssist(othello.IsAssist);
        }


        void SetDifficulty(Difficulty difficulty)
        {
            othello.Difficulty = difficulty;
            difficultyImage.sprite = difficultySprites[(int)difficulty];
            charaImage.sprite = charaSprites[(int)difficulty];
        }

        void SetPlayFist(PlayFirst playFirst)
        {
            othello.PlayFirst = playFirst;
            playFirstImage.sprite = playFirstSprites[(int)playFirst];
        }

        void SetAssist(bool isAssist)
        {
            othello.IsAssist = isAssist;
            assistImage.sprite = assistSprites[isAssist ? 1 : 0];
            board.UpdateAssist(isAssist, DiscType.Black);
        }

        public void OnDifficultyRightClick()
        {
            var value = (int)othello.Difficulty;
            value++;
            if (value > difficultySprites.Length - 1)
            {
                value = 0;
            }
            SetDifficulty((Difficulty)value);
        }

        public void OnDifficultyLeftClick()
        {
            var value = (int)othello.Difficulty;
            value--;
            if (value < 0)
            {
                value = difficultySprites.Length - 1;
            }
            SetDifficulty((Difficulty)value);
        }

        public void OnPlayFistRightClick()
        {
            var value = (int)othello.PlayFirst;
            value++;
            if (value > playFirstSprites.Length - 1)
            {
                value = 0;
            }
            SetPlayFist((PlayFirst)value);
        }

        public void OnPlayFistLeftClick()
        {
            var value = (int)othello.PlayFirst;
            value--;
            if (value < 0)
            {
                value = playFirstSprites.Length - 1;
            }
            SetPlayFist((PlayFirst)value);
        }

        public void OnAssistClick()
        {
            SetAssist(!othello.IsAssist);
        }


    }
}
