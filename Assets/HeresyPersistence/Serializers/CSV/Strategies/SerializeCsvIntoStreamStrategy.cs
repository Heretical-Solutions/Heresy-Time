using System.Collections;
using System.Globalization;
using System.IO;

using HereticalSolutions.Persistence.Arguments;
using HereticalSolutions.Persistence.IO;

using CsvHelper;

namespace HereticalSolutions.Persistence.Serializers
{
    public class SerializeCsvIntoStreamStrategy : ICsvSerializationStrategy
    {
        public bool Serialize<TValue>(ISerializationArgument argument, TValue value)
        {
            FileSystemSettings fileSystemSettings = ((StreamArgument)argument).Settings;
            
            if (!StreamIO.OpenWriteStream(fileSystemSettings, out StreamWriter streamWriter))
                return false;
            
            using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                var valueType = typeof(TValue); 
                
                if (valueType.IsTypeGenericArray()
                    || valueType.IsTypeEnumerable()
                    || valueType.IsTypeGenericEnumerable())
                {
                    csvWriter.WriteRecords((IEnumerable)value);
                }
                else
                    csvWriter.WriteRecord(value);
            }
            
            StreamIO.CloseStream(streamWriter);

            return true;
        }

        public bool Deserialize<TValue>(ISerializationArgument argument, out TValue value)
        {
            FileSystemSettings fileSystemSettings = ((StreamArgument)argument).Settings;

            if (!StreamIO.OpenReadStream(fileSystemSettings, out StreamReader streamReader))
            {
                value = default(TValue);
                
                return false;
            }

            using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                var valueType = typeof(TValue); 
                
                if (valueType.IsTypeGenericArray()
                    || valueType.IsTypeEnumerable()
                    || valueType.IsTypeGenericEnumerable())
                {
                    var underlyingType = (valueType.IsTypeGenericArray() || valueType.IsTypeEnumerable())
                        ? valueType.GetGenericArrayUnderlyingType()
                        : valueType.GetGenericEnumerableUnderlyingType();
                    
                    var records = csvReader.GetRecords(underlyingType);

                    value = (TValue)records;
                }
                else
                {
                    csvReader.Read();   
                    
                    value = csvReader.GetRecord<TValue>();
                }
            }
            
            StreamIO.CloseStream(streamReader);

            return true;
        }

        public void Erase(ISerializationArgument argument)
        {
            FileSystemSettings fileSystemSettings = ((StreamArgument)argument).Settings;
            
            StreamIO.Erase(fileSystemSettings);
        }
    }
}