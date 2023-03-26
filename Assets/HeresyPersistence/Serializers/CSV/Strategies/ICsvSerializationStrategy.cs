namespace HereticalSolutions.Persistence.Serializers
{
    public interface ICsvSerializationStrategy
    {
        bool Serialize<TValue>(ISerializationArgument argument, TValue value);
        
        bool Deserialize<TValue>(ISerializationArgument argument, out TValue value);

        void Erase(ISerializationArgument argument);
    }
}