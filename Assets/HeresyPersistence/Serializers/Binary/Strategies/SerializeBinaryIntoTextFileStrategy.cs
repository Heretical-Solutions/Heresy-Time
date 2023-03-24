using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using HereticalSolutions.Persistence.Arguments;
using HereticalSolutions.Persistence.IO;

namespace HereticalSolutions.Persistence.Serializers
{
    public class SerializeBinaryIntoTextFileStrategy : IBinarySerializationStrategy
    {
        private readonly BinaryFormatter formatter = new BinaryFormatter();
        
        public bool Serialize<TValue>(ISerializationArgument argument, TValue value)
        {
            FileSystemSettings fileSystemSettings = ((TextFileArgument)argument).Settings;

            byte[] bytes;
            
            using (MemoryStream memoryStream = new MemoryStream())
            {
                formatter.Serialize(memoryStream, value);
                
                bytes = memoryStream.ToArray();
            }
            
            return TextFileIO.Write(fileSystemSettings, bytes);
        }

        public bool Deserialize<TValue>(ISerializationArgument argument, out TValue value)
        {
            FileSystemSettings fileSystemSettings = ((TextFileArgument)argument).Settings;

            value = default(TValue);
            
            if (!TextFileIO.Read(fileSystemSettings, out byte[] bytes))
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
            FileSystemSettings fileSystemSettings = ((TextFileArgument)argument).Settings;
            
            TextFileIO.Erase(fileSystemSettings);
        }
    }
}