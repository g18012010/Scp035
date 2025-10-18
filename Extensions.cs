using LabApi.Features.Wrappers;

namespace Scp035
{
    public static class Extensions
    {
        public static bool IsScp035(this Player player)
        {
            if (player.GameObject.GetComponent<Scp035Component>() == null)
                return false;

            return true;
        }
    }
}
