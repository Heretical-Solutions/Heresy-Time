using System;

using HereticalSolutions.Persistence.Serializers;
using HereticalSolutions.Persistence.Arguments;

using HereticalSolutions.Repositories;
using HereticalSolutions.Repositories.Factories;

namespace HereticalSolutions.Persistence.Factories
{
    public static partial class PersistenceFactory
    {
        public static BinarySerializer BuildSimpleBinarySerializer()
        {
            IRepository<Type, object> database = RepositoriesFactory.BuildDictionaryRepository<Type, object>();
            
            database.Add(typeof(StreamArgument), new SerializeBinaryIntoStreamStrategy());
            
            IReadOnlyObjectRepository strategyRepository = RepositoriesFactory.BuildDictionaryObjectRepository(database);
            
            return new BinarySerializer(strategyRepository);
        }
        
        public static ProtobufSerializer BuildSimpleProtobufSerializer()
        {
            IRepository<Type, object> database = RepositoriesFactory.BuildDictionaryRepository<Type, object>();
            
            database.Add(typeof(StreamArgument), new SerializeProtobufIntoStreamStrategy());
            
            IReadOnlyObjectRepository strategyRepository = RepositoriesFactory.BuildDictionaryObjectRepository(database);
            
            return new ProtobufSerializer(strategyRepository);
        }
        
        public static JSONSerializer BuildSimpleJSONSerializer()
        {
            IRepository<Type, object> database = RepositoriesFactory.BuildDictionaryRepository<Type, object>();
            
            database.Add(typeof(StringArgument), new SerializeJsonIntoStringStrategy());
            
            database.Add(typeof(StreamArgument), new SerializeJsonIntoStreamStrategy());
            database.Add(typeof(TextFileArgument), new SerializeJsonIntoTextFileStrategy());
            
            IReadOnlyObjectRepository strategyRepository = RepositoriesFactory.BuildDictionaryObjectRepository(database);
            
            return new JSONSerializer(strategyRepository);
        }

        public static XMLSerializer BuildSimpleXMLSerializer()
        {
            IRepository<Type, object> database = RepositoriesFactory.BuildDictionaryRepository<Type, object>();
            
            database.Add(typeof(StringArgument), new SerializeXMLIntoStringStrategy());
            
            database.Add(typeof(StreamArgument), new SerializeXMLIntoStreamStrategy());
            database.Add(typeof(TextFileArgument), new SerializeXMLIntoTextFileStrategy());
            
            IReadOnlyObjectRepository strategyRepository = RepositoriesFactory.BuildDictionaryObjectRepository(database);
            
            return new XMLSerializer(strategyRepository);
        }
        
        public static YAMLSerializer BuildSimpleYAMLSerializer()
        {
            IRepository<Type, object> database = RepositoriesFactory.BuildDictionaryRepository<Type, object>();
            
            database.Add(typeof(StringArgument), new SerializeYamlIntoStringStrategy());
            
            database.Add(typeof(StreamArgument), new SerializeYamlIntoStreamStrategy());
            database.Add(typeof(TextFileArgument), new SerializeYamlIntoTextFileStrategy());
            
            IReadOnlyObjectRepository strategyRepository = RepositoriesFactory.BuildDictionaryObjectRepository(database);
            
            return new YAMLSerializer(strategyRepository);
        }
    }
}