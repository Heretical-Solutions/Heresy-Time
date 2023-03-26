using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using HereticalSolutions.Persistence.Arguments;
using HereticalSolutions.Persistence.IO;

namespace HereticalSolutions.Persistence.Serializers
{
    public class SerializeBinaryIntoStreamStrategy : IBinarySerializationStrategy
    {
        public bool Serialize<TValue>(ISerializationArgument argument, BinaryFormatter formatter, TValue value)
        {
            FileSystemSettings fileSystemSettings = ((StreamArgument)argument).Settings;
            
            if (!StreamIO.OpenWriteStream(fileSystemSettings, out FileStream fileStream))
                return false;
            
            formatter.Serialize(fileStream, value);
            
            StreamIO.CloseStream(fileStream);

            return true;
        }

        public bool Deserialize<TValue>(ISerializationArgument argument, BinaryFormatter formatter, out TValue value)
        {
            FileSystemSettings fileSystemSettings = ((StreamArgument)argument).Settings;
            
            value = default(TValue);
            
            if (!StreamIO.OpenReadStream(fileSystemSettings, out FileStream fileStream))
                return false;
            
            value = (TValue)formatter.Deserialize(fileStream);
            
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