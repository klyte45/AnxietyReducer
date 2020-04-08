using Klyte.AnxietyReducer;

namespace Klyte.Commons
{
    public static class CommonProperties
    {
        public static bool DebugMode => AnxietyReducerMod.DebugMode;
        public static string Version => AnxietyReducerMod.Version;
        public static string ModName => AnxietyReducerMod.Instance.SimpleName;
        public static string Acronym => "AR";
        public static string ModRootFolder => ARController.FOLDER_PATH; 
    }
}