using System;

using HereticalSolutions.Repositories;

using Newtonsoft.Json;

namespace HereticalSolutions.Persistence.Serializers
{
	public class JSONSerializer : ISerializer
	{
		/// <summary>
		/// JSON.Net serialization settings for writing
		/// </summary>
		private readonly JsonSerializerSettings writeSerializerSettings;

		/// <summary>
		/// JSON.Net serialization settings for reading
		/// </summary>
		private readonly JsonSerializerSettings readSerializerSettings;

		private readonly IReadOnlyObjectRepository strategyRepository;

		public JSONSerializer(IReadOnlyObjectRepository strategyRepository)
		{
			writeSerializerSettings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Auto,
				TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			};

			readSerializerSettings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Auto,
				TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
				MaxDepth = 10
			};

			this.strategyRepository = strategyRepository;
		}

		#region ISerializer
		
		public bool Serialize<TValue>(ISerializationArgument argument, TValue DTO)
		{
			string json = JsonConvert.SerializeObject(
				DTO,
				Formatting.Indented,
				writeSerializerSettings);
			
			if (!strategyRepository.TryGet(argument.GetType(), out var strategyObject))
				throw new Exception($"[JSONSerializer] COULD NOT RESOLVE STRATEGY BY ARGUMENT: {argument.GetType().ToString()}");

			var concreteStrategy = (IJsonSerializationStrategy)strategyObject;

			return concreteStrategy.Serialize(argument, json);
		}

		public bool Deserialize<TValue>(ISerializationArgument argument, out TValue DTO)
		{
			DTO = default(TValue);
			
			if (!strategyRepository.TryGet(argument.GetType(), out var strategyObject))
				throw new Exception($"[JSONSerializer] COULD NOT RESOLVE STRATEGY BY ARGUMENT: {argument.GetType().ToString()}");

			var concreteStrategy = (IJsonSerializationStrategy)strategyObject;

			if (!concreteStrategy.Deserialize(argument, out var json))
				return false;

			JsonConvert.PopulateObject(json, DTO, readSerializerSettings);

			return true;
		}
		
		public void Erase(ISerializationArgument argument)
		{
			if (!strategyRepository.TryGet(argument.GetType(), out var strategyObject))
				throw new Exception($"[JSONSerializer] COULD NOT RESOLVE STRATEGY BY ARGUMENT: {argument.GetType().ToString()}");

			var concreteStrategy = (IJsonSerializationStrategy)strategyObject;
			
			concreteStrategy.Erase(argument);
		}
		
		#endregion
	}
}