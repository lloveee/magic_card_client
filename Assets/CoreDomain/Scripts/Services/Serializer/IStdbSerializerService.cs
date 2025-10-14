using System.Collections.Generic;
using SpacetimeDB.BSATN;

namespace CoreDomain.Scripts.Services.Serializer
{
    public interface IStdbSerializerService
    {
        public string SerializeData<T, S>(List<T> data, S serializer)
            where T : IStructuralReadWrite
            where S : IReadWrite<T>;

        public List<T> DeserializeData<T, S>(string data, S serializer)
            where T : IStructuralReadWrite
            where S : IReadWrite<T>;
    }
}