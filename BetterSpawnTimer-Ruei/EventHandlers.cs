using Exiled.API.Enums;
using Exiled.API.Features.Waves;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Extensions;
using LabApi.Features.Wrappers;
using MEC;
using RueI.API;
using RueI.API.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterSpawnTimer_Ruei
{
    public class EventHandlers
    {
        private DynamicElement _element;
        private Tag _tag = new Tag("BetterSpawnTimer");

        public void OnWaitingForPlayers()
        {
            _element = new DynamicElement(Plugin.Instance.Config.Postion, () => GetText())
            {
                UpdateInterval = TimeSpan.FromSeconds(1f)
            };
        }

        public void OnChangedRole(PlayerChangedRoleEventArgs ev)
        {
            if (ev.NewRole.RoleTypeId == PlayerRoles.RoleTypeId.Spectator)
                RueDisplay.Get(ev.Player).SetVisible(_tag, true);
            else
                RueDisplay.Get(ev.Player).SetVisible(_tag, false);
        }

        public void OnVerified(VerifiedEventArgs ev)
        {
            RueDisplay display = RueDisplay.Get(ev.Player);
            display.Show(_tag, _element);
            bool isSpec = ev.Player.Role == PlayerRoles.RoleTypeId.Spectator;
            display.SetVisible(_tag, isSpec);
        }

        private static string GetText()
        {
            string text = Plugin.Instance.Config.TextFormatting;
            string timer = TimeToSpawn();

            return text.Replace("%timer%", timer);
        }

        private static string TimeToSpawn()
        {
            if (Exiled.API.Features.Respawn.IsSpawning)
            {
                switch (Exiled.API.Features.Respawn.NextKnownSpawnableFaction)
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

        private static string FormatTimer(WaveTimer wave, string color, string name) =>
                wave.TimeLeft < TimeSpan.FromSeconds(1) ? name : $"<color={color}>{wave.TimeLeft.Minutes}:{wave.TimeLeft.Seconds:00}</color>";

        private static readonly Dictionary<string, SpawnableFaction> WaveFactionMap =
            new Dictionary<string, SpawnableFaction>
            {
                { "NtfSpawnWave", SpawnableFaction.NtfWave },
                { "ChaosSpawnWave", SpawnableFaction.ChaosWave },
                { "NtfMiniWave", SpawnableFaction.NtfMiniWave },
                { "ChaosMiniWave", SpawnableFaction.ChaosMiniWave }
            };

        private static int GetTokens(string name)
        {
            if (WaveFactionMap.TryGetValue(name, out SpawnableFaction faction))
            {
                if (Exiled.API.Features.Respawn.TryGetTokens(faction, out int tokens))
                    return tokens;
            }

            return 0;
        }
    }
}
