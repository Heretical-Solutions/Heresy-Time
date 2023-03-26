using System.IO;
using System.Text;

using HereticalSolutions.Persistence.Arguments;
using HereticalSolutions.Persistence.IO;

namespace HereticalSolutions.Persistence.Serializers
{
    public class SerializeYamlIntoStreamStrategy : IYamlSerializationStrategy
    {
        public bool Serialize(ISerializationArgument argument, string yaml)
        {
            FileSystemSettings fileSystemSettings = ((StreamArgument)argument).Settings;
            
            if (!StreamIO.OpenWriteStream(fileSystemSettings, out var fileStream))
                return false;
            
            byte[] data = new UTF8Encoding(true).GetBytes(yaml);
            
            fileStream.Write(data, 0, data.Length);
            
            StreamIO.CloseStream(fileStream);

            return true;
        }

        public bool Deserialize(ISerializationArgument argument, out string yaml)
        {
            FileSystemSettings fileSystemSettings = ((StreamArgument)argument).Settings;
            
            yaml = string.Empty;
            
            if (!StreamIO.OpenReadStream(fileSystemSettings, out var fileStream))
                return false;
            
            var streamReader = new StreamReader(fileStream, Encoding.UTF8);

            yaml = streamReader.ReadToEnd();
            
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