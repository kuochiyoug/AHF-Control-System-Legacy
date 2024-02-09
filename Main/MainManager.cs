using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.SceneManagement;
using CutieroidUtility;


public class MainManager : MonoBehaviour
{
    public Scene FacialScene { get; private set; }
    public Scene MotorControlScene { get; private set; }

    public List<Scene> LoadedScenes { get; private set; }


    public bool doLoadScene = true;

    public  MotionPlayer MotionPlayer;
    public  FacialControlAPI FacialControlAPI;
    public RobotConstructor Robot;
    //public AngleSetter AngleSetter;

    private bool hasEventSystem = false;
    private bool hasAudioListener = false;

    private delegate void SceneEventHandler();
    private event SceneEventHandler OnFacialSceneEvent;
    private event SceneEventHandler OnMotorControlSceneEvent;

    string FacialControlAPISceneName = "ProjectionFace";
    string MotorControlPanelSceneName = "MotorControlPanel";
    string FullBodySimulationSceneName = "FullBodySimulationVMCControl";

    private void Awake()
    {
        LoadedScenes = new List<Scene>();
        if (doLoadScene)
        {
            OnMotorControlSceneEvent += OnMotorControlScene;
            OnFacialSceneEvent += OnFacialScene;
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log("Loading Scenes...");
            LoadSceneAdditively(FacialControlAPISceneName);
            LoadSceneAdditively(MotorControlPanelSceneName);
            LoadSceneAdditively(FullBodySimulationSceneName);
            //LoadSceneAdditively("callscene");
            //StartCoroutine(DisableEventSystemAtSubscenes());
        }
    }

    void Start() 
    {

    }

    private void Update()
    {
        
    }

    private void LoadSceneAdditively(string sceneName)
    {
        if ( isScene_CurrentlyLoaded(sceneName))
        {
            Debug.Log(sceneName + " already loaded!");
            return;
        }
        try
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
        catch(System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene " + scene.name + " is loaded!");
        LoadedScenes.Add(scene);

        //Check DontDestroyOnLoadScene
        if (CheckDontDestroyContainsCoponent<AudioListener>())
        {
            Debug.Log("Found AudioListener in DontDestroyOnLoadScene!");
            hasAudioListener = true;
        }

        if (CheckDontDestroyContainsCoponent<EventSystem>())
        {
            Debug.Log("Found EventSystem in DontDestroyOnLoadScene!");
            hasEventSystem = true;
        }

        List<AudioListener> audioListeners = CutieroidUtility.SceneUtility.GetTypeComponentsInScene<AudioListener>(scene);
        List<EventSystem> eventSystems = CutieroidUtility.SceneUtility.GetTypeComponentsInScene<EventSystem>(scene);
        CheckAudioListenerAndLeaveOneOnly(audioListeners);
        CheckEventSystemAndLeaveOneOnly(eventSystems);

        if (scene.name == FacialControlAPISceneName)
        {
            FacialScene = scene;
            OnFacialSceneEvent.Invoke();
        }

        if (scene.name == MotorControlPanelSceneName)
        {
            MotorControlScene = scene;
            OnMotorControlSceneEvent.Invoke();
        }

    }

    void OnFacialScene()
    {
        Debug.Log("[buildtest] Facial Scene valid " + FacialScene.IsValid());
        GameObject FaceModelGObj = CutieroidUtility.SceneUtility.GetGameObjectInScene(FacialScene, "FaceModel V6");
        FacialControlAPI = FaceModelGObj?.GetComponent<FacialControlAPI>();
        if (FacialControlAPI == null)
        {
            Debug.LogWarning("No facial api found at "+ FacialScene.name+"! Please check!");
            return;
        }
        Debug.Log("got facial api");
        //CutieroidUtility.SceneUtility.DisableAllEventSystemsInScene(FacialScene);
    }

    void OnMotorControlScene()
    {
        GameObject MotorControlAPIGObj = CutieroidUtility.SceneUtility.GetGameObjectInScene(MotorControlScene, "MotorControl");
        MotionPlayer = MotorControlAPIGObj?.GetComponent<MotionPlayer>();
        if (MotionPlayer == null)
        {
            Debug.LogWarning("No MotionPlayer found at " + MotorControlScene.name + "! Please check!");
            return;
        }
        Debug.Log("Got a MotionPlayer");

        Robot = MotorControlAPIGObj?.GetComponent<RobotConstructor>();
        if (Robot == null)
        {
            Debug.LogWarning("No RobotConstructor found at " + MotorControlScene.name + "! Please check!");
            return;
        }
        Debug.Log("got MotorContorlAPI");

        //CutieroidUtility.SceneUtility.DisableAllEventSystemsInScene(MotorControlScene);

    }

    void CheckAudioListenerAndLeaveOneOnly(List<AudioListener> audiolistenerList)
    {
        if (audiolistenerList.Count == 0) return;
        
        foreach(var audiolistener in audiolistenerList)
        {
            if (hasAudioListener)
            {
                Debug.Log("Already has AudioListener!");
                audiolistener.enabled = false;
                Debug.Log("Disable AudioListner in Scene " + audiolistener.gameObject.scene.name + " on " + audiolistener.gameObject.name + "!");
            }
            else
            {
                if (audiolistener.enabled)
                    hasAudioListener = true;
                continue;
            }
            
        }
    }

    private bool CheckDontDestroyContainsCoponent<T>()
    {
        GameObject[] gameObjects = CutieroidUtility.DontDestroyOnLoadCollector.CollectDontDestroyOnLoad();
        List<T> components = CutieroidUtility.SceneUtility.GetTypeComponentsInGameObjectArray<T>(gameObjects);
        foreach (var component in components)
        {
            return true;
        }
        return false;
    }

    void CheckEventSystemAndLeaveOneOnly(List<EventSystem> evenetsystemList)
    {
        if (evenetsystemList.Count == 0) return;
        
        foreach (var evenetsystem in evenetsystemList)
        {
            if (hasEventSystem)
            {
                Debug.Log("Already has EventSystem!");
                evenetsystem.enabled = false;
                Debug.Log("Disable EventSystem in Scene " + evenetsystem.gameObject.scene.name + " on " + evenetsystem.gameObject.name + "!");
            }
            else
            {
                hasEventSystem = true;
                continue;
            }
        }
    }

    private void OnDestroy()
    {
        OnFacialSceneEvent -= OnFacialScene;
        OnMotorControlSceneEvent -= OnMotorControlScene;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private static bool isScene_CurrentlyLoaded(string sceneName_no_extention)
    {
        for (int i = 0; i < SceneManager.sceneCount; ++i)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == sceneName_no_extention)
            {
                //the scene is already loaded
                return true;
            }
        }
        return false;//scene not currently loaded in the hierarchy
    }

    public void ApplicationQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
        UnityEngine.Application.Quit();
        return;
#endif
    }
}