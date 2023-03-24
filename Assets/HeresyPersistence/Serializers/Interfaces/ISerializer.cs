namespace HereticalSolutions.Persistence
{
    public interface ISerializer
    {
        bool Serialize<TValue>(ISerializationArgument argument, TValue DTO);
        
        bool Deserialize<TValue>(ISerializationArgument argument, out TValue DTO);

        void Erase(ISerializationArgument argument);
    }
}