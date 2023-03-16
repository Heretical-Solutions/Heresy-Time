namespace HereticalSolutions.Persistence
{
    public interface ISerializer
    {
        bool Serialize<TValue>(object medium, TValue DTO);
        
        bool Deserialize<TValue>(object medium, out TValue DTO);
    }
}