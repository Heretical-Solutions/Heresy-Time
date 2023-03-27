using System.Collections;
using System.Globalization;
using System.IO;

using HereticalSolutions.Persistence.Arguments;

using CsvHelper;

using UnityEngine;

namespace HereticalSolutions.Persistence.Serializers
{
    public class UnitySerializeCsvIntoPlayerPrefsStrategy : ICsvSerializationStrategy
    {
        public bool Serialize<TValue>(ISerializationArgument argument, TValue value)
        {
            string prefsKey = ((UnityPlayerPrefsArgument)argument).PrefsKey;
            
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
                
                PlayerPrefs.SetString(prefsKey, stringWriter.ToString());
            }
            
            PlayerPrefs.Save();
            
            return true;
        }

        public bool Deserialize<TValue>(ISerializationArgument argument, out TValue value)
        {
            string prefsKey = ((UnityPlayerPrefsArgument)argument).PrefsKey;

            if (!PlayerPrefs.HasKey(prefsKey))
            {
                value = default(TValue);
                
                return false;
            }

            using (StringReader stringReader = new StringReader(PlayerPrefs.GetString(prefsKey)))
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
            string prefsKey = ((UnityPlayerPrefsArgument)argument).PrefsKey;
            
            if (!PlayerPrefs.HasKey(prefsKey))
                return;
            
            PlayerPrefs.DeleteKey(prefsKey);
        }
    }
}