namespace HereticalSolutions.Persistence
{
    public interface IVisitable
    {
        bool Accept(ISaveVisitor visitor, out object DTO);
        
        bool Accept(ILoadVisitor visitor, object DTO);
    }
}