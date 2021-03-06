﻿using System.Collections.Generic;
using System.Linq;
using HarryPotterUnity.Cards;
using HarryPotterUnity.Cards.Interfaces;
using HarryPotterUnity.Enums;
using HarryPotterUnity.Tween;
using UnityLogWrapper;
using UnityEngine;
using Type = HarryPotterUnity.Enums.Type;

namespace HarryPotterUnity.Game
{
    public class InPlay : CardCollection
    {
        private static readonly Vector3 _lessonPositionOffset = new Vector3(-255f, -60f, 15f);
        private static readonly Vector3 _topRowPositionOffset = new Vector3(-255f, 0f, 15f);
        private static readonly Vector3 _creaturePositionOffset = new Vector3(5f, -60f, 15f);
        private static readonly Vector3 _characterPositionOffset = new Vector3(-356f, -207, 15f);

        private static readonly Vector2 _lessonSpacing = new Vector2(80f, 15f);
        private static readonly Vector2 _topRowSpacing = new Vector2(80f, 0f);
        private static readonly Vector2 _creatureSpacing = new Vector2(80f, 55f);
        private static readonly Vector2 _characterSpacing = new Vector2(80f, 27f);

        public delegate void CardEnteredPlayEvent(BaseCard card);
        public delegate void CardExitedPlayEvent(BaseCard card);

        public event CardEnteredPlayEvent OnCardEnteredPlay;
        public event CardExitedPlayEvent  OnCardExitedPlay;

        private void OnDestroy()
        {
            OnCardEnteredPlay = null;
            OnCardExitedPlay = null;
        }

        public override void Add(BaseCard card)
        {
            Cards.Add(card);
            
            card.transform.parent = transform;

            TweenCardToPosition(card);

            MoveToThisCollection(card);

            ((IPersistentCard) card).OnEnterInPlayAction();

            if (OnCardEnteredPlay != null) OnCardEnteredPlay(card);
        }

        protected override void Remove(BaseCard card)
        {
            Cards.Remove(card);

            RearrangeCardsOfType(card.Type);

            ((IPersistentCard) card).OnExitInPlayAction();

            if (OnCardExitedPlay != null) OnCardExitedPlay(card);
        }

        public override void AddAll(IEnumerable<BaseCard> cards)
        {
            var cardList = cards as IList<BaseCard> ?? cards.ToList();

            foreach (var card in cardList)
            {
                Add(card);
            }
        }
        
        protected override void RemoveAll(IEnumerable<BaseCard> cards)
        {
            var cardList = cards as IList<BaseCard> ?? cards.ToList();

            foreach (var card in cardList)
            {
                Cards.Remove(card);

                ((IPersistentCard)card).OnExitInPlayAction();

                if (OnCardExitedPlay != null) OnCardExitedPlay(card);
            }

            foreach (var type in cardList.GroupBy(c => c.Type))
            {
                RearrangeCardsOfType(type.Key);
            }
        }
        
        private void TweenCardToPosition(BaseCard card)
        {
            var tween = new MoveTween
            {
                Target = card.gameObject,
                Position = GetTargetPositionForCard(card),
                Time = 0.3f,
                Flip = FlipState.FaceUp,
                Rotate = TweenRotationType.Rotate90,
                OnCompleteCallback = () => card.State = State.InPlay
            };
            GameManager.TweenQueue.AddTweenToQueue( tween );
        }

        private void RearrangeCardsOfType(Type type)
        {
            var targets = 
                type.IsTopRow() ? 
                    Cards.FindAll(c => c.Type.IsTopRow()) : 
                    Cards.FindAll(c => c.Type == type);

            var tween = new AsyncMoveTween
            {
                Targets = targets,
                GetPosition = GetTargetPositionForCard
            };
            GameManager.TweenQueue.AddTweenToQueue(tween);
        }

        private Vector3 GetTargetPositionForCard(BaseCard card)
        {
            if (!Cards.Contains(card)) return card.transform.localPosition;

            int position = Cards.FindAll(c => c.Type == card.Type).IndexOf(card);
            
            var cardPosition = new Vector3();

            switch (card.Type)
            {
                case Type.Lesson:
                    cardPosition = _lessonPositionOffset;
                    cardPosition.x += (position % 3) * _lessonSpacing.x;
                    cardPosition.y -= (int)(position / 3) * _lessonSpacing.y;
                    cardPosition.z -= (int)(position / 3);
                    break;
                case Type.Creature:
                    cardPosition = _creaturePositionOffset;
                    cardPosition.x += (position % 4) * _creatureSpacing.x;
                    cardPosition.y -= (int)(position / 4) * _creatureSpacing.y;
                    cardPosition.z -= (int)(position / 4);
                    break;
                case Type.Character:
                    cardPosition = _characterPositionOffset;
                    cardPosition.x += (position % 2) * _characterSpacing.x * 0.5f;
                    cardPosition.y -= (int) (position/2)*_characterSpacing.y;

                    int zPosOffset = 2*(position/2);
                    if (position%2 == 1)
                    {
                        zPosOffset -= 2;
                    }

                    cardPosition.z -= zPosOffset;
                    break;
                case Type.Item:
                case Type.Location:
                case Type.Adventure:
                case Type.Match:
                    int topRowIndex = Cards.FindAll(c => c.Type.IsTopRow()).IndexOf(card);
                    float shrinkFactor = 0.5f;
                    cardPosition = _topRowPositionOffset;
                    cardPosition.x += topRowIndex * _topRowSpacing.x * shrinkFactor;
                    cardPosition.z -= topRowIndex;
                    break;
                default:
                    Log.Error("GetTargetPositionForCard could not identify cardType");
                    break;
            }

            

            return cardPosition;
        }
    }
}
