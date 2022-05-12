using UnityEngine;

namespace Assets.scriptableobject.Item
{
    [CreateAssetMenu(menuName = "ItemSO")]
    public class ItemSO : ScriptableObject
    {
        public Sprite Sprite;
        public Tier Tier;
        public int MaxHp;
        public int Atk;
        public float Speed;
        public float DashCd;
        public int CritAtk;
        public int CritRate;
        public string text;
        //public int Price;

        // Atk    ค่าพลังโจมตีของผู้เล่น
        // Speed    ค่าความเร็วในการเดินของผู้เล่น
        // HP    ค่าพลังชีวิตของผู้เล่น
        // DashCD    ค่าคูลดาวน์การแดชของผู้เล่น
        // CritAtk    ค่าดาเมจคริของผู้เล่น
        // CritRate    ค่าอัตราการเกิดคริติคอล
        // FOW    ระยะการมองเห็นของผู้เล่นตอนกลางคืน ----- ยังไม่ได้ใส่
        // AtkSpeed    ค่าระยะเวลาความห่างระหว่างโจมตีแต่หล่ะครั้ง ------ ยังไม่ได้ใส่
    }
}
