namespace HereticalSolutions.Persistence
{
    public interface IVisitable
    {
        void Accept(ISaveVisitor visitor);
        
        void Accept(ILoadVisitor visitor);
    }
}