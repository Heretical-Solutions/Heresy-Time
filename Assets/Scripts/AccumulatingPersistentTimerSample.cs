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
    private UnityFileSystemSettings jsonFSSettings;

    [SerializeField]
    private UnityFileSystemSettings xmlFSSettings;
    
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
    private ISerializer jsonSerializer;
    
    private ISerializer xmlSerializer;

    //Arguments
    private UnityTextFileArgument jsonTextFileArgument;
    
    private UnityTextFileArgument xmlTextFileArgument;

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
        jsonSerializer = PersistenceFactory.BuildSimpleUnityJSONSerializer();

        xmlSerializer = PersistenceFactory.BuildSimpleUnityXMLSerializer();

        //Initialize arguments
        jsonTextFileArgument = new UnityTextFileArgument();

        jsonTextFileArgument.Settings = jsonFSSettings;

        xmlTextFileArgument = new UnityTextFileArgument();

        xmlTextFileArgument.Settings = xmlFSSettings;

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
        persistentTimerAsVisitable.Accept(saveVisitor, out var dto);
        
        jsonSerializer.Serialize(jsonTextFileArgument, persistentTimerAsVisitable.DTOType, dto);

        xmlSerializer.Serialize(xmlTextFileArgument, persistentTimerAsVisitable.DTOType, dto);
        
        
        var timeProgress = ((IPersistentTimerContext)persistentTimer).SavedProgress;
        
        Debug.Log($"[AccumulatingPersistentTimerSample] ACCUMULATING PERSISTENT TIMER SERIALIZED. PROGRESS: HOURS: {timeProgress.Hours.ToString()} MINUTES: {timeProgress.Minutes.ToString()} SECONDS: {timeProgress.Seconds.ToString()}");
    }
    
    private bool Load()
    {
        object dto;
        
        bool json = UnityEngine.Random.Range(0f, 1f) > 0.5f;

        if (json)
            jsonSerializer.Deserialize(jsonTextFileArgument, persistentTimerAsVisitable.DTOType,  out dto);
        else
            xmlSerializer.Deserialize(xmlTextFileArgument, persistentTimerAsVisitable.DTOType,  out dto);
        
        bool result = persistentTimerAsVisitable.Accept(loadVisitor, dto);

        if (result)
        {
            var timeProgress = ((IPersistentTimerContext)persistentTimer).SavedProgress;

            Debug.Log($"[AccumulatingPersistentTimerSample] ACCUMULATING PERSISTENT TIMER DESERIALIZED. METHOD: \"{(json ? "JSON" : "XML")}\" PROGRESS: HOURS: {timeProgress.Hours.ToString()} MINUTES: {timeProgress.Minutes.ToString()} SECONDS: {timeProgress.Seconds.ToString()}");
        }

        return result;
    }
}
