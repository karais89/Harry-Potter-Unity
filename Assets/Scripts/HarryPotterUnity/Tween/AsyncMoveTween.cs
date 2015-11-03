﻿using System;
using System.Collections.Generic;
using HarryPotterUnity.Cards;
using UnityEngine;

namespace HarryPotterUnity.Tween
{
    class AsyncMoveTween : ITweenObject
    {

        private readonly List<BaseCard> _targets;
        private readonly Func<BaseCard, Vector3> _getPosition;
        private readonly float _timeUntilNextTween;

        public AsyncMoveTween(List<BaseCard> targets, Func<BaseCard, Vector3> getPositionFunction, float timeUntilNextTween = 0f)
        {
            _targets = targets;
            _getPosition = getPositionFunction;
            _timeUntilNextTween = timeUntilNextTween;
        }

        public float CompletionTime
        {
            get { return 0.2f; }
        }

        public float TimeUntilNextTween
        {
            get { return _timeUntilNextTween; }
        }

        public void ExecuteTween()
        {
            foreach (var target in _targets)
            {
                iTween.MoveTo(target.gameObject, iTween.Hash(
                "time", 0.2f,
                "position", _getPosition(target),
                "easetype", iTween.EaseType.EaseInOutSine,
                "islocal", true
                ));    
            }
        }
    }
}
