using System;

using HereticalSolutions.Persistence.Serializers;
using HereticalSolutions.Persistence.Arguments;

using HereticalSolutions.Repositories;
using HereticalSolutions.Repositories.Factories;

namespace HereticalSolutions.Persistence.Factories
{
    public static partial class PersistenceFactory
    {
        public static JSONSerializer BuildSimpleJSONSerializer()
        {
            IRepository<Type, object> database = RepositoriesFactory.BuildDictionaryRepository<Type, object>();
            
            database.Add(typeof(StringArgument), new SerializeJsonIntoStringStrategy());
            
            database.Add(typeof(StreamArgument), new SerializeJsonIntoStreamStrategy());
            database.Add(typeof(TextFileArgument), new SerializeJsonIntoTextFileStrategy());
            
            IReadOnlyObjectRepository strategyRepository = RepositoriesFactory.BuildDictionaryObjectRepository(database);
            
            return new JSONSerializer(strategyRepository);
        }

        public static BinarySerializer BuildSimpleBinarySerializer()
        {
            IRepository<Type, object> database = RepositoriesFactory.BuildDictionaryRepository<Type, object>();
            
            database.Add(typeof(StreamArgument), new SerializeBinaryIntoStreamStrategy());
            database.Add(typeof(TextFileArgument), new SerializeBinaryIntoTextFileStrategy());
            
            IReadOnlyObjectRepository strategyRepository = RepositoriesFactory.BuildDictionaryObjectRepository(database);
            
            return new BinarySerializer(strategyRepository);
        }
    }
}