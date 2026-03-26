using Exiled.API.Features;
using RueI;
using System;

namespace BetterSpawnTimer_Ruei
{
    public class Plugin : Plugin<Config>
    {
        public override Version RequiredExiledVersion => new Version(9, 9, 1);
        public override Version Version => new Version(1, 0, 3);
        public override string Author => "minect86";
        public override string Name => "BetterSpawnTimerRuei";

        public static Plugin Instance;
        private EventHandlers _eventHandler;

        public override void OnEnabled()
        {
            Instance = this;
            RegisterEvents();
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            UnregisterEvents();
            Instance = null;
            base.OnDisabled();
        }

        private void RegisterEvents()
        {
            _eventHandler = new EventHandlers();
            Exiled.Events.Handlers.Server.WaitingForPlayers += _eventHandler.OnWaitingForPlayers;
            LabApi.Events.Handlers.PlayerEvents.ChangedRole += _eventHandler.OnChangedRole;
            Exiled.Events.Handlers.Player.Verified += _eventHandler.OnVerified;
        }
        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= _eventHandler.OnWaitingForPlayers;
            LabApi.Events.Handlers.PlayerEvents.ChangedRole -= _eventHandler.OnChangedRole;
            Exiled.Events.Handlers.Player.Verified -= _eventHandler.OnVerified;
            _eventHandler = null;
        }
    }
}
