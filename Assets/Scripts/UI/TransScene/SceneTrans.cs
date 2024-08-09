using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTrans : MonoBehaviour
{
    Animator transition;
    GameObject blackCanvas;
    enum SceneType
    {
        StartScene,
        AtlasScene,
        NodeScene
    }
    SceneType currSceneType;
    public static SceneTrans sce_instance;
    public static SceneTrans instance
    {
        get
        {
            if(sce_instance == null)
            {
                sce_instance = FindObjectOfType<SceneTrans>();
            }
            return sce_instance;
        }
    }
    void Awake()
    {
        if(sce_instance == null)
        {
            sce_instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // 加载BlackCanvas预制件
        LoadBlackCanvas();

        SceneManager.sceneLoaded += OnSceneLoaded;//注册场景加载事件

        currSceneType = SceneType.StartScene;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadBlackCanvas();
    }

    public void LoadScene(string sceneName)
    {
        //StartCoroutine(LoadNextScene(sceneName));
        switch(sceneName)
        {
            case "Start":
                StartCoroutine(LoadNextScene("Start"));
                currSceneType = SceneType.StartScene;
                break;
            case "Atlas_1":
                if (currSceneType == SceneType.NodeScene) 
                {
                    if (SaveManager.instance.jsonData.mapData.currAtlasID == SaveManager.instance.jsonData.mapData.backAtlasID)//如果当前地区和将要前往的地区相同，则为返回地图
                    {
                        SaveManager.instance.isBackFromNodeScene = true;
                    }
                    else//否则为前往新地图，需要重新生成地图数据
                    {
                        SaveManager.instance.jsonData.mapData = new MapData();
                        SaveManager.instance.jsonData.mapData.mapNodes = new List<NodesListUnit>();
                        SaveManager.instance.jsonData.mapData.currAtlasID = MapManager.AtlasID.Atlas_1;
                        SaveManager.instance.Save();
                    }
                }
                StartCoroutine(LoadNextScene("Atlas_1"));
                currSceneType = SceneType.AtlasScene;
                break;
            case "Atlas_2":
                if (currSceneType == SceneType.NodeScene) 
                {
                    Debug.Log("当前地图id" + SaveManager.instance.jsonData.mapData.currAtlasID + "返回地图id" + SaveManager.instance.jsonData.mapData.backAtlasID);
                    if (SaveManager.instance.jsonData.mapData.currAtlasID == SaveManager.instance.jsonData.mapData.backAtlasID)//如果当前地区和将要前往的地区相同，则为返回地图
                    {
                        SaveManager.instance.isBackFromNodeScene = true;
                        Debug.Log("未重新生成地图数据");
                    }
                    else//否则为前往新地图，需要重新生成地图数据
                    {
                        SaveManager.instance.jsonData.mapData = new MapData();
                        SaveManager.instance.jsonData.mapData.mapNodes = new List<NodesListUnit>();
                        SaveManager.instance.jsonData.mapData.currAtlasID = MapManager.AtlasID.Atlas_2;
                        //SaveManager.instance.Save();
                        Debug.Log("加载Atlas_2");
                    }
                }
                StartCoroutine(LoadNextScene("Atlas_2"));
                currSceneType = SceneType.AtlasScene;
                break;
            case "Atlas_3":
                if (currSceneType == SceneType.NodeScene) 
                {
                    if (SaveManager.instance.jsonData.mapData.currAtlasID == SaveManager.instance.jsonData.mapData.backAtlasID)//如果当前地区和将要前往的地区相同，则为返回地图
                    {
                        SaveManager.instance.isBackFromNodeScene = true;
                    }
                    else//否则为前往新地图，需要重新生成地图数据
                    {
                        SaveManager.instance.jsonData.mapData = new MapData();
                        SaveManager.instance.jsonData.mapData.mapNodes = new List<NodesListUnit>();
                        SaveManager.instance.jsonData.mapData.currAtlasID = MapManager.AtlasID.Atlas_3;
                        SaveManager.instance.Save();
                    }
                }
                StartCoroutine(LoadNextScene("Atlas_3"));
                currSceneType = SceneType.AtlasScene;
                break;
            case "Atlas_4":
                if (currSceneType == SceneType.NodeScene) 
                {
                    if (SaveManager.instance.jsonData.mapData.currAtlasID == SaveManager.instance.jsonData.mapData.backAtlasID)//如果当前地区和将要前往的地区相同，则为返回地图
                    {
                        SaveManager.instance.isBackFromNodeScene = true;
                    }
                    else//否则为前往新地图，需要重新生成地图数据
                    {
                        SaveManager.instance.jsonData.mapData = new MapData();
                        SaveManager.instance.jsonData.mapData.mapNodes = new List<NodesListUnit>();
                        SaveManager.instance.jsonData.mapData.currAtlasID = MapManager.AtlasID.Atlas_4;
                        SaveManager.instance.Save();
                    }
                }
                StartCoroutine(LoadNextScene("Atlas_4"));
                currSceneType = SceneType.AtlasScene;
                break;
            case "CardTest":
                StartCoroutine(LoadNextScene("CardTest"));
                currSceneType = SceneType.NodeScene;
                break;
            case "Event":
                StartCoroutine(LoadNextScene("Event"));
                currSceneType = SceneType.NodeScene;
                break;
            case "Shop":
                StartCoroutine(LoadNextScene("Shop"));
                currSceneType = SceneType.NodeScene;
                break;
            case "Plot":
                StartCoroutine(LoadNextScene("Plot"));
                currSceneType = SceneType.NodeScene;
                break;
            case "Atlas_1_Boss":
                StartCoroutine(LoadNextScene("Atlas_1_Boss"));
                currSceneType = SceneType.NodeScene;
                break;
            default:
                //抛出异常
                Debug.LogError("未在SceneTrans中找到对应的场景名，请检查！");
                break;
        }
    }
    IEnumerator LoadNextScene(string sceneName)
    {
        transition.SetTrigger("start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }

    void LoadBlackCanvas()
    {
        // 加载BlackCanvas预制件
        GameObject blackCanvasPrefab = Resources.Load<GameObject>("Prefabs/UI/SceneTrans/BlackCanvas");
        if (blackCanvasPrefab != null)
        {
            // 实例化预制件
            GameObject blackCanvas = Instantiate(blackCanvasPrefab);

            // 创建新的Canvas对象
            GameObject newCanvas = new GameObject("BlackCanvasCanvas");
            Canvas canvasComponent = newCanvas.AddComponent<Canvas>();
            canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasComponent.sortingOrder = 100; // 确保新Canvas在最顶层

            // 添加CanvasScaler和GraphicRaycaster组件
            newCanvas.AddComponent<CanvasScaler>();
            newCanvas.AddComponent<GraphicRaycaster>();

            // 将blackCanvas设置为新Canvas的子对象
            blackCanvas.transform.SetParent(newCanvas.transform, false);

            // 将blackCanvas移动到父对象的最顶层
            blackCanvas.transform.SetAsLastSibling();

            // 获取Animator组件
            transition = blackCanvas.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("BlackCanvas 预制件未找到！");
        }
    }
}
