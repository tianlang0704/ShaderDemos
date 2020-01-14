using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneFilter : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject backPrefab;

    // Start is called before the first frame update
    void Start()
    {
        ArrayList exceptionSceneNames = new ArrayList() {"Menu", "SampleScene"};

        var dir = new DirectoryInfo("Assets");
        var allSceneFiles = dir.GetFiles("*.unity", SearchOption.AllDirectories);
        foreach(var sceneFile in allSceneFiles) {
            var currentSceneName = sceneFile.Name.Replace(".unity", "");
            if (exceptionSceneNames.Contains(currentSceneName)) { continue; }

            var button = Instantiate(buttonPrefab);
            button.transform.SetParent(gameObject.transform, false);
            Button b = button.GetComponent<Button>();
            b.onClick.AddListener(() => {
                StartCoroutine(LoadSceneAsync(currentSceneName));
                // SceneManager.LoadScene(currentSceneName, LoadSceneMode.Single);
            });
            var buttonText = button.GetComponentInChildren<Text>();
            buttonText.text = currentSceneName;
        }

        var rectTransform = GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator LoadSceneAsync(string sceneName) {
        // 异步切换成精
        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 切换场景后生成返回按钮
        var newScene = SceneManager.GetSceneByName(sceneName);
        var buttonHost = Instantiate(backPrefab);
        buttonHost.transform.SetParent(null, false);
        SceneManager.MoveGameObjectToScene(buttonHost, newScene);
        
        // 卸载当前场景
        SceneManager.UnloadSceneAsync(currentScene);
    }
}
