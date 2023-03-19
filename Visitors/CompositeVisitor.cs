namespace HereticalSolutions.Persistence.Visitors
{
    public class CompositeVisitor 
        : ISaveVisitor, 
          ILoadVisitor
    {
        public bool Save<TValue>(TValue value, out object DTO)
        {
            throw new System.NotImplementedException();
        }

        public bool Save<TValue, TDTO>(TValue value, out TDTO DTO)
        {
            throw new System.NotImplementedException();
        }

        public bool Load<TValue>(object DTO, out TValue value)
        {
            throw new System.NotImplementedException();
        }

        public bool Load<TValue, TDTO>(TDTO DTO, out TValue value)
        {
            throw new System.NotImplementedException();
        }
    }
}