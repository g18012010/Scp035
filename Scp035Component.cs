using LabApi.Features.Wrappers;
using MEC;
using System.Linq;
using UnityEngine;

namespace Scp035
{
    internal class Scp035Component : MonoBehaviour
    {
        public Scp035Component()
        {
            ReferenceHub referenceHub = GetComponentInParent<ReferenceHub>();

            if (referenceHub == null)
            {
                Destroy(this);
                return;
            }

            _player = Player.Dictionary[referenceHub];

            if (_player == null)
            {
                Destroy(this);
                return;
            }
        }

        private Player _player;

        private LightSourceToy _lightSourceToy;

        private Vector3 _spawnLocation;

        private void Start()
        {
            Timing.CallDelayed(1f, () => _player.MaxHealth = Plugin.Instance.Config.Health);
            Timing.CallDelayed(1f, () => _player.Health = Plugin.Instance.Config.Health);

            if (_spawnLocation == Vector3.zero)
            {
                Vector3 roomPosition = Room.Dictionary.FirstOrDefault(x => x.Key.Name == MapGeneration.RoomName.Hcz049).Value.Position;
                _player.Position = new Vector3(roomPosition.x, roomPosition.y + 1, roomPosition.z);
            }
            else
                _player.Position = _spawnLocation;

            _lightSourceToy = LightSourceToy.Create(_player.GameObject.transform);
            _lightSourceToy.Range = 1.15f;
            _lightSourceToy.Color = new Color(0.5f, 0, 0, 0.5f);
            _lightSourceToy.Intensity = 1;

            _player.DisplayName = $"({_player.Nickname}) SCP-035";

            _player.SendHint(Plugin.Instance.Config.Scp035Hint, 15f);
        }

        private void OnDestroy()
        {
            _lightSourceToy.Destroy();
            _player.DisplayName = null;
        }

        public void SetPosition(Vector3 position) => _spawnLocation = position;
    }
}
