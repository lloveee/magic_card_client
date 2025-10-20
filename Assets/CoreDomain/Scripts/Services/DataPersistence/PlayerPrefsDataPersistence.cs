using System;
using CoreDomain.Scripts.Services.Serializer;
using UnityEngine;
using Zenject;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.Scripts.Services.DataPersistence
{
    public class PlayerPrefsDataPersistence : IDataPersistence
    {
        private readonly ISerializerService _serializer;
        private readonly ILogger _logger;

        [Inject]
        public PlayerPrefsDataPersistence(ISerializerService serializer, ILogger logger)
        {
            _serializer = serializer;
            _logger = logger;
        }

        public void Save<T>(string id, T data)
        {
            try
            {
                var json = _serializer.SerializeJson(data);
                //var encrypted = EncryptionUtils.Encrypt(json);
                PlayerPrefs.SetString(id, json);
                PlayerPrefs.Save();
                _logger.Log("save prefs!!");
            }
            catch (Exception e)
            {
                _logger.LogError($"Tried to save {id}, but exception was thrown: {e}");
            }
        }

        public T Load<T>(string id, T defaultValue = default)
        {
            try
            {
                if (!PlayerPrefs.HasKey(id))
                    return defaultValue;

                var json = PlayerPrefs.GetString(id);
                //var json = EncryptionUtils.Decrypt(encrypted);
                return _serializer.DeserializeJson<T>(json);
            }
            catch (Exception e)
            {
                _logger.LogError($"Tried to load {id}, but exception was thrown: {e}");
                throw;
            }
        }
    }
}