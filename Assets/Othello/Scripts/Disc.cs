using UnityEngine;
using UnityEngine.UI;

namespace Othello
{
    public class Disc : MonoBehaviour
    {
        [SerializeField] Sprite[] discSprites;
        [SerializeField] Image image;
        DiscType discType;
        int reverseIdx;

        public DiscType DiscType => discType;
        public int ReverseIdx { get => reverseIdx; set => reverseIdx = value; }

        public void SetDiscType(DiscType discType)
        {
            this.discType = discType;
            name = discType.ToString();
            image.sprite = discSprites[(int)discType];
        }

        public void Init(DiscType discType)
        {
            SetDiscType(discType);
        }
    }
}
