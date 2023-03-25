using System.Runtime.Serialization.Formatters.Binary;

namespace HereticalSolutions.Persistence.Serializers
{
    public interface IBinarySerializationStrategy
    {
        bool Serialize<TValue>(ISerializationArgument argument, BinaryFormatter formatter, TValue value);

        bool Deserialize<TValue>(ISerializationArgument argument, BinaryFormatter formatter, out TValue value);

        void Erase(ISerializationArgument argument);
    }
}