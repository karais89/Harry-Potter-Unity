﻿using System.Collections.Generic;
using System.Linq;
using HarryPotterUnity.Cards;
using HarryPotterUnity.Tween;
using HarryPotterUnity.UI.Camera;
using UnityEngine;

namespace HarryPotterUnity.Game
{
    public static class GameManager
    {
        public const int PREVIEW_LAYER = 9;
        public const int CARD_LAYER = 10;
        public const int VALID_CHOICE_LAYER = 11;
        public const int IGNORE_RAYCAST_LAYER = 2;
        public const int DECK_LAYER = 12;

        public static bool IsInputGathererActive { get; set; }

        public static byte NetworkIdCounter { get; set; }

        public static readonly List<BaseCard> AllCards = new List<BaseCard>();

        public static readonly PreviewCamera PreviewCamera = GameObject.Find("Preview Camera").GetComponent<PreviewCamera>();

        public static readonly TweenQueue TweenQueue = new TweenQueue();

        public static PhotonView Network { get; set; }
        
        public static bool DebugModeEnabled { get; set; }

        public static List<GameObject> Debug_Player1Deck { private get; set; }
        public static List<GameObject> Debug_Player2Deck { private get; set; }
        public static GameObject Debug_Player1StartingCharacter { private get; set; }
        public static GameObject Debug_Player2StartingCharacter { private get; set; }

        public static List<BaseCard> GetPlayerTestDeck(int playerId)
        {
            var prefabList = playerId == 0
                ? Debug_Player1Deck
                : Debug_Player2Deck;

            return prefabList.Select(o => o.GetComponent<BaseCard>()).ToList();
        }

        public static void DisableCards(List<BaseCard> cards)
        {
            cards.ForEach(card => card.Disable());
        }

        public static void EnableCards(List<BaseCard> cards)
        {
            cards.ForEach(card => card.Enable());
        }

        public static BaseCard GetPlayerTestCharacter(int playerId)
        {
            var obj = playerId == 0
                ? Debug_Player1StartingCharacter
                : Debug_Player2StartingCharacter;

            return obj.GetComponent<BaseCard>();
        }
    }
}
