namespace HereticalSolutions.Persistence
{
    public interface ILoadVisitorGeneric<TValue, TDTO>
    {
        bool Load(TDTO DTO, out TValue value);
    }
}