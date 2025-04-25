using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityToolbarExtender;

namespace CoinDash.GameLogic.Editor
{
    static class ToolbarStyles
    {
        public static readonly GUIStyle commandButtonStyle;

        static ToolbarStyles()
        {
            commandButtonStyle = new GUIStyle("Command")
            {
                fontSize = 14,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold,
                stretchWidth = true,
                fixedWidth = 70,
            };
        }
    }
    
    [InitializeOnLoad]
    public static class LaunchButton {
        [System.Serializable]
        private class SceneSetupArrayWrapper
        {
            public SceneSetup[] setups;
        }
        
        private static string sceneToOpen;
        // private static SceneSetup[] sceneSetup;
        
        static LaunchButton()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }
        
        static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            if(GUILayout.Button(new GUIContent("Launch", "Launch the game"), ToolbarStyles.commandButtonStyle))
            {
                StartScene("Launch");
            }
        }
        
        public static void StartScene(string sceneName)
        {
            if(EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
            }

            sceneToOpen = sceneName;
            EditorApplication.update += OnUpdate;
            
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.EnteredEditMode)
            {
                string sceneSetupString = SessionState.GetString("sceneSetup", null);
                if (sceneSetupString != null)
                {
                    SceneSetupArrayWrapper sceneSetupWrapper = JsonUtility.FromJson<SceneSetupArrayWrapper>(sceneSetupString);
                    EditorSceneManager.RestoreSceneManagerSetup(sceneSetupWrapper.setups);
                    SessionState.EraseString("sceneSetup");
                }
            }
        }

        static void OnUpdate()
        {
            if (sceneToOpen == null ||
                EditorApplication.isPlaying || EditorApplication.isPaused ||
                EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            EditorApplication.update -= OnUpdate;

            if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                // need to get scene via search because the path to the scene
                // file contains the package version so it'll change over time
                string[] guids = AssetDatabase.FindAssets("t:scene " + sceneToOpen, null);
                if (guids.Length == 0)
                {
                    Debug.LogWarning("Couldn't find scene file");
                }
                else
                {
                    SceneSetupArrayWrapper sceneSetupWrapper = new SceneSetupArrayWrapper();
                    sceneSetupWrapper.setups = EditorSceneManager.GetSceneManagerSetup();
                    string sceneSetup = JsonUtility.ToJson(sceneSetupWrapper);
                    SessionState.SetString("sceneSetup", sceneSetup);
                    
                    string scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);
                    SessionState.SetString("sceneSetup", sceneSetup);
                    EditorSceneManager.OpenScene(scenePath);
                    EditorApplication.isPlaying = true;
                }
            }
            sceneToOpen = null;
        }
    }
}