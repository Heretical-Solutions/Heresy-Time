using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using HereticalSolutions.Persistence.Arguments;
using HereticalSolutions.Persistence.IO;

namespace HereticalSolutions.Persistence.Serializers
{
    public class UnitySerializeBinaryIntoTextFileStrategy : IBinarySerializationStrategy
    {
        private readonly BinaryFormatter formatter = new BinaryFormatter();
        
        public bool Serialize<TValue>(ISerializationArgument argument, TValue value)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityTextFileArgument)argument).Settings;

            byte[] bytes;
            
            using (MemoryStream memoryStream = new MemoryStream())
            {
                formatter.Serialize(memoryStream, value);
                
                bytes = memoryStream.ToArray();
            }
            
            return UnityTextFileIO.Write(fileSystemSettings, bytes);
        }

        public bool Deserialize<TValue>(ISerializationArgument argument, out TValue value)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityTextFileArgument)argument).Settings;

            value = default(TValue);
            
            if (!UnityTextFileIO.Read(fileSystemSettings, out byte[] bytes))
                return false;

            using (MemoryStream memStream = new MemoryStream())
            {
                memStream.Write(bytes, 0, bytes.Length);

                memStream.Seek(0, SeekOrigin.Begin);

                value = (TValue)formatter.Deserialize(memStream);
            }

            return true;
        }
        
        public void Erase(ISerializationArgument argument)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityTextFileArgument)argument).Settings;
            
            UnityTextFileIO.Erase(fileSystemSettings);
        }
    }
}