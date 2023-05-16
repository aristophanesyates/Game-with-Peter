using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Knife
{
    namespace Scenes
    {
        public class SceneLoadManager : MonoBehaviour
        {
            public static SceneLoadManager instance;
            private static void InitIfNeeded() // if no instance exists, creates one
            {
                if (!instance)
                {
                    Debug.Log("");
                    instance = new GameObject("SceneManager").AddComponent<SceneLoadManager>();
                }
            }
            // Start is called before the first frame update

            // Maybe consider using int / index / enum (build order)
            public static void ChangeScene(string sceneName)
            {
                InitIfNeeded();
                instance.StartCoroutine(instance.InitiateSceneChange(sceneName));
            }
            private IEnumerator InitiateSceneChange(string sceneName)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
                while (!UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).isLoaded)
                {
                    yield return new WaitForSeconds(0.05f);
                }
                UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName));
            }
        }
    }
}
