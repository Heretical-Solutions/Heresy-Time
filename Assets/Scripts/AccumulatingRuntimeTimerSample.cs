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
    private IRuntimeTimer runtimeTimer;

    private IVisitable runtimeTimerAsVisitable;

    private ITickable runtimeTimerAsTickable;
    
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
        runtimeTimerAsVisitable.Accept(saveVisitor, out var dto);
        
        jsonSerializer.Serialize(jsonTextFileArgument, runtimeTimerAsVisitable.DTOType, dto);

        xmlSerializer.Serialize(xmlTextFileArgument, runtimeTimerAsVisitable.DTOType, dto);
        
        
        var timeProgress = runtimeTimer.TimeElapsed;
        
        Debug.Log($"[AccumulatingRuntimeTimerSample] ACCUMULATING RUNTIME TIMER SERIALIZED. TIME ELAPSED: {timeProgress.ToString()}");
    }

    private bool Load()
    {
        object dto;
        
        bool json = UnityEngine.Random.Range(0f, 1f) > 0.5f;

        if (json)
            jsonSerializer.Deserialize(jsonTextFileArgument, runtimeTimerAsVisitable.DTOType, out dto);
        else
            xmlSerializer.Deserialize(xmlTextFileArgument, runtimeTimerAsVisitable.DTOType, out dto);
        
        bool result = runtimeTimerAsVisitable.Accept(loadVisitor, dto);

        if (result)
        {
            var timeProgress = runtimeTimer.TimeElapsed;

            Debug.Log(
                $"[AccumulatingRuntimeTimerSample] ACCUMULATING RUNTIME TIMER DESERIALIZED. METHOD: \"{(json ? "JSON" : "XML")}\" TIME ELAPSED: {timeProgress.ToString()}");
        }

        return result;
    }
}
