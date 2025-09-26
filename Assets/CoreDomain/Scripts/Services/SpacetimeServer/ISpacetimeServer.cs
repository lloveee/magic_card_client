using System;
using JetBrains.Annotations;
using SpacetimeDB;
using SpacetimeDB.Types;

namespace CoreDomain.Scripts.Services.SpacetimeServer
{
    public interface ISpacetimeServer
    {
        public DbConnection Conn { get; }
        public Identity LocalIdentity { get; set; }

        public void InitializeConnection(string url, string module,
            DbConnectionBuilder<DbConnection>.ConnectCallback on_connect_callback = null,
            DbConnectionBuilder<DbConnection>.ConnectErrorCallback on_error_callback = null,
            DbConnectionBuilder<DbConnection>.DisconnectCallback on_disconnect_callback = null);

        public void SubscribeTableWithId(string key, string[] querySql, [CanBeNull] Action<SubscriptionEventContext> on_apply_callback
            , [CanBeNull] Action<ErrorContext, Exception> on_error_callback);

        public void UnsubscribeTableWithId(string key,
            [CanBeNull] Action<SubscriptionEventContext> on_unapply_callback);
    }
}