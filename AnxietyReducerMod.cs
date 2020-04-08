using ColossalFramework;
using ColossalFramework.Globalization;
using ColossalFramework.UI;
using ICities;
using Klyte.AnxietyReducer.Interfaces;
using Klyte.AnxietyReducer.Overrides;
using Klyte.AnxietyReducer.Utils;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

[assembly: AssemblyVersion("1.0.0.0")]
namespace Klyte.AnxietyReducer
{
    public class AnxietyReducerMod : IUserMod
    {
        public string SimpleName => "Anxiety Reducer By Klyte";

        public string Description => "Make all wait counters of citizens instances to get slow down on increasing";

        public string Name => $"{SimpleName} {version}";


        public static string minorVersion => majorVersion + "." + typeof(AnxietyReducerMod).Assembly.GetName().Version.Build;
        public static string majorVersion => typeof(AnxietyReducerMod).Assembly.GetName().Version.Major + "." + typeof(AnxietyReducerMod).Assembly.GetName().Version.Minor;
        public static string fullVersion => minorVersion + " r" + typeof(AnxietyReducerMod).Assembly.GetName().Version.Revision;
        public static string version
        {
            get {
                if (typeof(AnxietyReducerMod).Assembly.GetName().Version.Minor == 0 && typeof(AnxietyReducerMod).Assembly.GetName().Version.Build == 0)
                {
                    return typeof(AnxietyReducerMod).Assembly.GetName().Version.Major.ToString();
                }
                if (typeof(AnxietyReducerMod).Assembly.GetName().Version.Build > 0)
                {
                    return minorVersion;
                }
                else
                {
                    return majorVersion;
                }
            }
        }

        private static SavedInt savedAnxietyCounterMultiplier = new SavedInt("KLT_AR_FACTOR", Settings.gameSettingsFile, 2, true);

        internal static int multiplier => savedAnxietyCounterMultiplier.value;

        public void OnSettingsUI(UIHelperBase helper)
        {
            ((UIDropDown)helper.AddDropdown("Reducer multiplier", Enumerable.Range(1, 25).Select(x => $"{1000 / x / 10f}%").ToArray(), savedAnxietyCounterMultiplier.value - 1, (x) => savedAnxietyCounterMultiplier.value = x + 1)).tooltip = "x1 means no change, ";
        }

    }
}
