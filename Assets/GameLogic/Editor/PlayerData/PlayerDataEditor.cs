using System.IO;
using CoinDash.GameLogic.Runtime.PlayerData;
using UnityEditor;
using UnityEngine;

namespace CoinDash.GameLogic.Editor.PlayerData
{
    public class PlayerDataEditor
    {
        // private static IEnumerable<string> ListSaves() {
        //     foreach (var path in Directory.EnumerateFiles(Application.persistentDataPath)) {
        //         if (Path.GetExtension(path) == FileExtension) {
        //             yield return Path.GetFileNameWithoutExtension(path);
        //         }
        //     }
        // }

        [MenuItem("PlayerData/Delete All Save Files", priority = 1000)]
        public static void DeleteAllFiles()
        {
            var savingPath = Path.Combine(Application.persistentDataPath, PlayerDataManager.SavingSubFolder);
            if (Directory.Exists(savingPath))
            {
                Directory.Delete(savingPath, true);
            }
        }

        [MenuItem("PlayerData/Open Save Folder", priority = 100)]
        public static void OpenSaveFolder()
        {
            EditorUtility.RevealInFinder(Path.Combine(Application.persistentDataPath, PlayerDataManager.SavingSubFolder));
        }
    }
}