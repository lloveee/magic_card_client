using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.SO
{
    [CreateAssetMenu(fileName = "RankTextureMapSO", menuName = "GameProfile/RankTextureMapSO", order = 0)]
    public class RankTextureMapSO : ScriptableObject
    {
        [SerializeField] private List<Texture2D> rankStaticTexture;

        public Texture2D GetTexture(uint rank)
        {
            int index = Mathf.Clamp((int)rank / 100, 0, rankStaticTexture.Count - 1);
            return rankStaticTexture[index];
        }
    }
}