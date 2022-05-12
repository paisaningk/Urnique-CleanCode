using scriptableobject.Character;
using UnityEngine;

namespace Script.Base
{
    public class CharacterBase : MonoBehaviour
    {
        [SerializeField] private CharacterSO Character;
    
        private string Name;
        private int Hp;
        public int Atk;
        private float Speed;

        public virtual void Setup()
        {
            Name = Character.Name;
            Hp = Character.MaxHp;
            Atk = Character.Atk;
            Speed = Character.Speed;
        }

        public virtual void PrintAll()
        {
            Debug.Log($"name:{Name}");
            Debug.Log($"HP:{Hp}");
            Debug.Log($"ATK:{Atk}");
            Debug.Log($"Speed:{Speed}");
        }
    
    }
}
