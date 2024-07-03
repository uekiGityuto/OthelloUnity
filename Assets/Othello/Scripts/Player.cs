using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Othello
{
    public class Player : MonoBehaviour
    {
        public const int DiscLayoutSpacing = 18;
        public const int DiscLayoutSpacing5 = 15;

        [SerializeField] Sprite[] orderSprites;
        [SerializeField] Image order;
        [SerializeField] Transform discSpace;
        DiscType discType;
        List<Disc> discs = new List<Disc>();

        public Transform DiscSpace => discSpace;
        public DiscType DiscType => discType;
        public List<Disc> Discs => discs;

        public void Init(DiscType discType, bool isFirstTurn)
        {
            this.discType = discType;
            order.sprite = orderSprites[(int)discType];

            var x = 0f;
            for (var i = 0; i < 30; i++)
            {
                var disc = Instantiate(Resources.Load<Disc>("Prefabs/Disc"), discSpace);
                disc.Init(discType);

                var pos = disc.transform.localPosition;
                pos.x = x;
                disc.transform.localPosition = pos;

                x -= DiscLayoutSpacing;
                if ((i + 1) % 5 == 0)
                {
                    x -= DiscLayoutSpacing5;
                }

                disc.transform.eulerAngles = new Vector3(0, 0, 90);
                discs.Add(disc);
            }
        }

        public Disc GetNextDisc()
        {
            var disc = discs[discs.Count - 1];
            discs.RemoveAt(discs.Count - 1);
            disc.gameObject.SetActive(false);
            return disc;
        }
    }
}
