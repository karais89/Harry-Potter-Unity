﻿using System.Collections.Generic;
using System.Linq;
using HarryPotterUnity.Cards.Interfaces;
using HarryPotterUnity.Cards.PlayRequirements;
using UnityEngine;

namespace HarryPotterUnity.Cards.Charms.Spells
{
    [RequireComponent(typeof(InputRequirement))]
    public class StreamOfFlames : BaseSpell, IDamageSpell
    {
        public int DamageAmount { get; set; }

        protected override void Start()
        {
            base.Start();
            DamageAmount = 3;
        }

        public override List<BaseCard> GetFromHandActionTargets()
        {
            return Player.OppositePlayer.InPlay.Creatures;
        }

        protected override void SpellAction(List<BaseCard> targets)
        {
            var creature = targets.First() as BaseCreature;

            if (creature != null)
            {
                creature.TakeDamage(DamageAmount);
            }

            Player.OppositePlayer.TakeDamage(this, DamageAmount);
        }
    }
}