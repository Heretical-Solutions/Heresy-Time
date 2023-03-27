using System;
using System.IO;

using HereticalSolutions.Persistence.Arguments;
using HereticalSolutions.Persistence.IO;

using ProtoBuf;
using ProtobufInternalSerializer = ProtoBuf.Serializer;

namespace HereticalSolutions.Persistence.Serializers
{
    public class UnitySerializeProtobufIntoStreamStrategy : IProtobufSerializationStrategy
    {
        public bool Serialize(ISerializationArgument argument, Type valueType, object value)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityStreamArgument)argument).Settings;
            
            if (!UnityStreamIO.OpenWriteStream(fileSystemSettings, out FileStream fileStream))
                return false;
            
            //According to this, is you serialize an 'object' you should add the following args
            //https://stackoverflow.com/questions/10510081/protobuf-net-argumentnullexception
            //ProtobufInternalSerializer.Serialize(fileStream, value);
            ProtobufInternalSerializer.NonGeneric.SerializeWithLengthPrefix(fileStream, value, PrefixStyle.Base128, 1);
            
            UnityStreamIO.CloseStream(fileStream);

            return true;
        }

        public bool Deserialize(ISerializationArgument argument, Type valueType, out object value)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityStreamArgument)argument).Settings;

            if (!UnityStreamIO.OpenReadStream(fileSystemSettings, out FileStream fileStream))
            {
                value = default(object);
                
                return false;
            }

            //value = ProtobufInternalSerializer.Deserialize<TValue>(fileStream);
            value = ProtobufInternalSerializer.NonGeneric.Deserialize(valueType, fileStream);
            
            UnityStreamIO.CloseStream(fileStream);

            return true;
        }

        public void Erase(ISerializationArgument argument)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityStreamArgument)argument).Settings;
            
            UnityStreamIO.Erase(fileSystemSettings);
        }
    }
}