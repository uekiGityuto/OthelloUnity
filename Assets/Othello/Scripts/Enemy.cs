using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Othello
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] Sprite[] charaSprites;
        [SerializeField] Sprite[] orderSprites;
        [SerializeField] Image chara;
        [SerializeField] BoundAnimation bound;
        [SerializeField] Image order;
        [SerializeField] Transform discSpace;
        Difficulty difficulty;
        DiscType discType;
        List<Disc> discs = new List<Disc>();

        public BoundAnimation Bound => bound;
        public Transform DiscSpace => discSpace;
        public DiscType DiscType => discType;
        public List<Disc> Discs => discs;

        public void Init(DiscType discType, bool isFirstTurn, Difficulty difficulty)
        {
            this.discType = discType;
            this.difficulty = difficulty;

            chara.sprite = charaSprites[(int)difficulty];
            order.sprite = orderSprites[(int)discType];

            var x = 0f;
            for (var i = 0; i < 30; i++)
            {
                var disc = Instantiate(Resources.Load<Disc>("Prefabs/Disc"), discSpace);
                disc.Init(discType);

                var pos = disc.transform.localPosition;
                pos.x = x;
                disc.transform.localPosition = pos;

                x += Player.DiscLayoutSpacing;
                if ((i + 1) % 5 == 0)
                {
                    x += Player.DiscLayoutSpacing5;
                }

                disc.transform.eulerAngles = new Vector3(0, 0, -90);
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
