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

            var concreteStrategy = (IXmlSerializationStrategy)strategyObject;

            return concreteStrategy.Serialize<TValue>(argument, serializer, DTO);
        }

        public bool Serialize(ISerializationArgument argument, Type DTOType, object DTO)
        {
            var serializer = new XmlSerializer(DTOType);
            
            if (!strategyRepository.TryGet(argument.GetType(), out var strategyObject))
                throw new Exception($"[XMLSerializer] COULD NOT RESOLVE STRATEGY BY ARGUMENT: {argument.GetType().ToString()}");

            var concreteStrategy = (IXmlSerializationStrategy)strategyObject;

            return concreteStrategy.Serialize(argument, serializer, DTO);
        }

        public bool Deserialize<TValue>(ISerializationArgument argument, out TValue DTO)
        {
            if (!strategyRepository.TryGet(argument.GetType(), out var strategyObject))
                throw new Exception($"[XMLSerializer] COULD NOT RESOLVE STRATEGY BY ARGUMENT: {argument.GetType().ToString()}");

            var concreteStrategy = (IXmlSerializationStrategy)strategyObject;

            var serializer = new XmlSerializer(typeof(TValue));
            
            return concreteStrategy.Deserialize<TValue>(argument, serializer, out DTO);
        }

        public bool Deserialize(ISerializationArgument argument, Type DTOType, out object DTO)
        {
            if (!strategyRepository.TryGet(argument.GetType(), out var strategyObject))
                throw new Exception($"[XMLSerializer] COULD NOT RESOLVE STRATEGY BY ARGUMENT: {argument.GetType().ToString()}");

            var concreteStrategy = (IXmlSerializationStrategy)strategyObject;

            var serializer = new XmlSerializer(DTOType);
            
            return concreteStrategy.Deserialize(argument, serializer, out DTO);
        }

        public void Erase(ISerializationArgument argument)
        {
            if (!strategyRepository.TryGet(argument.GetType(), out var strategyObject))
                throw new Exception($"[XMLSerializer] COULD NOT RESOLVE STRATEGY BY ARGUMENT: {argument.GetType().ToString()}");

            var concreteStrategy = (IXmlSerializationStrategy)strategyObject;
			
            concreteStrategy.Erase(argument);
        }
        
        #endregion
    }
}