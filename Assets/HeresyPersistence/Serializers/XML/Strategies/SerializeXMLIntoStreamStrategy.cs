using System.IO;
using System.Text;
using System.Xml.Serialization;

using HereticalSolutions.Persistence.Arguments;
using HereticalSolutions.Persistence.IO;

namespace HereticalSolutions.Persistence.Serializers
{
    public class SerializeXMLIntoStreamStrategy : IXMLSerializationStrategy
    {
        public bool Serialize<TValue>(ISerializationArgument argument, XmlSerializer serializer, TValue value)
        {
            FileSystemSettings fileSystemSettings = ((StreamArgument)argument).Settings;
            
            if (!StreamIO.OpenWriteStream(fileSystemSettings, out var fileStream))
                return false;
            
            using (var streamWriter = new StreamWriter(fileStream))
            {
                serializer.Serialize(streamWriter, value);
            }
            
            StreamIO.CloseStream(fileStream);

            return true;
        }

        public bool Deserialize<TValue>(ISerializationArgument argument, XmlSerializer serializer, out TValue value)
        {
            FileSystemSettings fileSystemSettings = ((StreamArgument)argument).Settings;

            value = default(TValue);
            
            if (!StreamIO.OpenReadStream(fileSystemSettings, out var fileStream))
                return false;

            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                value = (TValue)serializer.Deserialize(streamReader);
            }
            
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