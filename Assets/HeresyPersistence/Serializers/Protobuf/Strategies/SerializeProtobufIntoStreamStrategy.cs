using System;
using System.IO;

using HereticalSolutions.Persistence.Arguments;
using HereticalSolutions.Persistence.IO;

using ProtoBuf;
using ProtobufInternalSerializer = ProtoBuf.Serializer;

namespace HereticalSolutions.Persistence.Serializers
{
    public class SerializeProtobufIntoStreamStrategy : IProtobufSerializationStrategy
    {
        public bool Serialize(ISerializationArgument argument, Type valueType, object value)
        {
            FileSystemSettings fileSystemSettings = ((StreamArgument)argument).Settings;
            
            if (!StreamIO.OpenWriteStream(fileSystemSettings, out FileStream fileStream))
                return false;
            
            //According to this, is you serialize an 'object' you should add the following args
            //https://stackoverflow.com/questions/10510081/protobuf-net-argumentnullexception
            //ProtobufInternalSerializer.Serialize(fileStream, value);
            ProtobufInternalSerializer.NonGeneric.SerializeWithLengthPrefix(fileStream, value, PrefixStyle.Base128, 1);
            
            StreamIO.CloseStream(fileStream);

            return true;
        }

        public bool Deserialize(ISerializationArgument argument, Type valueType, out object value)
        {
            FileSystemSettings fileSystemSettings = ((StreamArgument)argument).Settings;

            if (!StreamIO.OpenReadStream(fileSystemSettings, out FileStream fileStream))
            {
                value = default(object);
                
                return false;
            }

            //value = ProtobufInternalSerializer.Deserialize<TValue>(fileStream);
            value = ProtobufInternalSerializer.NonGeneric.Deserialize(valueType, fileStream);
            
            StreamIO.CloseStream(fileStream);

            return true;
        }

        public void Erase(ISerializationArgument argument)
        {
            FileSystemSettings fileSystemSettings = ((StreamArgument)argument).Settings;
            
            StreamIO.Erase(fileSystemSettings);
        }
    }
}