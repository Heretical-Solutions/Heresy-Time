using System.IO;
using System.Xml.Serialization;

using HereticalSolutions.Persistence.Arguments;
using HereticalSolutions.Persistence.IO;

namespace HereticalSolutions.Persistence.Serializers
{
    public class SerializeXMLIntoTextFileStrategy : IXMLSerializationStrategy
    {
        public bool Serialize<TValue>(ISerializationArgument argument, XmlSerializer serializer, TValue value)
        {
            FileSystemSettings fileSystemSettings = ((TextFileArgument)argument).Settings;

            string xml;
            
            using (StringWriter stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, value);
                
                xml = stringWriter.ToString();
            }
            
            return TextFileIO.Write(fileSystemSettings, xml);
        }

        public bool Deserialize<TValue>(ISerializationArgument argument, XmlSerializer serializer, out TValue value)
        {
            FileSystemSettings fileSystemSettings = ((TextFileArgument)argument).Settings;

            value = default(TValue);
            
            bool result = TextFileIO.Read(fileSystemSettings, out string xml);

            if (!result)
                return false;
            
            using (StringReader stringReader = new StringReader(xml))
            {
                value = (TValue)serializer.Deserialize(stringReader);
            }

            return true;
        }
        
        public void Erase(ISerializationArgument argument)
        {
            FileSystemSettings fileSystemSettings = ((TextFileArgument)argument).Settings;
            
            TextFileIO.Erase(fileSystemSettings);
        }
    }
}