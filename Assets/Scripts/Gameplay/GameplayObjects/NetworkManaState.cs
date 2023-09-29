using System;
using Unity.Netcode;
using UnityEngine;

namespace Unity.BossRoom.Gameplay.GameplayObjects
{
    /// <summary>
    /// MonoBehaviour containing only one NetworkVariableInt which represents this object's Mana.
    /// </summary>
    public class NetworkManaState : NetworkBehaviour
    {
        [HideInInspector]
        public NetworkVariable<int> Mana = new NetworkVariable<int>();

        // public subscribable event to be invoked when Mana has been fully depleted
        public event Action ManaDepleted;

        // public subscribable event to be invoked when Mana has been replenished
        public event Action ManaReplenished;

        void OnEnable()
        {
            Mana.OnValueChanged += ManaChanged;
        }

        void OnDisable()
        {
            Mana.OnValueChanged -= ManaChanged;
        }

        void ManaChanged(int previousValue, int newValue)
        {
            if (previousValue > 0 && newValue <= 0)
            {
                ManaDepleted?.Invoke();
            }
            else if (previousValue <= 0 && newValue > 0)
            {
                ManaReplenished?.Invoke();
            }

            Debug.Log($"mana: {newValue}");

        }
    }
}
