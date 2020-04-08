using ColossalFramework.Globalization;
using ColossalFramework.UI;
using Klyte.AnxietyReducer.Data;
using Klyte.Commons.Extensors;
using Klyte.Commons.Interfaces;
using Klyte.Commons.Utils;
using UnityEngine;

namespace Klyte.AnxietyReducer
{
    public class ARPanel : BasicKPanel<AnxietyReducerMod, ARController, ARPanel>
    {
        public override float PanelWidth => 400;

        public override float PanelHeight => 130;

        protected override void AwakeActions()
        {
            KlyteMonoUtils.CreateUIElement(out UIPanel layoutPanel, MainPanel.transform, "LayoutPanel", new Vector4(0, 40, PanelWidth, PanelHeight - 40));
            layoutPanel.padding = new RectOffset(8, 8, 10, 10);
            layoutPanel.autoLayout = true;
            layoutPanel.autoLayoutDirection = LayoutDirection.Vertical;
            layoutPanel.autoLayoutPadding = new RectOffset(0, 0, 10, 10);
            var uiHelper = new UIHelperExtension(layoutPanel);



            UILabel labelReduction = null;
            UISlider reductionSlider = uiHelper.AddSlider(Locale.Get("K45_AR_ANXIETY_REDUCTION_FACTOR"), 0, 254, 1, AnxietyData.Instance.AnxietyFactor, (x) =>
            {
                AnxietyData.Instance.AnxietyFactor = (byte)Mathf.RoundToInt(x);
                labelReduction.suffix = (x/256f).ToString("P1");
            }, out labelReduction);
            reductionSlider.width = PanelWidth - 15;
            labelReduction.width = PanelWidth - 15;
            labelReduction.minimumSize = new Vector2(PanelWidth - 15, 0);
            labelReduction.suffix = (AnxietyData.Instance.AnxietyFactor/256f).ToString("P1");
            labelReduction.wordWrap = false;
            ((UIPanel)(labelReduction.parent)).autoLayoutPadding = new RectOffset();
            KlyteMonoUtils.LimitWidthAndBox(labelReduction);
        }
    }
}