using Exiled.API.Features;
using Exiled.API.Features.Waves;
using Exiled.Events.EventArgs.Server;
using RueI.Displays;
using RueI.Elements;
using System;
using System.Linq;

namespace BetterSpawnTimer_Ruei
{
    public class EventHandlers
    {
        private AutoElement _timerAutoElement;

        public void OnWaitingForPlayers() => KillAutoElement();
        public void OnRoundStarted() => StartAutoElement();
        public void OnRestartingRound() => KillAutoElement();
        public void OnRoundEnded(RoundEndedEventArgs ev) => KillAutoElement();

        private void KillAutoElement()
        {
            _timerAutoElement?.Disable();
            _timerAutoElement = null;
        }

        private void StartAutoElement()
        {
            _timerAutoElement = new(
                Roles.Spectator,
                core => new DynamicElement(
                    c => Plugin.Instance.Config.TextFormatting.Replace("%timer%", TimeToSpawn()),
                    position: Plugin.Instance.Config.Postion
                )
            );

            _timerAutoElement.UpdateEvery = new AutoElement.PeriodicUpdate(
                time: TimeSpan.FromSeconds(1),
                priority: 10
            );

            _timerAutoElement.Roles = Roles.Spectator;
        }

        private string TimeToSpawn()
        {
            var waves = WaveTimer.GetWaveTimers();
            WaveTimer ntfWave = waves[0];
            WaveTimer chaosWave = waves[1];

            WaveTimer minTimer = new[] { ntfWave, chaosWave }
                .OrderBy(w => w.TimeLeft.TotalSeconds)
                .First();

            var respawn = Respawn.NextKnownSpawnableFaction;
            int mtfWaveTokens = LabApi.Features.Wrappers.RespawnWaves.PrimaryMtfWave.RespawnTokens;
            int chaosWaveTokens = LabApi.Features.Wrappers.RespawnWaves.PrimaryChaosWave.RespawnTokens;

            if (respawn == Exiled.API.Enums.SpawnableFaction.NtfMiniWave) return Plugin.Instance.Config.MtfMiniText;
            if (respawn == Exiled.API.Enums.SpawnableFaction.ChaosMiniWave) return Plugin.Instance.Config.CiMiniText;

            if (mtfWaveTokens == 0 && chaosWaveTokens == 0) return " ";

            string FormatTimer(WaveTimer wave, string color, string name) =>
                wave.TimeLeft < TimeSpan.FromSeconds(1) ? name : $"<color={color}>{wave.TimeLeft.Minutes}:{wave.TimeLeft.Seconds:00}</color>";

            if (minTimer.Name == "NtfSpawnWave")
            {
                if (mtfWaveTokens > 0) return FormatTimer(minTimer, Plugin.Instance.Config.MtfTimeColor, Plugin.Instance.Config.MtfText);
                if (chaosWaveTokens > 0) return FormatTimer(chaosWave, Plugin.Instance.Config.CiTimeColor, Plugin.Instance.Config.CiText);
                return "Error";
            }
            else if (minTimer.Name == "ChaosSpawnWave")
            {
                if (chaosWaveTokens > 0) return FormatTimer(minTimer, Plugin.Instance.Config.CiTimeColor, Plugin.Instance.Config.CiText);
                if (mtfWaveTokens > 0) return FormatTimer(ntfWave, Plugin.Instance.Config.MtfTimeColor, Plugin.Instance.Config.MtfText);
                return "Error";
            }

            return "Error";
        }
    }
}
