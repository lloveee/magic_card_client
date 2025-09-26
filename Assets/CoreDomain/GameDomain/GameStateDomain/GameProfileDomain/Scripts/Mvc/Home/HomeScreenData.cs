using System;
using CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.SO;
using SpacetimeDB.Types;
using Unity.Properties;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Mvc.Home
{
    public class HomeScreenData
    {
        private PlayerAccount _playerAccount;
        private readonly RankTextureMapSO _rankTextureMap;

        public HomeScreenData(PlayerAccount player, RankTextureMapSO rankTextureMap)
        {
            _playerAccount = player;
            _rankTextureMap = rankTextureMap;
        }

        [CreateProperty]
        public string PlayerName
        {
            get => _playerAccount.Nickname;
            set
            {
                if (_playerAccount.Nickname != value)
                {
                    _playerAccount.Nickname = value;
                    return;
                }
            }
        }

        [CreateProperty]
        public string PlayerRank
        {
            get => _playerAccount.Rank.ToString();
            set
            {
                if (_playerAccount.Rank.ToString() != value)
                {
                    // 假设 Rank 是 int 或 enum，需要转换
                    if (uint.TryParse(value, out uint rankInt))
                    {
                        _playerAccount.Rank = rankInt;
                    }
                }
            }
        }

        public void IncreaseRank(uint rank)
        {
            _playerAccount.Rank += rank;
        }
        
        public void DecreaseRank(uint rank)
        {
            _playerAccount.Rank = (uint)Mathf.Clamp((int)_playerAccount.Rank - rank, 0, Int32.MaxValue);
        }

        [CreateProperty]
        public Texture2D RankIcon => _rankTextureMap.GetTexture(_playerAccount.Rank);
    }
}