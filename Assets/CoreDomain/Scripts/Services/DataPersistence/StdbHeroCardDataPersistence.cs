using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using CoreDomain.Scripts.Services.Serializer;
using SpacetimeDB.Types;
using UnityEngine;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.Scripts.Services.DataPersistence
{
    public class StdbHeroCardDataPersistence : IDataPersistence
    {
        private readonly IStdbSerializerService _stdbSerializer;
        private readonly ILogger _logger;
        private const string CacheFile = "hero_card_cache.json";
        private readonly string _dbPath;

        public StdbHeroCardDataPersistence(IStdbSerializerService stdbSerializer, ILogger logger)
        {
            _stdbSerializer = stdbSerializer;
            _logger = logger;
            _dbPath = Path.Combine(Application.persistentDataPath, CacheFile);
            _logger.Log($"{_dbPath}");
        }
        
        public void Save(List<HeroCard> data)
        {
            try
            {
                var play_load = _stdbSerializer.SerializeData(data, new HeroCard.BSATN());
                // encrypt
                File.WriteAllText(_dbPath, play_load);
            }
            catch (Exception e)
            {
                _logger.LogException(e);
            }
        }
        
        public void Save(string data)
        {
            try
            {
                //var play_load = _stdbSerializer.SerializeData(data, new HeroCard.BSATN());
                // encrypt
                File.WriteAllText(_dbPath, data);
            }
            catch (Exception e)
            {
                _logger.LogException(e);
            }
        }

        public List<HeroCard> ConvertSerializeDataToList(string data)
        {
            return _stdbSerializer.DeserializeData<HeroCard, HeroCard.BSATN>(data, new HeroCard.BSATN());
        }

        public async Awaitable<string> Load(CancellationTokenSource cancellationTokenSource)
        {
            if (File.Exists(_dbPath))
            {
                var data = await File.ReadAllTextAsync(_dbPath, cancellationTokenSource.Token);
                return data;
            }
            return "";
        }
    }
}