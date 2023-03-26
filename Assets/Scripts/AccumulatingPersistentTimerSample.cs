using System;

using HereticalSolutions.Persistence;
using HereticalSolutions.Persistence.Arguments;
using HereticalSolutions.Persistence.Factories;
using HereticalSolutions.Persistence.IO;

using HereticalSolutions.Time;
using HereticalSolutions.Time.Factories;

using UnityEngine;
using UnityEngine.Serialization;

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
    
    //Timers
    private IPersistentTimer persistentTimer;

    //Visitors
    private ISaveVisitor saveVisitor;

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
        
        //Initialize visitors
        saveVisitor = PersistenceFactory.BuildSimpleCompositeVisitorWithTimerVisitors();

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
        
        
        //Start timers
        persistentTimer.Start();
        
        //Serialize
        Save();
    }

    // Update is called once per frame
    void Update()
    {
        ((ITickable)persistentTimer).Tick(Time.deltaTime);

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
        ((IVisitable)persistentTimer).Accept(saveVisitor, out var dto);
        
        jsonSerializer.Serialize(jsonTextFileArgument, dto);

        xmlSerializer.Serialize(xmlTextFileArgument, ((IVisitable)persistentTimer).DTOType, dto);
        
        var timeProgress = ((IPersistentTimerContext)persistentTimer).SavedProgress;
        
        Debug.Log($"[AccumulatingPersistentTimerSample] ACCUMULATING PERSISTENT TIMER SERIALIZED. PROGRESS: HOURS: {timeProgress.Hours.ToString()} MINUTES: {timeProgress.Minutes.ToString()} SECONDS: {timeProgress.Seconds.ToString()}");
    }
}
