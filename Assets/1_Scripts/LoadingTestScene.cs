using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingTestScene : MonoBehaviour
{
    [Header("NextScene")]
    [SerializeField] private string name_NextScene;

    [Header("UI")]
    [SerializeField] private Slider _loadingBar;
    [SerializeField] private TMP_Text _loadingText;

    private List<AsyncOperationHandle> _operations = new();

    private void Awake()
    {
        InitLoad();        
    }

    private void InitLoad()
        => StartCoroutine(Co_LoadResources());

    private IEnumerator Co_LoadResources()
    {
        _operations.Add(ResourceManager.Instance.LoadAllAsyncByLabel<BaseSO>(Constants.Key_Entities));
        _operations.Add(ResourceManager.Instance.LoadAllAsyncByLabel<GameObject>(Constants.Key_Prefabs));
        _operations.Add(ResourceManager.Instance.LoadAllAsyncByLabel<GameObject>(Constants.Key_Equipments));        
        _loadingText.text = "리소스 로드 중...";
        while (true)
        {
            _loadingBar.value = 0.2f;
            yield return new WaitForSeconds(1f);

            if (IsAllProgressDone())
                break;

            float totalProgress = _operations.Sum(x => x.PercentComplete);
            Debug.Log(totalProgress);
            _loadingBar.value = totalProgress;
            yield return null;
        }
        _loadingBar.value = 1f;

        var sceneLoadOp = SceneManager.LoadSceneAsync(name_NextScene);
        sceneLoadOp.allowSceneActivation = false;
        _loadingText.text = "맵 로드 중...";
        while (sceneLoadOp.isDone)
        {
            float sceneLoadProgress = sceneLoadOp.progress;
            Debug.Log(sceneLoadProgress);
            _loadingBar.value = sceneLoadProgress;
            yield return null;
        }
        _loadingBar.value = 1f;
        sceneLoadOp.allowSceneActivation = true;
    }

    private bool IsAllProgressDone()
    {
        foreach(var operation in _operations)
        {
            if(!operation.IsDone)
                return false;
        }
        return true;
    }
}
