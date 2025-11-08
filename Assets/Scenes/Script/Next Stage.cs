using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerOnCollision : MonoBehaviour
{
    [Header("Scene to load (must be added to Build Settings)")]
    [Tooltip("Name of the scene to load")]
    public string nextSceneName;

    [Header("Trigger settings")]
    [Tooltip("Tag that triggers the scene change (default = Player)")]
    public string targetTag = "Player";
    [Tooltip("If true use trigger collider (OnTriggerEnter2D), otherwise use collision (OnCollisionEnter2D)")]
    public bool useTrigger = true;

    [Header("Loading options")]
    [Tooltip("Load scene asynchronously")]
    public bool useAsync = true;
    [Tooltip("Delay before loading (seconds)")]
    public float delaySeconds = 0f;

    bool isLoading = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!useTrigger || isLoading || other == null) return;
        if (other.CompareTag(targetTag))
            StartCoroutine(HandleLoad());
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (useTrigger || isLoading || collision == null || collision.collider == null) return;
        if (collision.collider.CompareTag(targetTag))
            StartCoroutine(HandleLoad());
    }

    IEnumerator HandleLoad()
    {
        if (isLoading) yield break;

        if (string.IsNullOrWhiteSpace(nextSceneName))
        {
            Debug.LogError($"{nameof(SceneChangerOnCollision)}: nextSceneName is empty.");
            yield break;
        }

        if (!SceneInBuild(nextSceneName))
        {
            Debug.LogError($"{nameof(SceneChangerOnCollision)}: Scene '{nextSceneName}' not found in Build Settings.");
            yield break;
        }

        isLoading = true;

        if (delaySeconds > 0f)
            yield return new WaitForSeconds(delaySeconds);

        if (useAsync)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);
            if (op == null)
            {
                Debug.LogError($"{nameof(SceneChangerOnCollision)}: Failed to start async load for '{nextSceneName}'.");
                isLoading = false;
                yield break;
            }
            while (!op.isDone)
                yield return null;
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
            yield return null;
        }
    }

    bool SceneInBuild(string sceneName)
    {
        int count = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < count; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (name == sceneName) return true;
        }
        return false;
    }
}
