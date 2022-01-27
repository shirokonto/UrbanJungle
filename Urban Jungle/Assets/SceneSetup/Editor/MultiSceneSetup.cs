using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Utils.Editor
{
    [CreateAssetMenu]
    public class MultiSceneSetup : ScriptableObject
    {
        public SceneSetup[] Setups;
    }

    public static class MultiSceneSetupMenu
    {
        [MenuItem("Assets/Multi Scene Setup/Create")]
        public static void CreateNewSceneSetup()
        {
            var folderPath = TryGetSelectedFolderPathInProjectsTab();

            var assetPath = ConvertFullAbsolutePathToAssetPath(Path.Combine(folderPath, "SceneSetup.asset"));

            SaveCurrentSceneSetup(assetPath);
        }

        [MenuItem("Assets/Multi Scene Setup/Create", true)]
        public static bool CreateNewSceneSetupValidate()
        {
            return TryGetSelectedFolderPathInProjectsTab() != null;
        }

        [MenuItem("Assets/Multi Scene Setup/Overwrite")]
        public static void SaveSceneSetup()
        {
            var assetPath = ConvertFullAbsolutePathToAssetPath(
                TryGetSelectedFilePathInProjectsTab());

            SaveCurrentSceneSetup(assetPath);
        }

        static void SaveCurrentSceneSetup(string assetPath)
        {
            var loader = ScriptableObject.CreateInstance<MultiSceneSetup>();

            loader.Setups = EditorSceneManager.GetSceneManagerSetup();

            AssetDatabase.CreateAsset(loader, assetPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log(string.Format("Scene setup '{0}' saved", Path.GetFileNameWithoutExtension(assetPath)));
        }

        [MenuItem("Assets/Multi Scene Setup/Load")]
        public static void RestoreSceneSetup()
        {
            var assetPath = ConvertFullAbsolutePathToAssetPath(
                TryGetSelectedFilePathInProjectsTab());

            var loader = AssetDatabase.LoadAssetAtPath<MultiSceneSetup>(assetPath);

            EditorSceneManager.RestoreSceneManagerSetup(loader.Setups);

            Debug.Log(string.Format("Scene setup '{0}' restored", Path.GetFileNameWithoutExtension(assetPath)));
        }

        [MenuItem("Assets/Multi Scene Setup", true)]
        public static bool SceneSetupRootValidate()
        {
            return HasSceneSetupFileSelected();
        }

        [MenuItem("Assets/Multi Scene Setup/Overwrite", true)]
        public static bool SaveSceneSetupValidate()
        {
            return HasSceneSetupFileSelected();
        }

        [MenuItem("Assets/Multi Scene Setup/Load", true)]
        public static bool RestoreSceneSetupValidate()
        {
            return HasSceneSetupFileSelected();
        }

        static bool HasSceneSetupFileSelected()
        {
            return TryGetSelectedFilePathInProjectsTab() != null;
        }

        static List<string> GetSelectedFilePathsInProjectsTab()
        {
            return GetSelectedPathsInProjectsTab()
                .Where(x => File.Exists(x)).ToList();
        }

        static string TryGetSelectedFilePathInProjectsTab()
        {
            var selectedPaths = GetSelectedFilePathsInProjectsTab();

            if (selectedPaths.Count == 1)
            {
                return selectedPaths[0];
            }

            return null;
        }

        // Returns the best guess directory in projects pane
        // Useful when adding to Assets -> Create context menu
        // Returns null if it can't find one
        // Note that the path is relative to the Assets folder for use in AssetDatabase.GenerateUniqueAssetPath etc.
        static string TryGetSelectedFolderPathInProjectsTab()
        {
            var selectedPaths = GetSelectedFolderPathsInProjectsTab();

            if (selectedPaths.Count == 1)
            {
                return selectedPaths[0];
            }

            return null;
        }

        // Note that the path is relative to the Assets folder
        static List<string> GetSelectedFolderPathsInProjectsTab()
        {
            return GetSelectedPathsInProjectsTab()
                .Where(x => Directory.Exists(x)).ToList();
        }

        static List<string> GetSelectedPathsInProjectsTab()
        {
            var paths = new List<string>();

            UnityEngine.Object[] selectedAssets = Selection.GetFiltered(
                typeof(UnityEngine.Object), SelectionMode.Assets);

            foreach (var item in selectedAssets)
            {
                var relativePath = AssetDatabase.GetAssetPath(item);

                if (!string.IsNullOrEmpty(relativePath))
                {
                    var fullPath = Path.GetFullPath(Path.Combine(
                        Application.dataPath, Path.Combine("..", relativePath)));

                    paths.Add(fullPath);
                }
            }

            return paths;
        }

        static string ConvertFullAbsolutePathToAssetPath(string fullPath)
        {
            return "Assets/" + Path.GetFullPath(fullPath)
                .Remove(0, Path.GetFullPath(Application.dataPath).Length + 1)
                .Replace("\\", "/");
        }
    }
}