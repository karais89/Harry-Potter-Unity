﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hand : MonoBehaviour {

    public const int SPACING = 25;
    private List<Transform> Cards;

	// Use this for initialization
	void Start () {
        Cards = new List<Transform>();
	}

    public void Add(Transform card)
    {
        Cards.Add(card);
    }

    public int NumberOfCards()
    {
        return Cards.Count;
    }
}
