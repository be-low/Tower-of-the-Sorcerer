using UnityEngine;

namespace Billow
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField]
        public int Hp = 1000, AttachPower = 10, DefensivePower = 10, CoinCount = 10;

        public bool BattleWith(Fighter other)
        {
            int damage1 = AttachPower - other.DefensivePower;
            if (damage1 <= 0) return false;
            int damage2 = other.AttachPower - DefensivePower;
            int hp1 = Hp, hp2 = other.Hp;
            while (hp1 >= 0 && hp2 >= 0)
            {
                hp1 -= damage2;
                hp2 -= damage1;
            }

            if (hp1 >= 0)
            {
                CoinCount += other.CoinCount;
                Hp = hp1;
                return true;
            }

            return false;
        }
    }
}