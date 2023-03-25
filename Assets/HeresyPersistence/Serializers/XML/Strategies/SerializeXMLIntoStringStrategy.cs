using System.IO;
using System.Xml.Serialization;

using HereticalSolutions.Persistence.Arguments;

namespace HereticalSolutions.Persistence.Serializers
{
    public class SerializeXMLIntoStringStrategy : IXMLSerializationStrategy
    {
        public bool Serialize<TValue>(ISerializationArgument argument, XmlSerializer serializer, TValue value)
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, value);
                
                ((StringArgument)argument).Value = stringWriter.ToString();
            }
            
            return true;
        }

        public bool Deserialize<TValue>(ISerializationArgument argument, XmlSerializer serializer, out TValue value)
        {
            using (StringReader stringReader = new StringReader(((StringArgument)argument).Value))
            {
                value = (TValue)serializer.Deserialize(stringReader);
            }
            
            return true;
        }
        
        public void Erase(ISerializationArgument argument)
        {
            ((StringArgument)argument).Value = string.Empty;
        }
    }
}