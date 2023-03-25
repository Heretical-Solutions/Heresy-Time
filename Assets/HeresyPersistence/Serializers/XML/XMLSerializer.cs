using System;
using System.Xml.Serialization;

using HereticalSolutions.Repositories;

namespace HereticalSolutions.Persistence.Serializers
{
    public class XMLSerializer : ISerializer
    {
        private readonly IReadOnlyObjectRepository strategyRepository;
        
        public XMLSerializer(IReadOnlyObjectRepository strategyRepository)
        {

            this.strategyRepository = strategyRepository;
        }
        
        #region ISerializer
        
        public bool Serialize<TValue>(ISerializationArgument argument, TValue DTO)
        {
            var serializer = new XmlSerializer(typeof(TValue));
            
            if (!strategyRepository.TryGet(argument.GetType(), out var strategyObject))
                throw new Exception($"[XMLSerializer] COULD NOT RESOLVE STRATEGY BY ARGUMENT: {argument.GetType().ToString()}");

            var concreteStrategy = (IXMLSerializationStrategy)strategyObject;

            return concreteStrategy.Serialize<TValue>(argument, serializer, DTO);
        }

        public bool Deserialize<TValue>(ISerializationArgument argument, out TValue DTO)
        {
            DTO = default(TValue);
			
            if (!strategyRepository.TryGet(argument.GetType(), out var strategyObject))
                throw new Exception($"[XMLSerializer] COULD NOT RESOLVE STRATEGY BY ARGUMENT: {argument.GetType().ToString()}");

            var concreteStrategy = (IXMLSerializationStrategy)strategyObject;

            var serializer = new XmlSerializer(typeof(TValue));
            
            return concreteStrategy.Deserialize<TValue>(argument, serializer, out DTO);
        }

        public void Erase(ISerializationArgument argument)
        {
            if (!strategyRepository.TryGet(argument.GetType(), out var strategyObject))
                throw new Exception($"[XMLSerializer] COULD NOT RESOLVE STRATEGY BY ARGUMENT: {argument.GetType().ToString()}");

            var concreteStrategy = (IXMLSerializationStrategy)strategyObject;
			
            concreteStrategy.Erase(argument);
        }
        
        #endregion
    }
}