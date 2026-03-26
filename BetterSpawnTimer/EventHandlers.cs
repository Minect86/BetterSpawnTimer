using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Waves;
using Exiled.Events.EventArgs.Server;
using LabApi.Features.Extensions;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BetterSpawnTimer
{
    public class EventHandlers
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

                string text = Plugin.Instance.Config.TextFormatting;
                if (text.Contains("%timer%"))
                    text = text.Replace("%timer%", TimeToSpawn());

                Player.List
                    .Where(p => p.Role.Type == RoleTypeId.Spectator)
                    .ToList()
                    .ForEach(p => p.ShowHint(text +
                    string.Concat(Enumerable.Repeat("\r\n", Plugin.Instance.Config.HintHeight)), 1.25f));
            }
        }

        private string TimeToSpawn()
        {
            if (Respawn.IsSpawning)
            {
                switch (Respawn.NextKnownSpawnableFaction)
                {
                    case SpawnableFaction.NtfWave:
                        return Plugin.Instance.Config.MtfText;
                    case SpawnableFaction.ChaosWave:
                        return Plugin.Instance.Config.CiText;
                    case SpawnableFaction.NtfMiniWave:
                        return Plugin.Instance.Config.MtfMiniText;
                    case SpawnableFaction.ChaosMiniWave:
                        return Plugin.Instance.Config.CiMiniText;
                }
            }

            WaveTimer wave = WaveTimer.GetWaveTimers().Where(w => GetTokens(w.Name) > 0).MinBy(w => w.TimeLeft);

            if (wave == null)
                return string.Empty;

            switch (wave.Name)
            {
                case "NtfSpawnWave":
                    return FormatTimer(wave, Plugin.Instance.Config.MtfTimeColor, Plugin.Instance.Config.MtfText);
                case "ChaosSpawnWave":
                    return FormatTimer(wave, Plugin.Instance.Config.CiTimeColor, Plugin.Instance.Config.CiText);
            }

            return string.Empty;
        }

        private string FormatTimer(WaveTimer wave, string color, string name) =>
                wave.TimeLeft < TimeSpan.FromSeconds(1) ? name : $"<color={color}>{wave.TimeLeft.Minutes}:{wave.TimeLeft.Seconds:00}</color>";

        private static readonly Dictionary<string, SpawnableFaction> WaveFactionMap =
            new Dictionary<string, SpawnableFaction>
            {
                { "NtfSpawnWave", SpawnableFaction.NtfWave },
                { "ChaosSpawnWave", SpawnableFaction.ChaosWave },
                { "NtfMiniWave", SpawnableFaction.NtfMiniWave },
                { "ChaosMiniWave", SpawnableFaction.ChaosMiniWave }
            };

        private int GetTokens(string name)
        {
            if (WaveFactionMap.TryGetValue(name, out SpawnableFaction faction))
            {
                if (Respawn.TryGetTokens(faction, out int tokens))
                    return tokens;
            }

            return 0;
        }
    }
}