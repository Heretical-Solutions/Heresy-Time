using System.IO;
using System.Xml.Serialization;

using HereticalSolutions.Persistence.Arguments;
using HereticalSolutions.Persistence.IO;

namespace HereticalSolutions.Persistence.Serializers
{
    public class UnitySerializeXmlIntoTextFileStrategy : IXmlSerializationStrategy
    {
        public bool Serialize<TValue>(ISerializationArgument argument, XmlSerializer serializer, TValue value)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityTextFileArgument)argument).Settings;

            string xml;
            
            using (StringWriter stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, value);
                
                xml = stringWriter.ToString();
            }
            
            return UnityTextFileIO.Write(fileSystemSettings, xml);
        }

        public bool Deserialize<TValue>(ISerializationArgument argument, XmlSerializer serializer, out TValue value)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityTextFileArgument)argument).Settings;
            
            bool result = UnityTextFileIO.Read(fileSystemSettings, out string xml);

            if (!result)
            {
                value = default(TValue);
                
                return false;
            }

            using (StringReader stringReader = new StringReader(xml))
            {
                value = (TValue)serializer.Deserialize(stringReader);
            }

            return true;
        }
        
        public void Erase(ISerializationArgument argument)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityTextFileArgument)argument).Settings;
            
            UnityTextFileIO.Erase(fileSystemSettings);
        }
    }
}