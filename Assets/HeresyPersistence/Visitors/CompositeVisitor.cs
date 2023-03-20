using System;
using HereticalSolutions.Repositories;

namespace HereticalSolutions.Persistence.Visitors
{
    public class CompositeVisitor 
        : ISaveVisitor, 
          ILoadVisitor
    {
        private IReadOnlyObjectRepository loadVisitorsRepository;

        private IReadOnlyRepository<Type, object> saveVisitorRepository;
        
        public bool Save<TValue>(TValue value, out object DTO)
        {
            throw new System.NotImplementedException();
        }

        public bool Save<TValue, TDTO>(TValue value, out TDTO DTO)
        {
            if (!saveVisitorRepository.TryGet(typeof(TDTO), out object concreteVisitorObject))
                throw new Exception();
            
            var concreteVisitor = (ISaveVisitorGeneric<TValue, TDTO>)concreteVisitorObject;
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