using Exiled.API.Features;
using Exiled.API.Features.Waves;
using Exiled.Events.EventArgs.Server;
using HintServiceMeow.Core.Utilities;
using MEC;
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

        private void KillCoroutine()
        {
            if (_timerCoroutine.IsRunning)
                Timing.KillCoroutines(_timerCoroutine);
        }

        private IEnumerator<float> TimerEnumerator()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(0.5f);
                foreach (Player pl in Player.List)
                {
                    UpdateHint(pl);
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

        private void UpdateHint(Player player)
        {
            Hint hint = GetOrCreateHint(player);
            hint.Text = $"<b><size=50%>{TimeToSpawn()}</size></b>";
            if (player.Role.Type == PlayerRoles.RoleTypeId.Spectator)
                hint.Hide = false;
            else
                hint.Hide = true;
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
