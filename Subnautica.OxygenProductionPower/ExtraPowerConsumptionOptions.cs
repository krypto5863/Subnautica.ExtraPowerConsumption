using BepInEx.Configuration;
using Nautilus.Handlers;
using Nautilus.Options;

namespace Subnautica.ExtraPowerConsumption;

public class ExtraPowerConsumptionOptions : ModOptions
{
	public readonly ConfigEntry<float> PowerUseMultiplier;
	public readonly ConfigEntry<float> SeaMothLightsUse;
	public readonly ConfigEntry<float> CyclopsInteriorLightsUse;
	public readonly ConfigEntry<float> CyclopsFloodLightsUse;
	public readonly ConfigEntry<float> CyclopsCamLightsUse;

	public readonly ConfigEntry<bool> LowPowerCyclopsInteriorLights;
	public readonly ConfigEntry<bool> AutoCyclopsInteriorLights;
	public readonly ConfigEntry<bool> AutoCyclopsFloodlights;

	public readonly ConfigEntry<bool> SeaMothLightMode;

	public static ExtraPowerConsumptionOptions Main;

	public ExtraPowerConsumptionOptions(ConfigFile config) : base("OxygenConsumption Production Power")
	{
		Main = this;

		OptionsPanelHandler.RegisterModOptions(this);

		PowerUseMultiplier = config.Bind(
			section: "Vehicles",
			key: "Oxygen Power",
			configDescription: new ConfigDescription("This setting controls how much power is used in vehicles every oxygen unit. An oxygen unit is typically used every second."),
			defaultValue: 0.025f
		);

		AddItem(PowerUseMultiplier.ToModSliderOption(0.0001f, 0.2f, 0.0005f, floatFormat: "{0:F4}"));

		SeaMothLightsUse = config.Bind(
			section: "Vehicles",
			key: "SeaMoth Lights",
			configDescription: new ConfigDescription("This setting controls how much power is used per second in the SeaMoth when the headlights are on."),
			defaultValue: 0.0166f
		);

		AddItem(SeaMothLightsUse.ToModSliderOption(0.0001f, 0.2f, 0.0005f, floatFormat: "{0:F4}"));

		CyclopsInteriorLightsUse = config.Bind(
			section: "Vehicles",
			key: "Cyclops Interior Lights",
			configDescription: new ConfigDescription("This setting controls how much power is used per second in the Cyclops when the interior lights are on."),
			defaultValue: 0.0025f
		);

		AddItem(CyclopsInteriorLightsUse.ToModSliderOption(0.0001f, 0.1f, 0.0005f, floatFormat: "{0:F4}"));

		CyclopsFloodLightsUse = config.Bind(
			section: "Vehicles",
			key: "Cyclops Floodlights",
			configDescription: new ConfigDescription("This setting controls how much power is used per second in the Cyclops when the floodlights are on."),
			defaultValue: 0.02f
		);

		AddItem(CyclopsFloodLightsUse.ToModSliderOption(0.0001f, 0.2f, 0.0005f, floatFormat: "{0:F4}"));

		CyclopsCamLightsUse = config.Bind(
			section: "Vehicles",
			key: "Cyclops Cam Lights",
			configDescription: new ConfigDescription("This setting controls how much power is used per second in the Cyclops when the cam lights are on. When the lights are in their dim mode, consumption is halved."),
			defaultValue: 0.02f
		);

		AddItem(CyclopsCamLightsUse.ToModSliderOption(0.0001f, 0.2f, 0.0005f, floatFormat: "{0:F4}"));

		LowPowerCyclopsInteriorLights = config.Bind(
			section: "Vehicles",
			key: "Low Power Cyclops Interior Lights",
			configDescription: new ConfigDescription("The cyclops will automatically use red low power lighting when you turn off your lights so you can still see."),
			defaultValue: false
		);

		AddItem(LowPowerCyclopsInteriorLights.ToModToggleOption());

		AutoCyclopsInteriorLights = config.Bind(
			section: "Vehicles",
			key: "Auto Cyclops Interior Lights",
			configDescription: new ConfigDescription("The cyclops will automatically turn off it's interior lights when you leave, and back on when you return if they were left on."),
			defaultValue: false
		);

		AddItem(AutoCyclopsInteriorLights.ToModToggleOption());

		AutoCyclopsFloodlights = config.Bind(
			section: "Vehicles",
			key: "Auto Cyclops Flood Lights",
			configDescription: new ConfigDescription("The cyclops will automatically turn off it's floodlights when you leave, and back on when you return if they were left on."),
			defaultValue: false
		);

		AddItem(AutoCyclopsFloodlights.ToModToggleOption());

		SeaMothLightMode = config.Bind(
			section: "Vehicles",
			key: "SeaMoth Lights Mode",
			configDescription: new ConfigDescription("The SeaMoth will automatically turn off it's lights when you leave, and back on when you return if they were left on."),
			defaultValue: false
		);

		AddItem(SeaMothLightMode.ToModToggleOption());
	}
}