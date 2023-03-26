using HereticalSolutions.Persistence;
using HereticalSolutions.Persistence.Arguments;
using HereticalSolutions.Persistence.Factories;
using HereticalSolutions.Persistence.IO;

using HereticalSolutions.Time;
using HereticalSolutions.Time.Factories;

using UnityEngine;

public class AccumulatingRuntimeTimerSample : MonoBehaviour
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
    private float autosaveCooldown = 5f;

    [SerializeField]
    private float debugCountdown;

    [SerializeField]
    private bool append = false;
    
    [SerializeField]
    private float forceDeserializationRoll = -1f;
    
    //Timers
    private IRuntimeTimer runtimeTimer;

    private IVisitable runtimeTimerAsVisitable;

    private ITickable runtimeTimerAsTickable;
    
    //Visitors
    private ISaveVisitor saveVisitor;
    
    private ILoadVisitor loadVisitor;

    //Serializers
    private ISerializer binarySerializer;
    
    private ISerializer protobufSerializer;
    
    private ISerializer jsonSerializer;
    
    private ISerializer xmlSerializer;
    
    private ISerializer yamlSerializer;

    //Arguments
    private UnityStreamArgument binaryStreamArgument;
    
    private UnityStreamArgument protobufStreamArgument;
    
    private UnityTextFileArgument jsonTextFileArgument;
    
    private UnityTextFileArgument xmlTextFileArgument;
    
    private UnityTextFileArgument yamlTextFileArgument;
    
    //Countdowns
    private float countdown;
    
    // Start is called before the first frame update
    void Start()
    {
        //Initialize timers
        runtimeTimer = TimersFactory.BuildRuntimeTimer(
            "AccumulatingPersistentTimer",
            0f);

        runtimeTimer.Accumulate = true;

        runtimeTimerAsVisitable = (IVisitable)runtimeTimer;

        runtimeTimerAsTickable = (ITickable)runtimeTimer;
        
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

        //Initialize countdown
        countdown = autosaveCooldown;


        if (append)
        {
            //Deserialize
            if (!Load())
            {
                //Start timers
                runtimeTimer.Start();

                //Serialize
                Save();
            }
        }
        else
        {
            //Start timers
            runtimeTimer.Start();

            //Serialize
            Save();
        }
    }

    // Update is called once per frame
    void Update()
    {
        runtimeTimerAsTickable.Tick(Time.deltaTime);

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
        runtimeTimerAsVisitable.Accept(saveVisitor, out var dto);
        
        //Serialize
        binarySerializer.Serialize(binaryStreamArgument, runtimeTimerAsVisitable.DTOType, dto);
        
        //Skip for timers - no contract defined
        //protobufSerializer.Serialize(protobufStreamArgument, runtimeTimerAsVisitable.DTOType, dto);
        
        jsonSerializer.Serialize(jsonTextFileArgument, runtimeTimerAsVisitable.DTOType, dto);

        xmlSerializer.Serialize(xmlTextFileArgument, runtimeTimerAsVisitable.DTOType, dto);

        yamlSerializer.Serialize(yamlTextFileArgument, runtimeTimerAsVisitable.DTOType, dto);
        
        
        //Debug
        var timeProgress = runtimeTimer.TimeElapsed;
        
        Debug.Log($"[AccumulatingRuntimeTimerSample] ACCUMULATING RUNTIME TIMER SERIALIZED. TIME ELAPSED: {timeProgress.ToString()}");
    }

    private bool Load()
    {
        object dto;
        
        //Roll deserialization method
        float roll = UnityEngine.Random.Range(0f, 1f);

        if (forceDeserializationRoll > 0f)
            roll = forceDeserializationRoll;

        bool deserialized;
        
        if (roll < 0.2f) //BINARY
            deserialized = binarySerializer.Deserialize(binaryStreamArgument, runtimeTimerAsVisitable.DTOType,  out dto);
        else if (roll < 0.4f) //PROTOBUF
        {
            //Skip for timers - no contract defined
            //deserialized = protobufSerializer.Deserialize(protobufStreamArgument, runtimeTimerAsVisitable.DTOType, out dto);

            return false;
        }
        else if (roll < 0.6f) //JSON
            deserialized = jsonSerializer.Deserialize(jsonTextFileArgument, runtimeTimerAsVisitable.DTOType,  out dto);
        else if (roll < 0.8f) //XML
            deserialized = xmlSerializer.Deserialize(xmlTextFileArgument, runtimeTimerAsVisitable.DTOType,  out dto);
        else //YAML
            deserialized = yamlSerializer.Deserialize(yamlTextFileArgument, runtimeTimerAsVisitable.DTOType,  out dto);

        if (!deserialized)
            return false;
        
        //Visit
        bool result = runtimeTimerAsVisitable.Accept(loadVisitor, dto);

        //Debug
        if (result)
        {
            var timeProgress = runtimeTimer.TimeElapsed;

            string methodRolled = string.Empty;

            if (roll < 0.2f) //BINARY
                methodRolled = "binary";
            else if (roll < 0.4f) //PROTOBUF
                methodRolled = "protobuf";
            else if (roll < 0.6f) //JSON
                methodRolled = "JSON";
            else if (roll < 0.8f) //XML
                methodRolled = "XML";
            else //YAML
                methodRolled = "YAML";
            
            Debug.Log(
                $"[AccumulatingRuntimeTimerSample] ACCUMULATING RUNTIME TIMER DESERIALIZED. METHOD: \"{methodRolled}\" TIME ELAPSED: {timeProgress.ToString()}");
        }

        return result;
    }
}
