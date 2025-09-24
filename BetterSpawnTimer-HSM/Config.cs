using Exiled.API.Interfaces;
using HintServiceMeow.Core.Enum;
using System.ComponentModel;

namespace BetterSpawnTimer_HSM
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public string MtfText { get; set; } = "<color=#6D9FF7>Mobile Task Force</color>";
        public string CiText { get; set; } = "<color=#608F38>Chaos Insurgency</color>";
        public string MtfTimeColor { get; set; } = "#6D9FF7";
        public string CiTimeColor { get; set; } = "#608F38";
        public string MtfMiniText { get; set; } = " ";
        public string CiMiniText { get; set; } = " ";

        [Description("Hint confis")]
        public float XPos { get; set; } = 0;
        public float YPos { get; set; } = 100;
        public HintAlignment Alignment { get; set; } = HintAlignment.Center;
        public HintVerticalAlign VerticalAlign { get; set; } = HintVerticalAlign.Middle;
    }
}
