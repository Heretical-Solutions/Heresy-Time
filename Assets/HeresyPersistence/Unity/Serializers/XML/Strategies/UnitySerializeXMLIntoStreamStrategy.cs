using System.IO;
using System.Text;
using System.Xml.Serialization;

using HereticalSolutions.Persistence.Arguments;
using HereticalSolutions.Persistence.IO;

namespace HereticalSolutions.Persistence.Serializers
{
    public class UnitySerializeXMLIntoStreamStrategy : IXMLSerializationStrategy
    {
        public bool Serialize<TValue>(ISerializationArgument argument, XmlSerializer serializer, TValue value)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityStreamArgument)argument).Settings;
            
            if (!UnityStreamIO.OpenWriteStream(fileSystemSettings, out var fileStream))
                return false;
            
            using (var streamWriter = new StreamWriter(fileStream))
            {
                serializer.Serialize(streamWriter, value);
            }
            
            UnityStreamIO.CloseStream(fileStream);

            return true;
        }

        public bool Deserialize<TValue>(ISerializationArgument argument, XmlSerializer serializer, out TValue value)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityStreamArgument)argument).Settings;

            value = default(TValue);
            
            if (!UnityStreamIO.OpenReadStream(fileSystemSettings, out var fileStream))
                return false;

            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                value = (TValue)serializer.Deserialize(streamReader);
            }
            
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