using HarmonyLib;
using PlayerStatsSystem;

namespace Scp035.Patches
{
    [HarmonyPatch(typeof(HealthStat), "ServerHeal")]
    internal static class HealthStatPatch
    {
        public static void Prefix(HealthStat __instance, ref float healAmount)
        {
            if (Plugin.IsScp035(__instance.Hub))
                healAmount = healAmount / Plugin.Instance.Config.DividedHealAmount;
        }
    }
}
