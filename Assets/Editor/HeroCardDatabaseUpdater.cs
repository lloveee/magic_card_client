using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class HeroCardDatabaseUpdater
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void UpdateDatabaseOnPlay()
        {
            var db = Resources.Load<HeroCardDatabase>("HeroCardSO/HeroCardDatabase");
            if (db)
            {
                db.RefreshDatabase();
                EditorUtility.SetDirty(db);
                AssetDatabase.SaveAssets();
                Debug.Log($"HeroCardDatabase 已刷新并保存，共找到 {db.data.Count} 张卡牌。");
            }
        }
    }
}