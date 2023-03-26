using System.IO;
using System.Text;

using HereticalSolutions.Persistence.Arguments;
using HereticalSolutions.Persistence.IO;

namespace HereticalSolutions.Persistence.Serializers
{
    public class UnitySerializeYamlIntoStreamStrategy : IYamlSerializationStrategy
    {
        public bool Serialize(ISerializationArgument argument, string yaml)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityStreamArgument)argument).Settings;
            
            if (!UnityStreamIO.OpenWriteStream(fileSystemSettings, out var fileStream))
                return false;
            
            byte[] data = new UTF8Encoding(true).GetBytes(yaml);
            
            fileStream.Write(data, 0, data.Length);
            
            UnityStreamIO.CloseStream(fileStream);

            return true;
        }

        public bool Deserialize(ISerializationArgument argument, out string yaml)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityStreamArgument)argument).Settings;
            
            yaml = string.Empty;
            
            if (!UnityStreamIO.OpenReadStream(fileSystemSettings, out var fileStream))
                return false;
            
            var streamReader = new StreamReader(fileStream, Encoding.UTF8);

            yaml = streamReader.ReadToEnd();
            
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