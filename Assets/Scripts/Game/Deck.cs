﻿using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {

    public List<GenericCard> Cards;

    public Player _Player;

    private readonly Vector2 _deckPositionOffset = new Vector2(-355f, -124f);

    public float DeckShuffleTweenTime = 0.5f;

	// Use this for initialization
	public void Start () {
        //instantiate cards into scene
        Vector3 cardPos = new Vector3(_deckPositionOffset.x, _deckPositionOffset.y, 0f);
        for (int i = 0; i < Cards.Count; i++)
        {
            Cards[i] = (GenericCard)Instantiate(Cards[i]);
            Cards[i].transform.parent = transform;
            Cards[i].transform.localPosition = cardPos + Vector3.back * -16f;
            Cards[i].transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, _Player.transform.rotation.eulerAngles.z));
            Cards[i].transform.position += i * Vector3.back * 0.2f;

            Cards[i]._Player = _Player;
        }

        //Set the collider to the proper position
        if (gameObject.collider == null)
        {
            var col = gameObject.AddComponent<BoxCollider>();
            col.isTrigger = true;
            col.size = new Vector3(50f, 70f, 1f);
            col.center = new Vector3(_deckPositionOffset.x, _deckPositionOffset.y, 0f);
        }

        //tell the player to draw his hand after this is all done
        _Player.DrawInitialHand();
	}
	
    public GenericCard TakeTopCard()
    {
        if (Cards.Count <= 0)
        {
            return null;
        }

        GenericCard card = Cards[Cards.Count - 1];
        Cards.RemoveAt(Cards.Count - 1);
        return card;
    }

    public void OnMouseUp()
    {
        if (Cards.Count <= 0 || !_Player.CanUseAction()) return;

        DrawCard();
        _Player.UseAction();
    }

    public void DrawCard()
    {
       var card = TakeTopCard();
       if (card == null)
       {
           //GameOver
           return;
       }

       _Player._Hand.Add(card);
    }

    public void Shuffle()
    {
        for (var i = Cards.Count-1; i >= 0; i--)
        {
            var random = Random.Range(0, i);

            var temp = Cards[i];
            Cards[i] = Cards[random];
            Cards[random] = temp;

            var newZ = (transform.position.z + 16f) - i * 0.2f;

            var point1 = new Vector3(Cards[i].transform.position.x, Cards[i].transform.position.y + 80, Cards[i].transform.position.z);
            var point2 = new Vector3(Cards[i].transform.position.x, Cards[i].transform.position.y, newZ);

            iTween.MoveTo(Cards[i].gameObject, iTween.Hash("time", DeckShuffleTweenTime, 
                                                           "path", new[] {point1, point2}, 
                                                           "easetype", iTween.EaseType.easeInOutSine, 
                                                           "delay", Random.Range(0f,1.5f))
                                                           );
        }
    }

    public void Disable()
    {
        gameObject.layer = Helper.IgnoreRaycastLayer;
    }

    public void Enable()
    {
        gameObject.layer = Helper.DeckLayer;
    }
}
