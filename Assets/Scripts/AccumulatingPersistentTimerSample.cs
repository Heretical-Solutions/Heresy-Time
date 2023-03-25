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
    private UnityFileSystemSettings fsSettings;

    [SerializeField]
    private float autosaveCooldown = 5f;

    [SerializeField]
    private float debugCountdown;
    
    private IPersistentTimer persistentTimer;

    private ISaveVisitor saveVisitor;

    private ISerializer serializer;

    private UnityTextFileArgument textFileArgument;
    
    private float countdown;
    
    // Start is called before the first frame update
    void Start()
    {
        persistentTimer = TimersFactory.BuildPersistentTimer(
            "AccumulatingPersistentTimer",
            default(TimeSpan));

        persistentTimer.Accumulate = true;
        
        saveVisitor = PersistenceFactory.BuildSimpleCompositeVisitorWithTimerVisitors();

        serializer = PersistenceFactory.BuildSimpleUnityJSONSerializer();

        textFileArgument = new UnityTextFileArgument();

        textFileArgument.Settings = fsSettings;

        countdown = autosaveCooldown;
        
        
        persistentTimer.Start();
        
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
        //saveVisitor.Save<IPersistentTimer, PersistentTimerDTO>(persistentTimer, out PersistentTimerDTO dto);
        
        //serializer.Serialize<PersistentTimerDTO>(textFileArgument, dto);
        
        ((IVisitable)persistentTimer).Accept(saveVisitor, out var dto);
        
        serializer.Serialize(textFileArgument, dto);
        
        var timeProgress = ((IPersistentTimerContext)persistentTimer).SavedProgress;
        
        Debug.Log($"[AccumulatingPersistentTimerSample] ACCUMULATING PERSISTENT TIMER SERIALIZED. PROGRESS: HOURS: {timeProgress.Hours.ToString()} MINUTES: {timeProgress.Minutes.ToString()} SECONDS: {timeProgress.Seconds.ToString()}");
    }
}
