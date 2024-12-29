using System.Collections.Generic;
using Nautilus.Json;

namespace Subnautica.ExtraPowerConsumption;

public class ExtraPowerConsumptionSaveData : SaveDataCache
{
	public Dictionary<string, SubSaveData> SubsData = new();
	public Dictionary<string, VehicleSaveData> VehiclesData = new();
}

public class SubSaveData
{
	public bool ShouldResetFloodlights;
	public bool ShouldResetIntLights;
	//public bool SpecialLightsShouldBeOn;
}

public class VehicleSaveData
{
	public bool ShouldResetLights;
}