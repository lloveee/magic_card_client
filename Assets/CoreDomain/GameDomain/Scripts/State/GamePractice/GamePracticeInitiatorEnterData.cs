using System;
using CoreDomain.Scripts.CoreInitiator.Base;

namespace CoreDomain.GameDomain.Scripts.State.GamePractice
{
    [Serializable]
    public class GamePracticeInitiatorEnterData : IInitiatorEnterData
    {
        public GamePracticeInitiatorEnterData(string username)
        {
            Username = username;
        }
        public string Username;
    }
}