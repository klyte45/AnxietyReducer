using Klyte.Commons.Interfaces;
using Klyte.Commons.Utils;

namespace Klyte.AnxietyReducer
{
    public class ARController : BaseController<AnxietyReducerMod, ARController>
    {
        public static readonly string FOLDER_NAME = "Anxiety Reducer";
        public static readonly string FOLDER_PATH = FileUtils.BASE_FOLDER_PATH + FOLDER_NAME;

    }
}