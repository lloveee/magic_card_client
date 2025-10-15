using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData.HeroData;
using CoreDomain.Scripts.Services.DataPersistence;
using CoreDomain.Scripts.Services.SpacetimeServer;
using CoreDomain.Scripts.Utils;
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
        private StdbHeroCardDataPersistence _heroCardData;
        private ILogger _logger;
        
        private const string k_hero_card_play_load = "data_signature";

        [Inject]
        public void Construct(ISpacetimeServer spacetime, ILogger logger, StdbHeroCardDataPersistence heroCardDataPersistence)
        {
            _spacetime = spacetime;
            _heroCardData = heroCardDataPersistence;
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
        }

        public void TryUpdateData()
        {
            _spacetime.Conn.Reducers.BulkInsertOrUpdateHeroCard(data.Select(d => d.GetHeroCardData()).ToList());
        }

        public void TryReUpdateData()
        {
            _spacetime.Conn.Reducers.ReInsertHeroCard(data.Select(d => d.GetHeroCardData()).ToList());
        }

        public async Awaitable TryLoadRemoteData(CancellationTokenSource cancellation)
        {
            var play_load = await _heroCardData.Load(cancellation);
            var tcs = AwaitableUtils.CreateLinkedTcs<bool>(cancellation.Token);
            _spacetime.Conn.Reducers.OnVerifyData +=(ctx, target, c_data) => Reducer_VerifyData(ctx, target, c_data, tcs);
            _spacetime.Conn.Reducers.VerifyData("HeroCard", play_load);
            await tcs.Task;
        }
        
        public void TryValidateData(CancellationTokenSource cancellation, RemoteReducers.TryValidateHeroCardHandler Reducer_ValidateHeroCard)
        {
            _spacetime.Conn.Reducers.OnTryValidateHeroCard += Reducer_ValidateHeroCard;
            _spacetime.Conn.Reducers.TryValidateHeroCard(data.Select(d => d.GetHeroCardData()).ToList());
        }

        private void Reducer_VerifyData(ReducerEventContext ctx, string target, string playLoad, TaskCompletionSource<bool> tcs)
        {
            var e = ctx.Event;
            if (e.CallerIdentity == _spacetime.LocalIdentity)
            {
                if (e.Status is Status.Failed(var error))
                {
                    //load remote data
                    string data_signature_sub_query =  $"SELECT * FROM {k_hero_card_play_load} c WHERE c.ValidateInfoId = 'HeroCard'";
                    _spacetime.SubscribeTableWithId(k_hero_card_play_load, new string[]{data_signature_sub_query}
                    , (context) => OnDataSignatureSub(context, tcs)
                    , (errorContext, exception) => OnDataSignatureSubError(errorContext, exception, tcs)
                    );
                }
                else if (e.Status is Status.Committed)
                {
                    var list  = _heroCardData.ConvertSerializeDataToList(playLoad);
                    for (int i = 0; i < data.Count; i++)
                    {
                        data[i].SetHeroCardData(list[i]);
                    }

                    tcs.TrySetResult(true);
                    _logger.Log("Update Data from cache");
                }
            }
        }

        private void OnDataSignatureSubError(ErrorContext ctx, Exception e, TaskCompletionSource<bool> tcs)
        {
            _logger.LogWarning("Network error");
            tcs.TrySetException(e);
        }

        private void OnDataSignatureSub(SubscriptionEventContext context, TaskCompletionSource<bool> tcs)
        {
            _logger.Log("Subscription data_signature applied");
            var dataSignature = context.Db.DataSignature.ValidateInfoId.Find("HeroCard");
            if (dataSignature != null)
            {
                _heroCardData.Save(dataSignature.PlayLoad);
                var list = _heroCardData.ConvertSerializeDataToList(dataSignature.PlayLoad);
                for (int i = 0; i < data.Count; i++)
                {
                    data[i].SetHeroCardData(list[i]);
                }
                //
                _logger.Log("Update Data From Remote");
            }
            
            tcs.TrySetResult(true);
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