using Exiled.API.Features;
using Exiled.API.Features.Waves;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterSpawnTimer
{
    internal class EventHandlers
    {
        private CoroutineHandle _coroutineTimer;

        public void OnWaintingForPlayers() => KillCoroutine();
        public void OnRoundStarted() => _coroutineTimer = Timing.RunCoroutine(BetterSpawnTimer());
        public void OnRestartingRound() => KillCoroutine();
        public void OnRoundEnded(RoundEndedEventArgs ev) => KillCoroutine();

        public void KillCoroutine()
        {
            if (Timing.IsRunning(_coroutineTimer))
                Timing.KillCoroutines(_coroutineTimer);
        }

        private IEnumerator<float> BetterSpawnTimer()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(1f);
                Player.List
                    .Where(p => p.Role.Type == RoleTypeId.Spectator)
                    .ToList()
                    .ForEach(p => p.ShowHint($"<b><size=50%>{TimeToSpawn()}</size></b>" +
                    Plugin.Instance.Config.height, 1.25f));
            }
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

            if (respawn == Exiled.API.Enums.SpawnableFaction.NtfMiniWave) return Plugin.Instance.Config.mtf_mini;
            if (respawn == Exiled.API.Enums.SpawnableFaction.ChaosMiniWave) return Plugin.Instance.Config.ci_mini;

            if (mtfWaveTokens == 0 && chaosWaveTokens == 0) return " ";

            string FormatTimer(WaveTimer wave, string color, string name) =>
                wave.TimeLeft < TimeSpan.FromSeconds(1) ? name : $"<color={color}>{wave.TimeLeft.Minutes}:{wave.TimeLeft.Seconds:00}</color>";

            if (minTimer.Name == "NtfSpawnWave")
            {
                if (mtfWaveTokens > 0) return FormatTimer(minTimer, Plugin.Instance.Config.mtf_time_color, Plugin.Instance.Config.mtf);
                if (chaosWaveTokens > 0) return FormatTimer(chaosWave, Plugin.Instance.Config.ci_time_color, Plugin.Instance.Config.ci);
                return "Error";
            }
            else if (minTimer.Name == "ChaosSpawnWave")
            {
                if (chaosWaveTokens > 0) return FormatTimer(minTimer, Plugin.Instance.Config.ci_time_color, Plugin.Instance.Config.ci);
                if (mtfWaveTokens > 0) return FormatTimer(ntfWave, Plugin.Instance.Config.mtf_time_color, Plugin.Instance.Config.mtf);
                return "Error";
            }

            return "Error";
        }
    }
}
