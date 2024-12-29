using UnityEngine;

namespace Subnautica.ExtraPowerConsumption;

public class ExtraPowerConsumptionSeaMothLightsController : MonoBehaviour
{
	private static ExtraPowerConsumptionOptions Options => ExtraPowerConsumption.Options;
	private static ExtraPowerConsumptionSaveData SaveData => ExtraPowerConsumption.SaveData;

	private SeaMoth _seaMoth;
	private PrefabIdentifier _prefabId;

	private VehicleSaveData _instanceSaveData;

	private bool _sawPlayerLeave;

	private bool EnableLightsOnReturn
	{
		get => _instanceSaveData.ShouldResetLights;
		set => _instanceSaveData.ShouldResetLights = value;
	}

	void Awake()
	{
		_seaMoth = GetComponent<SeaMoth>();
		_prefabId = GetComponent<PrefabIdentifier>();

		_instanceSaveData = SaveData.VehiclesData.GetOrAddNew(_prefabId.id);
	}

	void Update()
	{
		if (_sawPlayerLeave == false && _seaMoth.GetPilotingMode() == false)
		{
			if (Options.SeaMothLightMode.Value && _seaMoth.toggleLights.lightsActive)
			{
				_sawPlayerLeave = true;
				EnableLightsOnReturn = true;
				_seaMoth.toggleLights.SetLightsActive(false);
			}
		}

		if (_sawPlayerLeave && _seaMoth.playerFullyEntered && _seaMoth.GetPilotingMode())
		{
			_sawPlayerLeave = false;

			if (EnableLightsOnReturn && _seaMoth.toggleLights.lightsActive == false)
			{
				_seaMoth.toggleLights.SetLightsActive(true);
			}
			EnableLightsOnReturn = false;
		}
	}
}