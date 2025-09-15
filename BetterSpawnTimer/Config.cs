using Exiled.API.Interfaces;

namespace BetterSpawnTimer
{
    internal class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public string mtf { get; set; } = "<color=#6D9FF7>Mobile Task Force</color>";
        public string ci { get; set; } = "<color=#608F38>Chaos Insurgency</color>";
        public string mtf_time_color { get; set; } = "#6D9FF7";
        public string ci_time_color { get; set; } = "#608F38";
        public string mtf_mini { get; set; } = " ";
        public string ci_mini { get; set; } = " ";
        public string height { get; set; } = "\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n";
    }
}
