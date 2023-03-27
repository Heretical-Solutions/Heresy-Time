using System.IO;
using System.Xml.Serialization;

using HereticalSolutions.Persistence.Arguments;
using HereticalSolutions.Persistence.IO;

namespace HereticalSolutions.Persistence.Serializers
{
    public class UnitySerializeXmlIntoStreamStrategy : IXmlSerializationStrategy
    {
        public bool Serialize<TValue>(ISerializationArgument argument, XmlSerializer serializer, TValue value)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityStreamArgument)argument).Settings;
            
            if (!UnityStreamIO.OpenWriteStream(fileSystemSettings, out StreamWriter streamWriter))
                return false;
            
            serializer.Serialize(streamWriter, value);
            
            UnityStreamIO.CloseStream(streamWriter);

            return true;
        }

        public bool Deserialize<TValue>(ISerializationArgument argument, XmlSerializer serializer, out TValue value)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityStreamArgument)argument).Settings;

            value = default(TValue);
            
            if (!UnityStreamIO.OpenReadStream(fileSystemSettings, out StreamReader streamReader))
                return false;

            value = (TValue)serializer.Deserialize(streamReader);
            
            UnityStreamIO.CloseStream(streamReader);

            return true;
        }

        public void Erase(ISerializationArgument argument)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityStreamArgument)argument).Settings;
            
            UnityStreamIO.Erase(fileSystemSettings);
        }
    }
}