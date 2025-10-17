using MapGeneration;

namespace Scp035
{
    internal class Config
    {
        public float Health { get; set; } = 300;
        public float DividedHealAmount { get; set; } = 3;
        public float DividedRegenerationAmount { get; set; } = 3;
        public string Scp035ItemPickedUpHint { get; set; } = "<b><color=red>You picked up SCP-035.</color></b>";
        public string Scp035ItemChangedHint { get; set; } = "<b><color=red>You selected SCP-035.</color></b>";
        public string Scp035Hint { get; set; } = "<b><color=red>You are SCP-035</color></b>";
        public float Scp035HintDuration { get; set; } = 15f;
        public RoomName[] Scp035ItemSpawnRooms { get; set; } = { RoomName.Hcz049, RoomName.Hcz079 };
    }
}
