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
    
    //Timers
    private IPersistentTimer persistentTimer;
    
    private IVisitable persistentTimerAsVisitable;

    private ITickable persistentTimerAsTickable;

    //Visitors
    private ISaveVisitor saveVisitor;
    
    private ILoadVisitor loadVisitor;

    //Serializers
    private ISerializer binarySerializer;
    
    private ISerializer jsonSerializer;
    
    private ISerializer xmlSerializer;
    
    private ISerializer yamlSerializer;

    //Arguments
    private UnityStreamArgument binaryStreamArgument;
    
    private UnityTextFileArgument jsonTextFileArgument;
    
    private UnityTextFileArgument xmlTextFileArgument;
    
    private UnityTextFileArgument yamlTextFileArgument;

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
        
        jsonSerializer = PersistenceFactory.BuildSimpleUnityJSONSerializer();

        xmlSerializer = PersistenceFactory.BuildSimpleUnityXMLSerializer();
        
        yamlSerializer = PersistenceFactory.BuildSimpleUnityYAMLSerializer();

        //Initialize arguments
        binaryStreamArgument = new UnityStreamArgument();

        binaryStreamArgument.Settings = binFSSettings;
        
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
        
        jsonSerializer.Serialize(jsonTextFileArgument, persistentTimerAsVisitable.DTOType, dto);

        xmlSerializer.Serialize(xmlTextFileArgument, persistentTimerAsVisitable.DTOType, dto);

        yamlSerializer.Serialize(yamlTextFileArgument, persistentTimerAsVisitable.DTOType, dto);
        
        
        //Debug
        var timeProgress = ((IPersistentTimerContext)persistentTimer).SavedProgress;
        
        Debug.Log($"[AccumulatingPersistentTimerSample] ACCUMULATING PERSISTENT TIMER SERIALIZED. PROGRESS: HOURS: {timeProgress.Hours.ToString()} MINUTES: {timeProgress.Minutes.ToString()} SECONDS: {timeProgress.Seconds.ToString()}");
    }
    
    private bool Load()
    {
        object dto;
        
        //Roll deserialization method
        float roll = UnityEngine.Random.Range(0f, 1f);

        bool deserialized;
        
        if (roll < 0.25f) //BINARY
            deserialized = binarySerializer.Deserialize(binaryStreamArgument, persistentTimerAsVisitable.DTOType,  out dto);
        else if (roll < 0.5f) //JSON
            deserialized = jsonSerializer.Deserialize(jsonTextFileArgument, persistentTimerAsVisitable.DTOType,  out dto);
        else if (roll < 0.75f) //XML
            deserialized = xmlSerializer.Deserialize(xmlTextFileArgument, persistentTimerAsVisitable.DTOType,  out dto);
        else //YAML
            deserialized = yamlSerializer.Deserialize(yamlTextFileArgument, persistentTimerAsVisitable.DTOType,  out dto);
        
        if (!deserialized)
            return false;
        
        //Visit
        bool result = persistentTimerAsVisitable.Accept(loadVisitor, dto);

        //Debug
        if (result)
        {
            var timeProgress = ((IPersistentTimerContext)persistentTimer).SavedProgress;

            string methodRolled = string.Empty;

            if (roll < 0.25f) //BINARY
                methodRolled = "binary";
            else if (roll < 0.5f) //JSON
                methodRolled = "JSON";
            else if (roll < 0.75f) //XML
                methodRolled = "XML";
            else //YAML
                methodRolled = "YAML";
            
            Debug.Log($"[AccumulatingPersistentTimerSample] ACCUMULATING PERSISTENT TIMER DESERIALIZED. METHOD: \"{methodRolled}\" PROGRESS: HOURS: {timeProgress.Hours.ToString()} MINUTES: {timeProgress.Minutes.ToString()} SECONDS: {timeProgress.Seconds.ToString()}");
        }

        return result;
    }
}
