using System;
using CoreDomain.Scripts.CoreInitiator.Base;
using SpacetimeDB.Types;
using UnityEngine;

namespace CoreDomain.GameDomain.Scripts.State.GameProfileState
{
    [Serializable]
    public class GameProfileInitiatorEnterData : IInitiatorEnterData
    {
        public PlayerAccount CurrentPlayer;

        public GameProfileInitiatorEnterData()
        {
            CurrentPlayer = new PlayerAccount("MockUser", "MockNick", new MatchInfo(66, 0), 1000, 1);
        }
        public GameProfileInitiatorEnterData(PlayerAccount player)
        {
            CurrentPlayer = player;
        }
    }
}