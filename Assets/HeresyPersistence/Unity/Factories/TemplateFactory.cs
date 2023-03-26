using System;

using HereticalSolutions.Persistence.Serializers;
using HereticalSolutions.Persistence.Arguments;

using HereticalSolutions.Repositories;
using HereticalSolutions.Repositories.Factories;

namespace HereticalSolutions.Persistence.Factories
{
    public static partial class PersistenceFactory
    {
        public static JSONSerializer BuildSimpleUnityJSONSerializer()
        {
            IRepository<Type, object> database = RepositoriesFactory.BuildDictionaryRepository<Type, object>();
            
            database.Add(typeof(StringArgument), new SerializeJsonIntoStringStrategy());
            
            database.Add(typeof(StreamArgument), new SerializeJsonIntoStreamStrategy());
            database.Add(typeof(TextFileArgument), new SerializeJsonIntoTextFileStrategy());
            
            database.Add(typeof(UnityStreamArgument), new UnitySerializeJsonIntoStreamStrategy());
            database.Add(typeof(UnityTextFileArgument), new UnitySerializeJsonIntoTextFileStrategy());
            
            database.Add(typeof(UnityPlayerPrefsArgument), new UnitySerializeJsonIntoPlayerPrefsStrategy());
            
            IReadOnlyObjectRepository strategyRepository = RepositoriesFactory.BuildDictionaryObjectRepository(database);
            
            return new JSONSerializer(strategyRepository);
        }

        public static BinarySerializer BuildSimpleUnityBinarySerializer()
        {
            IRepository<Type, object> database = RepositoriesFactory.BuildDictionaryRepository<Type, object>();
            
            database.Add(typeof(StreamArgument), new SerializeBinaryIntoStreamStrategy());
            database.Add(typeof(TextFileArgument), new SerializeBinaryIntoTextFileStrategy());
            
            database.Add(typeof(UnityStreamArgument), new UnitySerializeBinaryIntoStreamStrategy());
            database.Add(typeof(UnityTextFileArgument), new UnitySerializeBinaryIntoTextFileStrategy());
            
            IReadOnlyObjectRepository strategyRepository = RepositoriesFactory.BuildDictionaryObjectRepository(database);
            
            return new BinarySerializer(strategyRepository);
        }
        
        public static XMLSerializer BuildSimpleUnityXMLSerializer()
        {
            IRepository<Type, object> database = RepositoriesFactory.BuildDictionaryRepository<Type, object>();
            
            database.Add(typeof(StringArgument), new SerializeXMLIntoStringStrategy());
            
            database.Add(typeof(StreamArgument), new SerializeXMLIntoStreamStrategy());
            database.Add(typeof(TextFileArgument), new SerializeXMLIntoTextFileStrategy());
            
            database.Add(typeof(UnityStreamArgument), new UnitySerializeXMLIntoStreamStrategy());
            database.Add(typeof(UnityTextFileArgument), new UnitySerializeXMLIntoTextFileStrategy());
            
            database.Add(typeof(UnityPlayerPrefsArgument), new UnitySerializeXMLIntoPlayerPrefsStrategy());
            
            IReadOnlyObjectRepository strategyRepository = RepositoriesFactory.BuildDictionaryObjectRepository(database);
            
            return new XMLSerializer(strategyRepository);
        }
        
        public static YAMLSerializer BuildSimpleUnityYAMLSerializer()
        {
            IRepository<Type, object> database = RepositoriesFactory.BuildDictionaryRepository<Type, object>();
            
            database.Add(typeof(StringArgument), new SerializeYamlIntoStringStrategy());
            
            database.Add(typeof(StreamArgument), new SerializeYamlIntoStreamStrategy());
            database.Add(typeof(TextFileArgument), new SerializeYamlIntoTextFileStrategy());
            
            database.Add(typeof(UnityStreamArgument), new UnitySerializeYamlIntoStreamStrategy());
            database.Add(typeof(UnityTextFileArgument), new UnitySerializeYamlIntoTextFileStrategy());
            
            database.Add(typeof(UnityPlayerPrefsArgument), new UnitySerializeYamlIntoPlayerPrefsStrategy());
            
            IReadOnlyObjectRepository strategyRepository = RepositoriesFactory.BuildDictionaryObjectRepository(database);
            
            return new YAMLSerializer(strategyRepository);
        }
    }
}