namespace CoreDomain.Scripts.Services.Serializer
{
    public interface ISerializerService
    {
        string SerializeJson<T>(T obj);
        T DeserializeJson<T>(string json);
    }
}