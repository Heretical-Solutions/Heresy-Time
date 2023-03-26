using HereticalSolutions.Persistence.Arguments;
using HereticalSolutions.Persistence.IO;

using ProtobufInternalSerializer = ProtoBuf.Serializer;

namespace HereticalSolutions.Persistence.Serializers
{
    public class SerializeProtobufIntoStreamStrategy : IProtobufSerializationStrategy
    {
        public bool Serialize<TValue>(ISerializationArgument argument, TValue value)
        {
            FileSystemSettings fileSystemSettings = ((StreamArgument)argument).Settings;
            
            if (!StreamIO.OpenWriteStream(fileSystemSettings, out var fileStream))
                return false;
            
            ProtobufInternalSerializer.Serialize(fileStream, value);
            
            StreamIO.CloseStream(fileStream);

            return true;
        }

        public bool Deserialize<TValue>(ISerializationArgument argument, out TValue value)
        {
            FileSystemSettings fileSystemSettings = ((StreamArgument)argument).Settings;
            
            value = default(TValue);
            
            if (!StreamIO.OpenReadStream(fileSystemSettings, out var fileStream))
                return false;
            
            value = ProtobufInternalSerializer.Deserialize<TValue>(fileStream);
            
            StreamIO.CloseStream(fileStream);

            return true;
        }

        public void Erase(ISerializationArgument argument)
        {
            FileSystemSettings fileSystemSettings = ((StreamArgument)argument).Settings;
            
            StreamIO.Erase(fileSystemSettings);
        }
    }
}