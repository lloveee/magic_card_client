using System;
using CoreDomain.Scripts.CoreInitiator.Base;
using SpacetimeDB;

namespace CoreDomain.Scripts.CoreInitiator
{
    [Serializable]
    public class GameInitiatorEnterData : IInitiatorEnterData
    {
        public Identity LocalIdentity;

        public GameInitiatorEnterData()
        {
            LocalIdentity = new Identity();
            
        }
        public GameInitiatorEnterData(Identity identity)
        {
            LocalIdentity = identity;
        }
    }
}