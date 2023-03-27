using System;

using HereticalSolutions.Persistence;
using HereticalSolutions.Persistence.Arguments;
using HereticalSolutions.Persistence.Factories;
using HereticalSolutions.Persistence.IO;

using HereticalSolutions.Time;
using HereticalSolutions.Time.Factories;

using UnityEngine;

public class AccumulatingPersistentTimerSample : MonoBehaviour
{
    [SerializeField]
    private UnityFileSystemSettings binFSSettings;
    
    [SerializeField]
    private UnityFileSystemSettings protoFSSettings;
    
    [SerializeField]
    private UnityFileSystemSettings jsonFSSettings;

    [SerializeField]
    private UnityFileSystemSettings xmlFSSettings;
    
    [SerializeField]
    private UnityFileSystemSettings yamlFSSettings;
    
    [SerializeField]
    private UnityFileSystemSettings csvFSSettings;
    
    [SerializeField]
    private float autosaveCooldown = 5f;

    [SerializeField]
    private float debugCountdown;
    
    [SerializeField]
    private bool append = false;
    
    [SerializeField]
    private float forceDeserializationRoll = -1f;
    
    //Timers
    private IPersistentTimer persistentTimer;
    
    private IVisitable persistentTimerAsVisitable;

    private ITickable persistentTimerAsTickable;

    //Visitors
    private ISaveVisitor saveVisitor;
    
    private ILoadVisitor loadVisitor;

    //Serializers
    private ISerializer binarySerializer;
    
    private ISerializer protobufSerializer;
    
    private ISerializer jsonSerializer;
    
    private ISerializer xmlSerializer;
    
    private ISerializer yamlSerializer;

    private ISerializer csvSerializer;
    
    //Arguments
    private UnityStreamArgument binaryStreamArgument;
    
    private UnityStreamArgument protobufStreamArgument;
    
    private UnityTextFileArgument jsonTextFileArgument;
    
    private UnityTextFileArgument xmlTextFileArgument;
    
    private UnityTextFileArgument yamlTextFileArgument;

    private UnityTextFileArgument csvTextFileArgument;
    
    //Countdowns
    private float countdown;
    
    // Start is called before the first frame update
    void Start()
    {
        //Initialize timers
        persistentTimer = TimersFactory.BuildPersistentTimer(
            "AccumulatingPersistentTimer",
            default(TimeSpan));

        persistentTimer.Accumulate = true;
        
        persistentTimerAsVisitable = (IVisitable)persistentTimer;

        persistentTimerAsTickable = (ITickable)persistentTimer;
        
        //Initialize visitors
        var visitor = PersistenceFactory.BuildSimpleCompositeVisitorWithTimerVisitors();

        saveVisitor = visitor;

        loadVisitor = visitor;

        //Initialize serializers
        binarySerializer = PersistenceFactory.BuildSimpleUnityBinarySerializer();
        
        protobufSerializer = PersistenceFactory.BuildSimpleUnityProtobufSerializer();
        
        jsonSerializer = PersistenceFactory.BuildSimpleUnityJSONSerializer();

        xmlSerializer = PersistenceFactory.BuildSimpleUnityXMLSerializer();
        
        yamlSerializer = PersistenceFactory.BuildSimpleUnityYAMLSerializer();

        csvSerializer = PersistenceFactory.BuildSimpleUnityCSVSerializer();
        
        //Initialize arguments
        binaryStreamArgument = new UnityStreamArgument();

        binaryStreamArgument.Settings = binFSSettings;
        
        protobufStreamArgument = new UnityStreamArgument();

        protobufStreamArgument.Settings = protoFSSettings;
        
        jsonTextFileArgument = new UnityTextFileArgument();

        jsonTextFileArgument.Settings = jsonFSSettings;

        xmlTextFileArgument = new UnityTextFileArgument();

        xmlTextFileArgument.Settings = xmlFSSettings;
        
        yamlTextFileArgument = new UnityTextFileArgument();

        yamlTextFileArgument.Settings = yamlFSSettings;
        
        csvTextFileArgument = new UnityTextFileArgument();

        csvTextFileArgument.Settings = csvFSSettings;

        //Initialize countdown
        countdown = autosaveCooldown;
        
        
        if (append)
        {
            //Deserialize
            if (!Load())
            {
                //Start timers
                persistentTimer.Start();

                //Serialize
                Save();
            }
        }
        else
        {
            //Start timers
            persistentTimer.Start();

            //Serialize
            Save();
        }
    }

    // Update is called once per frame
    void Update()
    {
        persistentTimerAsTickable.Tick(Time.deltaTime);

        countdown -= Time.deltaTime;

        if (countdown < 0f)
        {
            countdown = autosaveCooldown;
            
            Save();
        }

        debugCountdown = countdown;
    }

    private void Save()
    {
        //Visit
        persistentTimerAsVisitable.Accept(saveVisitor, out var dto);
        
        //Serialize
        binarySerializer.Serialize(binaryStreamArgument, persistentTimerAsVisitable.DTOType, dto);
        
        //Skip for DTOs with no attributes defined
        protobufSerializer.Serialize(protobufStreamArgument, persistentTimerAsVisitable.DTOType, dto);
        
        jsonSerializer.Serialize(jsonTextFileArgument, persistentTimerAsVisitable.DTOType, dto);

        xmlSerializer.Serialize(xmlTextFileArgument, persistentTimerAsVisitable.DTOType, dto);

        yamlSerializer.Serialize(yamlTextFileArgument, persistentTimerAsVisitable.DTOType, dto);
        
        //Skip for DTOs with no attributes defined
        csvSerializer.Serialize(csvTextFileArgument, persistentTimerAsVisitable.DTOType, dto);
        
        
        //Debug
        var timeProgress = ((IPersistentTimerContext)persistentTimer).SavedProgress;
        
        Debug.Log($"[AccumulatingPersistentTimerSample] ACCUMULATING PERSISTENT TIMER SERIALIZED. PROGRESS: HOURS: {timeProgress.Hours.ToString()} MINUTES: {timeProgress.Minutes.ToString()} SECONDS: {timeProgress.Seconds.ToString()}");
    }
    
    private bool Load()
    {
        object dto;
        
        //Roll deserialization method
        float roll = UnityEngine.Random.Range(0f, 1f);

        if (forceDeserializationRoll > 0f)
            roll = forceDeserializationRoll;
        
        bool deserialized;
        
        if (roll < 0.16f) //BINARY
            deserialized = binarySerializer.Deserialize(binaryStreamArgument, persistentTimerAsVisitable.DTOType,  out dto);
        else if (roll < 0.33f) //PROTOBUF
        {
            //Skip for DTOs with no attributes defined
            deserialized = protobufSerializer.Deserialize(protobufStreamArgument, persistentTimerAsVisitable.DTOType,  out dto);
            
            //return false;
        }
        else if (roll < 0.5f) //JSON
            deserialized = jsonSerializer.Deserialize(jsonTextFileArgument, persistentTimerAsVisitable.DTOType,  out dto);
        else if (roll < 0.66f) //XML
            deserialized = xmlSerializer.Deserialize(xmlTextFileArgument, persistentTimerAsVisitable.DTOType,  out dto);
        else if (roll < 0.83f) //YAML
            deserialized = yamlSerializer.Deserialize(yamlTextFileArgument, persistentTimerAsVisitable.DTOType,  out dto);
        else //CSV
        {
            //Skip for DTOs with no attributes defined
            deserialized = csvSerializer.Deserialize(csvTextFileArgument, persistentTimerAsVisitable.DTOType,  out dto);
            
            //return false;
        }
        
        if (!deserialized)
            return false;
        
        //Visit
        bool result = persistentTimerAsVisitable.Accept(loadVisitor, dto);

        //Debug
        if (result)
        {
            var timeProgress = ((IPersistentTimerContext)persistentTimer).SavedProgress;

            string methodRolled = string.Empty;

            if (roll < 0.16f) //BINARY
                methodRolled = "binary";
            else if (roll < 0.33f) //PROTOBUF
                methodRolled = "protobuf";
            else if (roll < 0.5f) //JSON
                methodRolled = "JSON";
            else if (roll < 0.66f) //XML
                methodRolled = "XML";
            else if (roll < 0.83f) //YAML
                methodRolled = "YAML";
            else //CSV
                methodRolled = "CSV";
            
            Debug.Log($"[AccumulatingPersistentTimerSample] ACCUMULATING PERSISTENT TIMER DESERIALIZED. METHOD: \"{methodRolled}\" PROGRESS: HOURS: {timeProgress.Hours.ToString()} MINUTES: {timeProgress.Minutes.ToString()} SECONDS: {timeProgress.Seconds.ToString()}");
        }

        return result;
    }
}