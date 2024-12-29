using HarmonyLib;
using System;
using UnityEngine.SceneManagement;
using static LightingController;
using Object = UnityEngine.Object;

namespace Subnautica.ExtraPowerConsumption;

public static class LightsConsumption
{
	internal static void RegisterHooks()
	{
		ExtraPowerConsumption.Options.SeaMothLightsUse.SettingChanged += UpdateSeamothLightPowerUse;
		Harmony.CreateAndPatchAll(typeof(Hooks));
	}

	private static void UpdateSeamothLightPowerUse(object sender, EventArgs args)
	{
		var seamMoths = Object.FindObjectsOfType<SeaMoth>();

		foreach (var seamMoth in seamMoths)
		{
			seamMoth.toggleLights.energyPerSecond = ExtraPowerConsumption.Options.SeaMothLightsUse.Value;
		}
	}

	private static class Hooks
	{
		[HarmonyPatch(typeof(Vehicle), nameof(Vehicle.Start))]
		[HarmonyPostfix]
		private static void VehicleLightsDrainSet(ref Vehicle __instance)
		{
			if (__instance is SeaMoth moth)
			{
				moth.toggleLights.energyPerSecond = ExtraPowerConsumption.Options.SeaMothLightsUse.Value;

				if (moth.gameObject.GetComponent<ExtraPowerConsumptionSeaMothLightsController>() == null)
				{
					moth.gameObject.AddComponent<ExtraPowerConsumptionSeaMothLightsController>();
				}
			}

			if (__instance is Exosuit exoSuit)
			{
				//Todo
			}
		}

		[HarmonyPatch(typeof(SubRoot), nameof(SubRoot.Start))]
		[HarmonyPostfix]
		private static void SwapEmissiveController(ref SubRoot __instance)
		{
			if (__instance.gameObject.scene.name.Equals("Main") == false)
			{
				return;
			}
			if (__instance.isBase)
			{
				return;
			}

			if (__instance.gameObject.GetComponent<ExtraPowerConsumptionSubLightsController>() == null)
			{
				__instance.gameObject.AddComponent<ExtraPowerConsumptionSubLightsController>();
			}

			ExtraPowerConsumptionSubSpecialLights.SetupSpecialLights(__instance);
		}
	}
}