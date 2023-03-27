using System;

using HereticalSolutions.Repositories;

namespace HereticalSolutions.Persistence.Serializers
{
	public class CSVSerializer : ISerializer
	{
		private readonly IReadOnlyObjectRepository strategyRepository;

		public CSVSerializer(IReadOnlyObjectRepository strategyRepository)
		{
			this.strategyRepository = strategyRepository;
		}

		#region ISerializer
		
		public bool Serialize<TValue>(ISerializationArgument argument, TValue DTO)
		{
			if (!strategyRepository.TryGet(argument.GetType(), out var strategyObject))
				throw new Exception($"[CSVSerializer] COULD NOT RESOLVE STRATEGY BY ARGUMENT: {argument.GetType().ToString()}");

			var concreteStrategy = (ICsvSerializationStrategy)strategyObject;

			return concreteStrategy.Serialize(argument, DTO);
		}

		public bool Serialize(ISerializationArgument argument, Type DTOType, object DTO)
		{
			if (!strategyRepository.TryGet(argument.GetType(), out var strategyObject))
				throw new Exception($"[CSVSerializer] COULD NOT RESOLVE STRATEGY BY ARGUMENT: {argument.GetType().ToString()}");

			var concreteStrategy = (ICsvSerializationStrategy)strategyObject;

			return concreteStrategy.Serialize(argument, DTO);
		}

		public bool Deserialize<TValue>(ISerializationArgument argument, out TValue DTO)
		{
			if (!strategyRepository.TryGet(argument.GetType(), out var strategyObject))
				throw new Exception($"[CSVSerializer] COULD NOT RESOLVE STRATEGY BY ARGUMENT: {argument.GetType().ToString()}");

			var concreteStrategy = (ICsvSerializationStrategy)strategyObject;

			return concreteStrategy.Deserialize(argument, out DTO);
		}

		public bool Deserialize(ISerializationArgument argument, Type DTOType, out object DTO)
		{
			if (!strategyRepository.TryGet(argument.GetType(), out var strategyObject))
				throw new Exception($"[CSVSerializer] COULD NOT RESOLVE STRATEGY BY ARGUMENT: {argument.GetType().ToString()}");

			var concreteStrategy = (ICsvSerializationStrategy)strategyObject;

			return concreteStrategy.Deserialize(argument, out DTO);
		}

		public void Erase(ISerializationArgument argument)
		{
			if (!strategyRepository.TryGet(argument.GetType(), out var strategyObject))
				throw new Exception($"[CSVSerializer] COULD NOT RESOLVE STRATEGY BY ARGUMENT: {argument.GetType().ToString()}");

			var concreteStrategy = (ICsvSerializationStrategy)strategyObject;
			
			concreteStrategy.Erase(argument);
		}
		
		#endregion
	}
}