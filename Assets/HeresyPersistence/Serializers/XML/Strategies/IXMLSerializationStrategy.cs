using System.Xml.Serialization;

namespace HereticalSolutions.Persistence.Serializers
{
    public interface IXMLSerializationStrategy
    {
        bool Serialize<TValue>(ISerializationArgument argument, XmlSerializer serializer, TValue value);
        
        bool Deserialize<TValue>(ISerializationArgument argument, XmlSerializer serializer, out TValue value);

        void Erase(ISerializationArgument argument);
    }
}