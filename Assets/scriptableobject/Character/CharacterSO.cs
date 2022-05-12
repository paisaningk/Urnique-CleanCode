using UnityEngine;

namespace scriptableobject.Character
{
    [CreateAssetMenu(menuName = "CharacterSO")]
    public class CharacterSO : ScriptableObject
    {
        public string Name;
        public int MaxHp;
        public int Atk;
        [Range(3f, 15f)]
        public float Speed;
        public GameObject Popup;
        
        public void Print()
        {
            Debug.Log($"My Name is {Name} + MaxHp {MaxHp} + Atk {Atk} + speed {Speed}");
        }
    }

    public enum Character
    {
        Player,
        Enemy,
    }
}
