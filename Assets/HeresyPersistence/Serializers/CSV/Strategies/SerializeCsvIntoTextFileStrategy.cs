using System.Collections;
using System.Globalization;
using System.IO;

using HereticalSolutions.Persistence.Arguments;
using HereticalSolutions.Persistence.IO;

using CsvHelper;

namespace HereticalSolutions.Persistence.Serializers
{
    public class SerializeCsvIntoTextFileStrategy : ICsvSerializationStrategy
    {
        public bool Serialize<TValue>(ISerializationArgument argument, TValue value)
        {
            FileSystemSettings fileSystemSettings = ((TextFileArgument)argument).Settings;

            string csv;
            
            using (StringWriter stringWriter = new StringWriter())
            {
                using (var csvWriter = new CsvWriter(stringWriter, CultureInfo.InvariantCulture))
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
                
                csv = stringWriter.ToString();
            }
            
            return TextFileIO.Write(fileSystemSettings, csv);
        }

        public bool Deserialize<TValue>(ISerializationArgument argument, out TValue value)
        {
            FileSystemSettings fileSystemSettings = ((TextFileArgument)argument).Settings;

            bool result = TextFileIO.Read(fileSystemSettings, out string csv);

            if (!result)
            {
                value = default(TValue);
                
                return false;
            }

            using (StringReader stringReader = new StringReader(csv))
            {
                using (var csvReader = new CsvReader(stringReader, CultureInfo.InvariantCulture))
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