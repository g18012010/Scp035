using HarmonyLib;
using InventorySystem.Items.Usables;

namespace Scp035.Patches
{
    [HarmonyPatch(typeof(UsableItem), "ServerAddRegeneration")]
    internal static class UsableItemPatch
    {
        public static void Prefix(UsableItem __instance, ref float hpMultiplier)
        {
            if (Plugin.IsScp035(__instance.Owner))
                hpMultiplier = hpMultiplier / Plugin.Instance.Config.DividedRegenerationAmount;
        }
    }
}
