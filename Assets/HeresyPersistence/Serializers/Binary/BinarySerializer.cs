using System;
using System.Runtime.Serialization.Formatters.Binary;

using HereticalSolutions.Repositories;

namespace HereticalSolutions.Persistence.Serializers
{
	public class BinarySerializer : ISerializer
	{
		private readonly IReadOnlyObjectRepository strategyRepository;
		
		private readonly BinaryFormatter formatter = new BinaryFormatter();

		public BinarySerializer(IReadOnlyObjectRepository strategyRepository)
		{
			this.strategyRepository = strategyRepository;
		}

		#region ISerializer
		
		public bool Serialize<TValue>(ISerializationArgument argument, TValue DTO)
		{
			if (!strategyRepository.TryGet(argument.GetType(), out var strategyObject))
				throw new Exception($"[BinarySerializer] COULD NOT RESOLVE STRATEGY BY ARGUMENT: {argument.GetType().ToString()}");

			var concreteStrategy = (IBinarySerializationStrategy)strategyObject;

			return concreteStrategy.Serialize<TValue>(argument, formatter, DTO);
		}

		public bool Deserialize<TValue>(ISerializationArgument argument, out TValue DTO)
		{
			DTO = default(TValue);
			
			if (!strategyRepository.TryGet(argument.GetType(), out var strategyObject))
				throw new Exception($"[BinarySerializer] COULD NOT RESOLVE STRATEGY BY ARGUMENT: {argument.GetType().ToString()}");

			var concreteStrategy = (IBinarySerializationStrategy)strategyObject;

			return concreteStrategy.Deserialize<TValue>(argument, formatter, out DTO);
		}
		
		public void Erase(ISerializationArgument argument)
		{
			if (!strategyRepository.TryGet(argument.GetType(), out var strategyObject))
				throw new Exception($"[BinarySerializer] COULD NOT RESOLVE STRATEGY BY ARGUMENT: {argument.GetType().ToString()}");

			var concreteStrategy = (IJsonSerializationStrategy)strategyObject;
			
			concreteStrategy.Erase(argument);
		}
		
		#endregion
	}
}