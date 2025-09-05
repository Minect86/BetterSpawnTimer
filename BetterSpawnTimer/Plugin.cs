using Exiled.API.Features;
using System;

namespace BetterSpawnTimer
{
    internal class Plugin : Plugin<Config>
    {
        public override Version RequiredExiledVersion => new Version(9, 8, 0);
        public override Version Version => new Version(1, 0, 0);
        public override string Author => "minect86";
        public override string Name => "BetterSpawnTimer";

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
            Exiled.Events.Handlers.Server.WaitingForPlayers += _eventHandler.OnWaintingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += _eventHandler.OnRoundStarted;
            Exiled.Events.Handlers.Server.RestartingRound += _eventHandler.OnRestartingRound;
            Exiled.Events.Handlers.Server.RoundEnded += _eventHandler.OnRoundEnded;
        }
        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= _eventHandler.OnWaintingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= _eventHandler.OnRoundStarted;
            Exiled.Events.Handlers.Server.RestartingRound -= _eventHandler.OnRestartingRound;
            Exiled.Events.Handlers.Server.RoundEnded -= _eventHandler.OnRoundEnded;
            _eventHandler = null;
        }
    }
}
