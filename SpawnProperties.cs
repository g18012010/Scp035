using MapGeneration;
using UnityEngine;

namespace Scp035
{
    internal class SpawnProperties
    {
        public Vector3 StaticPosition { get; set; } = Vector3.zero;
        public RoomName[] SpawnRooms { get; set; }
    }
}
