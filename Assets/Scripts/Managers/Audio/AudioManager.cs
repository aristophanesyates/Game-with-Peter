using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    #region Variables

    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;

    #endregion

    #region Singleton Pattern
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Debug.LogError("More then one Audio Manager in Scene.");
            Destroy(this.gameObject);
        }

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();
    }
    #endregion

    #region Public Methods

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitialiseEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    #endregion

    #region Cleanup Methods

    private void Cleanup()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }

    public void Cleanup(GameObject emitterObject)
    {
        eventEmitters.Remove(emitterObject.GetComponent<StudioEventEmitter>());
    }

    private void OnDestroy()
    {
        Cleanup();
    }

    #endregion
}
