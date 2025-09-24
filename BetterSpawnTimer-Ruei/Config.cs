using Exiled.API.Interfaces;
using System.ComponentModel;

namespace BetterSpawnTimer_Ruei
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        [Description("Display text for Mobile Task Force")]
        public string MtfText { get; set; } = "<color=#6D9FF7>Mobile Task Force</color>";

        [Description("Display text for Chaos Insurgency")]
        public string CiText { get; set; } = "<color=#608F38>Chaos Insurgency</color>";

        [Description("Timer color for Mobile Task Force")]
        public string MtfTimeColor { get; set; } = "#6D9FF7";

        [Description("Timer color for Chaos Insurgency")]
        public string CiTimeColor { get; set; } = "#608F38";

        [Description("Display text for Mobile Task Force Mini")]
        public string MtfMiniText { get; set; } = " ";

        [Description("Display text for Chaos Insurgency Mini")]
        public string CiMiniText { get; set; } = " ";

        [Description("Formatting text for a timer")]
        public string TextFormatting { get; set; } = "<b><size=50%>%timer%</size></b>";

        [Description("Height for Hint")]
        public float Postion { get; set; } = 970;
    }
}
