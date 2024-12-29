using HarmonyLib;

namespace Subnautica.ExtraPowerConsumption;

internal static class OxygenConsumption
{
	internal static void RegisterHooks()
	{
		Harmony.CreateAndPatchAll(typeof(Hooks));
	}

	private static float GetOxygenUsedASecond()
	{
		var player = Player.main;

		if (player.CanBreathe() == false)
		{
			return 0;
		}

		if (player.IsFrozenStats())
		{
			return 0;
		}

		var breathPeriod = player.GetBreathPeriod();
		var oxygenUsed = player.GetOxygenPerBreath(breathPeriod, 1);
		var oxygenPerSecond = oxygenUsed / breathPeriod * 0.1f * ExtraPowerConsumption.Options.PowerUseMultiplier.Value;
		return oxygenPerSecond;
	}

	private static class Hooks
	{
		[HarmonyPatch(typeof(Vehicle), nameof(Vehicle.ReplenishOxygen))]
		[HarmonyPostfix]
		private static void VehicleOxygenPowerDrain(ref Vehicle __instance)
		{
			if (__instance.turnedOn == false || __instance.replenishesOxygen == false || __instance.GetPilotingMode() == false)
			{
				return;
			}

			var oxygenPerSecond = GetOxygenUsedASecond();
			__instance.energyInterface.ConsumeEnergy(oxygenPerSecond * DayNightCycle.main.deltaTime);
		}

		[HarmonyPatch(typeof(SubRoot), nameof(SubRoot.Update))]
		[HarmonyPostfix]
		private static void SubOxygenPowerDrain(ref SubRoot __instance)
		{
			if (__instance.playerInside == false || __instance.powerRelay.GetPowerStatus() == PowerSystem.Status.Offline)
			{
				return;
			}

			var oxygenPerSecond = GetOxygenUsedASecond();
			__instance.powerRelay.ModifyPower(oxygenPerSecond * DayNightCycle.main.deltaTime * -1, out _);
		}
	}
}