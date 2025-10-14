using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using SpacetimeDB.BSATN;

namespace CoreDomain.Scripts.Services.Serializer
{
    public class StdbStdbSerializerService : IStdbSerializerService
    {
        public string SerializeData<T, S>(List<T> data, S serializer) where T : IStructuralReadWrite where S : IReadWrite<T>
        {
            using var ms = new MemoryStream();
            using var writer = new BinaryWriter(ms);
            
            foreach (var card in data)
            {
                serializer.Write(writer, card);
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        public List<T> DeserializeData<T, S>(string data, S serializer) where T : IStructuralReadWrite where S : IReadWrite<T>
        {
            var bytes = Convert.FromBase64String(data);
            using var ms = new MemoryStream(bytes);
            using var reader = new BinaryReader(ms);

            var result = new List<T>();

            while (ms.Position < ms.Length)
            {
                var card = serializer.Read(reader);
                result.Add(card);
            }

            return result;
        }
    }
}