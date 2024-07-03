using UnityEngine;
using UnityEngine.UI;

namespace Othello
{
    public class Disc : MonoBehaviour
    {
        [SerializeField] Sprite[] discSprites;
        [SerializeField] Image image;
        DiscType discType;

        public DiscType DiscType => discType;

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
