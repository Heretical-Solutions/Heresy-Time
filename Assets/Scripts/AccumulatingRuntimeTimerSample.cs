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
    private UnityFileSystemSettings fsSettings;

    [SerializeField]
    private float autosaveCooldown = 5f;

    [SerializeField]
    private float debugCountdown;
    
    private IRuntimeTimer runtimeTimer;

    private ISaveVisitor saveVisitor;

    private ISerializer serializer;

    private UnityTextFileArgument textFileArgument;
    
    private float countdown;
    
    // Start is called before the first frame update
    void Start()
    {
        runtimeTimer = TimersFactory.BuildRuntimeTimer(
            "AccumulatingPersistentTimer",
            0f);

        runtimeTimer.Accumulate = true;
        
        saveVisitor = PersistenceFactory.BuildSimpleCompositeVisitorWithTimerVisitors();

        serializer = PersistenceFactory.BuildSimpleUnityJSONSerializer();

        textFileArgument = new UnityTextFileArgument();

        textFileArgument.Settings = fsSettings;

        countdown = autosaveCooldown;
        
        
        runtimeTimer.Start();
        
        Save();
    }

    // Update is called once per frame
    void Update()
    {
        ((ITickable)runtimeTimer).Tick(Time.deltaTime);

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
        //saveVisitor.Save<IRuntimeTimer, RuntimeTimerDTO>(runtimeTimer, out RuntimeTimerDTO dto);

        //serializer.Serialize<RuntimeTimerDTO>(textFileArgument, dto);
        
        ((IVisitable)runtimeTimer).Accept(saveVisitor, out var dto);
        
        serializer.Serialize(textFileArgument, dto);
        
        var timeProgress = runtimeTimer.TimeElapsed;
        
        Debug.Log($"[AccumulatingRuntimeTimerSample] ACCUMULATING RUNTIME TIMER SERIALIZED. TIME ELAPSED: {timeProgress.ToString()}");
    }
}
