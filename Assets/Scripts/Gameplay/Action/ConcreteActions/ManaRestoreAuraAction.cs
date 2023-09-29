using System;
using Unity.BossRoom.Gameplay.GameplayObjects.Character;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace Unity.BossRoom.Gameplay.Actions
{
    [CreateAssetMenu(menuName = "BossRoom/Actions/Mana Restore Aura Action")]
    public partial class ManaRestoreAuraAction : Action
    {
        float time = 0;

        public override bool OnStart(ServerCharacter serverCharacter)
        {
            serverCharacter.serverAnimationHandler.NetworkAnimator.SetTrigger(Config.Anim);
            serverCharacter.clientCharacter.RecvDoActionClientRPC(Data);
            return true;
        }

        public override bool OnUpdate(ServerCharacter clientCharacter)
        {
            time += Time.deltaTime;
            if (time > 1)
            {
                ApplyAuraEffect(clientCharacter);
                time = time % 1;
            }
            return true;
        }

        private void ApplyAuraEffect(ServerCharacter caster)
        {
            int layerMask = caster.IsNpc ? LayerMask.GetMask("NPCs") : LayerMask.GetMask("PCs");

            var colliders = Physics.OverlapSphere(caster.transform.position, Config.Radius, layerMask);
            for (var i = 0; i < colliders.Length; i++)
            {
                ServerCharacter affectedCharacter = colliders[i].GetComponent<ServerCharacter>();
                if (affectedCharacter != null)
                {
                    AuraEffect(affectedCharacter);
                }
            }

        }

        protected virtual void AuraEffect(ServerCharacter target)
        {
            target.ChangeMana(Config.Amount);
        }
    }
}
