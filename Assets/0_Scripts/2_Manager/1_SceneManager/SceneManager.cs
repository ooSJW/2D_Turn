/*
	* Coder :
	* Last Update :
	* Information
*/
namespace project02
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public partial class SceneManager : MonoBehaviour // Data Field
    {
        public BaseScene ActiveScene { get; private set; } = default;
        public string LoadSceneName { get; private set; } = default;
    }

    public partial class SceneManager : MonoBehaviour // Initialize
    {
        private void Allocate()
        {

        }
        public void Initialize()
        {
            Allocate();
            Setup();
        }
        private void Setup()
        {

        }
    }

    public partial class SceneManager : MonoBehaviour // Sign
    {
        public void SignUpActiveScene(BaseScene activeSceneValue)
        {
            ActiveScene = activeSceneValue;
            ActiveScene.Initialize();
        }
        public void SignOutActiveScene()
        {
            ActiveScene = null;
        }
    }

    public partial class SceneManager : MonoBehaviour // Property
    {
        public void LoadScene(string sceneName)
        {
            LoadSceneName = sceneName;
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName.LoadingScene.ToString());
        }
    }
}
