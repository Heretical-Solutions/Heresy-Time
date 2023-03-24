using System.IO;
using System.Text;

using HereticalSolutions.Persistence.Arguments;
using HereticalSolutions.Persistence.IO;

namespace HereticalSolutions.Persistence.Serializers
{
    public class UnitySerializeJsonIntoStreamStrategy : IJsonSerializationStrategy
    {
        public bool Serialize(ISerializationArgument argument, string json)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityStreamArgument)argument).Settings;
            
            if (!UnityStreamIO.OpenWriteStream(fileSystemSettings, out var fileStream))
                return false;
            
            byte[] data = new UTF8Encoding(true).GetBytes(json);
            
            fileStream.Write(data, 0, data.Length);
            
            UnityStreamIO.CloseStream(fileStream);

            return true;
        }

        public bool Deserialize(ISerializationArgument argument, out string json)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityStreamArgument)argument).Settings;
            
            json = string.Empty;
            
            if (!UnityStreamIO.OpenReadStream(fileSystemSettings, out var fileStream))
                return false;
            
            var streamReader = new StreamReader(fileStream, Encoding.UTF8);

            json = streamReader.ReadToEnd();
            
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