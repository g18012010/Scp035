using CustomPlayerEffects;
using HarmonyLib;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.Scp096Events;
using LabApi.Events.Arguments.Scp173Events;
using LabApi.Events.Arguments.Scp3114Events;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;
using MapGeneration;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scp035
{
    internal class Plugin : Plugin<Config>
    {
        public override string Name => "Scp035";
        public override string Description => "Adds SCP-035 to the game.";
        public override string Author => "g18012010";
        public override Version Version => new Version(1, 1);
        public override Version RequiredApiVersion => new Version(1, 0, 0);

        public static List<ushort> Scp035ItemSerials = new List<ushort>();

        public static Plugin Instance { get; private set; }
        private static Harmony Harmony;

        public override void Enable()
        {
            Instance = this;

            Harmony = new Harmony("SCP035");
            Harmony.PatchAll();

            ServerEvents.MapGenerated += OnMapGenerated;
            ServerEvents.RoundEndingConditionsCheck += OnRoundEndingConditionsCheck;
            ServerEvents.RoundEnding += OnRoundEnding;
            ServerEvents.RoomLightChanged += OnRoomLightChanged;

            PlayerEvents.ChangedRole += OnPlayerChangedRole;
            PlayerEvents.Hurting += OnPlayerHurting;
            PlayerEvents.Escaping += OnEscaping;
            PlayerEvents.RoomChanged += OnPlayerRoomChanged;

            PlayerEvents.PickingUpItem += OnPlayerPickingUpItem;
            PlayerEvents.ChangedItem += OnPlayerChangedItem;
            PlayerEvents.UsedItem += OnPlayerUsedItem;
            PlayerEvents.DroppedItem += OnPlayerDroppedItem;

            PlayerEvents.Death += OnPlayerDeath;

            Scp173Events.AddingObserver += OnScp173AddingObserver;

            Scp096Events.AddingTarget += OnScp096AddingTarget;

            Scp3114Events.StrangleStarting += OnScp3114StrangleStarting;
        }
        public override void Disable()
        {
            Instance = null;

            Harmony.UnpatchAll("SCP035");
            Harmony = null;

            ServerEvents.MapGenerated -= OnMapGenerated;
            ServerEvents.RoundEndingConditionsCheck -= OnRoundEndingConditionsCheck;
            ServerEvents.RoundEnding -= OnRoundEnding;
            ServerEvents.RoomLightChanged -= OnRoomLightChanged;

            PlayerEvents.ChangedRole -= OnPlayerChangedRole;
            PlayerEvents.Hurting -= OnPlayerHurting;
            PlayerEvents.Escaping -= OnEscaping;
            PlayerEvents.RoomChanged -= OnPlayerRoomChanged;

            PlayerEvents.PickingUpItem -= OnPlayerPickingUpItem;
            PlayerEvents.ChangedItem -= OnPlayerChangedItem;
            PlayerEvents.UsedItem -= OnPlayerUsedItem;
            PlayerEvents.DroppedItem -= OnPlayerDroppedItem;

            PlayerEvents.Death -= OnPlayerDeath;

            Scp173Events.AddingObserver -= OnScp173AddingObserver;

            Scp096Events.AddingTarget -= OnScp096AddingTarget;

            Scp3114Events.StrangleStarting -= OnScp3114StrangleStarting;
        }

        private void OnMapGenerated(MapGeneratedEventArgs ev)
        {
            Timing.CallDelayed(5f, () =>
            {
                if(Config.SpawnProperties.StaticPosition == Vector3.zero)
                {
                    RoomName randomRoomName = Config.SpawnProperties.SpawnRooms.RandomItem();

                    CreateScp035Item(Room.List.First(x => x.Name == randomRoomName).Position + new Vector3(0, 1, 0));
                } else
                    CreateScp035Item(Config.SpawnProperties.StaticPosition);
            });
        }

        private void OnRoundEnding(RoundEndingEventArgs ev)
        {
            if(Player.List.Count(x => x.IsAlive) == 1)
            {
                if(Player.List.First(x => x.IsAlive && IsScp035(x)) != null)
                {
                    ev.LeadingTeam = RoundSummary.LeadingTeam.Anomalies;
                }
            }
        }

        private void OnRoundEndingConditionsCheck(RoundEndingConditionsCheckEventArgs ev)
        {
            if(Round.ScpTargetsAmount == 1)
            {
                Player lastAlive = Player.List.First(x => x.IsAlive && !x.IsSCP);

                if (IsScp035(lastAlive))
                    ev.CanEnd = true;
            }
            if(Player.List.Where(x => x.IsAlive).All(x => x.IsHuman) && Player.List.Where(x => x.IsAlive).Count() > 1)
            {
                if(Player.List.Any(x => x.IsAlive && IsScp035(x)))
                {
                    ev.CanEnd = false;
                }
            }
        }

        private void OnPlayerChangedRole(PlayerChangedRoleEventArgs ev)
        {
            if (ev.Player.GameObject.TryGetComponent<Scp035Component>(out Scp035Component scp035Component))
                if (scp035Component.didStart)
                    GameObject.Destroy(scp035Component);
        }

        private void OnEscaping(PlayerEscapingEventArgs ev)
        {
            if (IsScp035(ev.Player))
                ev.IsAllowed = false;
        }

        private void OnPlayerHurting(PlayerHurtingEventArgs ev)
        {
            if (ev.Attacker != null && ev.Player != null)
            {
                if (IsScp035(ev.Player))
                {
                    if (ev.Attacker.Team == PlayerRoles.Team.SCPs)
                        ev.IsAllowed = false;
                }else if (IsScp035(ev.Attacker))
                {
                    if (ev.Player.Team == PlayerRoles.Team.SCPs)
                        ev.IsAllowed = false;
                }
            }
        }

        private void OnPlayerRoomChanged(PlayerRoomChangedEventArgs ev)
        {
            if (ev.NewRoom.AllLightControllers.Any(x => !x.LightsEnabled))
                ev.Player.EnableEffect<NightVision>(255);
            else if (ev.NewRoom.AllLightControllers.Any(x => x.LightsEnabled))
                ev.Player.DisableEffect<NightVision>();
        }

        private void OnRoomLightChanged(RoomLightChangedEventArgs ev)
        {
            if (!ev.NewState)
            {
                if (ev.Room.Players.Any(x => IsScp035(x)))
                    ev.Room.Players.First(x => IsScp035(x)).EnableEffect<NightVision>(255);
            }
            else
            {
                foreach (var player in ev.Room.Players)
                    player.DisableEffect<NightVision>();
            }
        }

        private void OnScp173AddingObserver(Scp173AddingObserverEventArgs ev)
        {
            if (IsScp035(ev.Target))
                ev.IsAllowed = false;
        }

        private void OnScp096AddingTarget(Scp096AddingTargetEventArgs ev)
        {
            if (IsScp035(ev.Target))
                ev.IsAllowed = false;
        }

        private void OnScp3114StrangleStarting(Scp3114StrangleStartingEventArgs ev)
        {
            if (IsScp035(ev.Target))
                ev.IsAllowed = false;
        }

        private void OnPlayerPickingUpItem(PlayerPickingUpItemEventArgs ev)
        {
            if (Scp035ItemSerials.Contains(ev.Pickup.Serial))
                ev.Player.SendHint(Config.Scp035ItemPickedUpHint, 4f);
        }
        private void OnPlayerChangedItem(PlayerChangedItemEventArgs ev)
        {
            if (ev.NewItem == null)
                return;

            if (Scp035ItemSerials.Contains(ev.NewItem.Serial))
                ev.Player.SendHint(Config.Scp035ItemChangedHint, 4f);
        }
        private void OnPlayerUsedItem(PlayerUsedItemEventArgs ev)
        {
            if (IsScp035(ev.Player))
                return;

            if (Scp035ItemSerials.Contains(ev.UsableItem.Serial))
            {
                Scp035ItemSerials.Remove(ev.UsableItem.Serial);
                ev.Player.RemoveItem(ev.UsableItem);

                ev.Player.SetRole(RoleTypeId.Tutorial, flags: RoleSpawnFlags.None);
                ev.Player.GameObject.AddComponent<Scp035Component>().SetPosition(ev.Player.Position);
            }
        }
        private void OnPlayerDroppedItem(PlayerDroppedItemEventArgs ev)
        {
            if (ev.Pickup == null)
                return;

            if (Scp035ItemSerials.Contains(ev.Pickup.Serial))
            {
                LightSourceToy lightSourceToy = LightSourceToy.Create(ev.Pickup.Transform);
                lightSourceToy.Color = Color.red;
                lightSourceToy.Range = 0.5f;
                lightSourceToy.Intensity = 0.1f;
            }
        }

        private void OnPlayerDeath(PlayerDeathEventArgs ev)
        {
            if (!IsScp035(ev.Player))
                return;

            if(UnityEngine.Random.Range(0, 100) <= Config.Scp035ItemDropChance)
                CreateScp035Item(ev.OldPosition);
        }

        public static bool IsScp035(Player player)
        {
            if (player.GameObject.GetComponent<Scp035Component>() == null)
                return false;

            return true;
        }
        public static bool IsScp035(ReferenceHub referenceHub)
        {
            if (referenceHub.GetComponent<Scp035Component>() == null)
                return false;

            return true;
        }
        public static void CreateScp035Item(Vector3 position)
        {
            Pickup scp035Pickup = Pickup.Create(ItemType.SCP268, position);
            scp035Pickup.Spawn();

            LightSourceToy lightSourceToy = LightSourceToy.Create(scp035Pickup.Transform);
            lightSourceToy.Color = Color.red;
            lightSourceToy.Range = 0.5f;
            lightSourceToy.Intensity = 0.1f;
            Scp035ItemSerials.Add(scp035Pickup.Serial);
        }

    }
}
