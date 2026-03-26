using Exiled.API.Features;
using Exiled.API.Features.Waves;
using Exiled.Events.EventArgs.Server;
using HintServiceMeow.Core.Utilities;
using LabApi.Features.Extensions;
using MEC;
using Exiled.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Hint = HintServiceMeow.Core.Models.Hints.Hint;

namespace BetterSpawnTimer_HSM
{
    public class EventHandlers
    {
        private CoroutineHandle _timerCoroutine;

        public void OnWaitingForPlayers() => KillCoroutine();
        public void OnRoundStarted() => _timerCoroutine = Timing.RunCoroutine(TimerEnumerator());
        public void OnRestartingRound() => KillCoroutine();
        public void OnRoundEnded(RoundEndedEventArgs ev) => KillCoroutine();

        private void KillCoroutine() => Timing.KillCoroutines(_timerCoroutine);

        private IEnumerator<float> TimerEnumerator()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(1f);

                string text = TimeToSpawn();

                foreach (Player pl in Player.List)
                {
                    UpdateHint(pl, text);
                }
            }
        }

        private Hint GetOrCreateHint(Player player)
        {
            PlayerDisplay playerDisplay = PlayerDisplay.Get(player);

            if (playerDisplay.GetHint($"BST-{player.UserId}") is not Hint hint)
            {
                hint = new Hint
                {
                    Id = $"BST-{player.UserId}",
                    XCoordinate = Plugin.Instance.Config.XPos,
                    YCoordinate = Plugin.Instance.Config.YPos,
                    Alignment = Plugin.Instance.Config.Alignment,
                    YCoordinateAlign = Plugin.Instance.Config.VerticalAlign,
                    Hide = true
                };
                playerDisplay.AddHint(hint);
            }

            return hint;
        }

        private void UpdateHint(Player player, string timeToSpawn)
        {
            Hint hint = GetOrCreateHint(player);

            string text = Plugin.Instance.Config.TextFormatting;

            hint.Text = text.Replace("%timer%", timeToSpawn);
            hint.Hide = player.Role.Type != PlayerRoles.RoleTypeId.Spectator;
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
