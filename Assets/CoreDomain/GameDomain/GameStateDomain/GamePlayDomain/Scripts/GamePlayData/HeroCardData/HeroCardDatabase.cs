using System;
using System.Collections.Generic;
using System.Linq;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData.HeroData;
using CoreDomain.Scripts.Services.SpacetimeServer;
using SpacetimeDB;
using SpacetimeDB.Types;
using UnityEngine;
using Zenject;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData
{
    [CreateAssetMenu(fileName = "HeroCardDatabase", menuName = "GamePlay/HeroCardDatabase", order = 0)]
    public class HeroCardDatabase : ScriptableObject
    {
        [SerializeField] private string url = "http://localhost:3000";
        [SerializeField] private string module = "c-mc";
        [SerializeField] public List<HeroCardSO> data;
        private ISpacetimeServer _spacetime;
        private ILogger _logger;

        [Inject]
        public void Construct(ISpacetimeServer spacetime, ILogger logger)
        {
            _spacetime = spacetime;
            _logger = logger;
        }

        public void TryConnectSpacetimeDb()
        {
            _spacetime.InitializeConnection(url, module, OnConnected, OnConnectError, OnDisconnected);
        }

        private void OnDisconnected(DbConnection conn, Exception e)
        {
            _logger.Log("Disconnected");
        }

        private void OnConnectError(Exception e)
        {
            _logger.LogError(e.Message);
        }

        private void OnConnected(DbConnection conn, Identity identity, string token)
        {
            _spacetime.LocalIdentity = identity;
            AuthToken.SaveToken(token);
            _logger.Log("Connected Management Admin");
            _spacetime.Conn.Reducers.OnBulkInsertOrUpdateHeroCard += Reducer_OnUpdateHeroCard;
            _spacetime.Conn.Reducers.OnReInsertHeroCard += Reducer_OnReInsertHeroCard;
            //TryUpdateData();
        }

        public void TryUpdateData()
        {
            _spacetime.Conn.Reducers.BulkInsertOrUpdateHeroCard(data.Select(d => d.TryUpdateData()).ToList());
        }

        public void TryReUpdateData()
        {
            _spacetime.Conn.Reducers.ReInsertHeroCard(data.Select(d => d.TryUpdateData()).ToList());
        }

        public void TryValidateData(RemoteReducers.TryValidateHeroCardHandler Reducer_ValidateHeroCard)
        {
            _spacetime.Conn.Reducers.OnTryValidateHeroCard += Reducer_ValidateHeroCard;
            _spacetime.Conn.Reducers.TryValidateHeroCard(data.Select(d => d.TryUpdateData()).ToList());
        }

        private void Reducer_OnUpdateHeroCard(ReducerEventContext ctx, List<HeroCard> cards)
        {
            var e = ctx.Event;
            if (e.CallerIdentity == _spacetime.LocalIdentity)
            {
                if (e.Status is Status.Failed(var error))
                {
                    _logger.Log($"{error}");
                }
                else if (e.Status is Status.Committed)
                {
                    foreach (var card in cards)
                    {
                        _logger.Log($"{card.HeroCardId} | {card.CardName} | {card.CardDescription} | {card.Stats}");
                    }
                }
            }
        }

        private void Reducer_OnReInsertHeroCard(ReducerEventContext ctx, List<HeroCard> cards)
        {
            var e = ctx.Event;
            if (e.CallerIdentity == _spacetime.LocalIdentity)
            {
                if (e.Status is Status.Failed(var error))
                {
                    _logger.Log($"{error}");
                }
                else if (e.Status is Status.Committed)
                {
                    foreach (var card in cards)
                    {
                        _logger.Log($"{card.HeroCardId} | {card.CardName} | {card.CardDescription} | {card.Stats}");
                    }
                }
            }
        }
        
        public void RefreshDatabase()
        {
            data.Clear();
            var loadedCards = Resources.LoadAll<HeroCardSO>("");
            data.AddRange(loadedCards);
        }
        /*
        private void OnDisable()
        {
            _spacetime.Conn.Reducers.OnBulkInsertOrUpdateHeroCard -= Reducer_OnUpdateHeroCard;
        }*/
    }
}