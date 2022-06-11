using System;
using UnityEngine;
using UnityEngine.Events;

namespace Components.ColliderBased
{
    // добавляем к пикам, tag Player
// при столкновении объекта Player мы передаем Event с объектом с которым мы столнулись
// добавляем копонент DamageComponent метод ApplyDamage из объекта пик
    public class EnterCollisionComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;

        // Unity не может серриализовать дженерик классы с аргументом класса
        [SerializeField] private EnterEvent _action;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(_tag))
            {
                _action?.Invoke(other.gameObject);
            }
        }
    }
    [Serializable]
// создаем класс и наследуемся с аргументом класса GameObject в качестве типа,
// для вызова other.gameObject
    public class EnterEvent : UnityEvent<GameObject>
    {
    }
}