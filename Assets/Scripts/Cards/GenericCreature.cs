﻿using UnityEngine;
using System.Collections;
using LessonTypes = Lesson.LessonTypes;

public class GenericCreature : GenericCard, PersistentCard {

	// Use this for initialization

    public LessonTypes CostType;

    public int CostAmount;

    public int DamagePerTurn;
    public int Health;

	public new void Start ()
    {
        base.Start();
	}

    public void OnMouseUp()
    {
        if (State != CardStates.IN_HAND) return;

        if (_Player.UseAction())
        {
            if(_Player.nLessonsInPlay >= CostAmount && _Player.LessonTypesInPlay.Contains(CostType))
            {
                _Player._Hand.Remove(this);
                _Player._InPlay.Add(this);
            }
        }
    }

    public void OnEnterInPlayAction()
    {
        _Player.nCreaturesInPlay++;
        _Player.DamagePerTurn += DamagePerTurn;

        State = CardStates.IN_PLAY;
    }

    public void OnExitInPlayAction()
    {
        _Player.nCreaturesInPlay--;
        _Player.DamagePerTurn -= DamagePerTurn;

        State = CardStates.DISCARDED;
    }

    //Generic Creatures don't do anything special on these actions
    public void OnInPlayBeforeTurnAction() { }
    public void OnInPlayAfterTurnAction() { }
    public void OnSelectedAction() { }
  
}
