using UnityEngine;

namespace CoreDomain.Scripts.Utils
{
    [CreateAssetMenu(fileName = "Default Config", menuName = "Server/Config", order = 0)]
    public class ServerConfig : ScriptableObject
    {
        public string url = "http://localhost:3000";
        public string module = "mc";
    }
}