using System.Collections;
using System.Globalization;
using System.IO;

using HereticalSolutions.Persistence.Arguments;
using HereticalSolutions.Persistence.IO;

using CsvHelper;

namespace HereticalSolutions.Persistence.Serializers
{
    public class UnitySerializeCsvIntoTextFileStrategy : ICsvSerializationStrategy
    {
        public bool Serialize<TValue>(ISerializationArgument argument, TValue value)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityTextFileArgument)argument).Settings;

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
            
            return UnityTextFileIO.Write(fileSystemSettings, csv);
        }

        public bool Deserialize<TValue>(ISerializationArgument argument, out TValue value)
        {
            UnityFileSystemSettings fileSystemSettings = ((UnityTextFileArgument)argument).Settings;

            value = default(TValue);
            
            bool result = UnityTextFileIO.Read(fileSystemSettings, out string csv);

            if (!result)
                return false;
            
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
                        value = csvReader.GetRecord<TValue>();
                }
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