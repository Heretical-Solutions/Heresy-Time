namespace HereticalSolutions.Persistence
{
    public interface ILoadVisitor
    {
        bool Load<TValue>(object DTO, out TValue value);
        
        bool Load<TValue, TDTO>(TDTO DTO, out TValue value);
    }
}