using Klyte.Commons.Interfaces;
using System.Reflection;

[assembly: AssemblyVersion("2.0.0.2")]
namespace Klyte.AnxietyReducer
{
    public class AnxietyReducerMod : BasicIUserMod<AnxietyReducerMod, ARController, ARPanel>
    {
        public override string SimpleName => "Anxiety Reducer";

        public override string Description => "Make all wait counters of citizens instances to get slow down on increasing";
    }
}
