using System;
using System.Collections.Generic;
using CoreDomain.Scripts.Services.Logger;
using JetBrains.Annotations;
using SpacetimeDB;
using SpacetimeDB.Types;
using Zenject;

namespace CoreDomain.Scripts.Services.SpacetimeServer
{
    public class SpacetimeServer : ISpacetimeServer
    {
        public DbConnection Conn { get; private set; }
        public Identity LocalIdentity { get; set; }

        private Dictionary<string, SubscriptionHandle> subscriptions = new ();

        private readonly ILogger _logger;
        
        [Inject]
        public SpacetimeServer(ILogger logger)
        {
            _logger = logger;
        }
        
        public void InitializeConnection(string url, string module, 
            DbConnectionBuilder<DbConnection>.ConnectCallback on_connect_callback = null,
            DbConnectionBuilder<DbConnection>.ConnectErrorCallback on_error_callback = null, 
            DbConnectionBuilder<DbConnection>.DisconnectCallback on_disconnect_callback = null)
        {
            if (Conn != null)
            {
                _logger.LogWarning("Connection is already initialized");
                return;
            }

            var builder = DbConnection.Builder()
                .WithUri(url)
                .WithModuleName(module);

            if (on_connect_callback != null) builder = builder.OnConnect(on_connect_callback);
            if (on_error_callback != null) builder = builder.OnConnectError(on_error_callback);
            if (on_disconnect_callback != null) builder = builder.OnDisconnect(on_disconnect_callback);

            if (AuthToken.Token != "")
            {
                builder = builder.WithToken(AuthToken.Token);
            }
            Conn = builder.Build();
        }

        public void SubscribeTableWithId(string key, string[] querySql, Action<SubscriptionEventContext> on_apply_callback, Action<ErrorContext, Exception> on_error_callback)
        {
            if (subscriptions.ContainsKey(key))
            {
                _logger.LogWarning("Subscription with key " + key + " is already exist");
                //subscriptions.Remove(key);
                return;
            }

            var builder = Conn.SubscriptionBuilder();
            if (on_apply_callback != null) builder = builder.OnApplied(on_apply_callback);
            if (on_error_callback != null) builder = builder.OnError(on_error_callback);

            var handle = builder.Subscribe(querySql);
            subscriptions.Add(key, handle);
        }
        
        public void UnsubscribeTableWithId(string key, Action<SubscriptionEventContext> on_unapply_callback)
        {
            if (!subscriptions.TryGetValue(key, out var handle))
            {
                _logger.LogWarning("Subscription with key " + key + " is not exist");
                return;
            }
            if (on_unapply_callback != null) handle.UnsubscribeThen(on_unapply_callback);
            else handle.Unsubscribe();
            subscriptions.Remove(key);
        }

    }
}