using System.Runtime.Serialization.Formatters.Binary;

using HereticalSolutions.Persistence.Arguments;
using HereticalSolutions.Persistence.IO;

namespace HereticalSolutions.Persistence.Serializers
{
    public class UnitySerializeBinaryIntoStreamStrategy : IBinarySerializationStrategy
    {
        private readonly BinaryFormatter formatter = new BinaryFormatter();
        
        public bool Serialize<TValue>(ISerializationArgument argument, TValue value)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityStreamArgument)argument).Settings;
            
            if (!UnityStreamIO.OpenWriteStream(fileSystemSettings, out var fileStream))
                return false;
            
            formatter.Serialize(fileStream, value);
            
            UnityStreamIO.CloseStream(fileStream);

            return true;
        }

        public bool Deserialize<TValue>(ISerializationArgument argument, out TValue value)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityStreamArgument)argument).Settings;
            
            value = default(TValue);
            
            if (!UnityStreamIO.OpenReadStream(fileSystemSettings, out var fileStream))
                return false;
            
            value = (TValue)formatter.Deserialize(fileStream);
            
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