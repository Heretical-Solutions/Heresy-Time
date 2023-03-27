using System.IO;

using HereticalSolutions.Persistence.Arguments;
using HereticalSolutions.Persistence.IO;

using ProtobufInternalSerializer = ProtoBuf.Serializer;

namespace HereticalSolutions.Persistence.Serializers
{
    public class UnitySerializeProtobufIntoStreamStrategy : IProtobufSerializationStrategy
    {
        public bool Serialize<TValue>(ISerializationArgument argument, TValue value)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityStreamArgument)argument).Settings;
            
            if (!UnityStreamIO.OpenWriteStream(fileSystemSettings, out FileStream fileStream))
                return false;
            
            ProtobufInternalSerializer.Serialize(fileStream, value);
            
            UnityStreamIO.CloseStream(fileStream);

            return true;
        }

        public bool Deserialize<TValue>(ISerializationArgument argument, out TValue value)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityStreamArgument)argument).Settings;
            
            value = default(TValue);
            
            if (!UnityStreamIO.OpenReadStream(fileSystemSettings, out FileStream fileStream))
                return false;
            
            value = ProtobufInternalSerializer.Deserialize<TValue>(fileStream);
            
            UnityStreamIO.CloseStream(fileStream);

            return true;
        }

        public void Erase(ISerializationArgument argument)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityStreamArgument)argument).Settings;
            
            UnityStreamIO.Erase(fileSystemSettings);
        }
    }
}