using System.ComponentModel;

namespace Subnautica.ExtraPowerConsumption;
public enum LightManagementMode
{
	[Description("Lights are not managed.")]
	None,
	[Description("Lights are toggled immediately upon exit.")]
	Auto,
	[Description("Lights are turned after 30 seconds of the vehicle being empty.")]
	Delayed,
	[Description("Lights are only powered off when you aren't inside and power is below 30%.")]
	LowPower
}