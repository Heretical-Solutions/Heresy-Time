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
                if (typeof(TValue).IsArray)
                {
                    //TODO: array logic
                }
                else if(typeof(IEnumerable).IsAssignableFrom(typeof(TValue)))
                {
                    csvWriter.WriteRecords((IEnumerable)value);
                }
                else if ()
                {
                    
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

            value = default(TValue);
            
            if (!StreamIO.OpenReadStream(fileSystemSettings, out StreamReader streamReader))
                return false;
            
            using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
            {
                if(typeof(IEnumerable).IsAssignableFrom(typeof(TValue)))
                {
                    var underlyingType = typeof(TValue).GetGenericArguments()[0]; 
                    
                    var records = csvReader.GetRecords(underlyingType);

                    value = (TValue)records;
                }
                else
                    value = csvReader.GetRecord<TValue>();
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