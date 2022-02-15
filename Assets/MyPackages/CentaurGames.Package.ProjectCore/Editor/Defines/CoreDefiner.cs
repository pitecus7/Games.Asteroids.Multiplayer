using UnityEditor;
using System.Collections.Generic;

namespace CentaurGames.Packages.Games.Core.Editor
{
    public class CoreDefiner
    {
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        public static void AddDefineSymbols()
        {
            string currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            HashSet<string> defines = new HashSet<string>(currentDefines.Split(';'))
        {
            "PROJECT_CORE"
        };

            string newDefines = string.Join(";", defines);
            if (newDefines != currentDefines)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, newDefines);
            }
        }
#endif
    }
}
