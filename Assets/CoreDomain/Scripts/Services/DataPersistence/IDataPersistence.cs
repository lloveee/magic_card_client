using System.Collections.Generic;
using System.Threading;
using SpacetimeDB.Types;
using UnityEngine;

namespace CoreDomain.Scripts.Services.DataPersistence
{
    public interface IDataPersistence
    {
        public void Save(List<HeroCard> data);
        public Awaitable<string> Load(CancellationTokenSource cancellationTokenSource);
    }
}