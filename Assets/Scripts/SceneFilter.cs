using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneFilter : MonoBehaviour
{
    public GameObject buttonPrefab;

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
                SceneManager.LoadScene(currentSceneName, LoadSceneMode.Single);
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
}
