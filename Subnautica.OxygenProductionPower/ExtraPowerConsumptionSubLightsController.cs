using UnityEngine;

namespace Subnautica.ExtraPowerConsumption;

public class ExtraPowerConsumptionSubLightsController : MonoBehaviour
{
	private static ExtraPowerConsumptionOptions Options => ExtraPowerConsumption.Options;
	private static ExtraPowerConsumptionSaveData SaveData => ExtraPowerConsumption.SaveData;

	private SubRoot _subRoot;
	private CyclopsLightingPanel _lightingPanel;
	private CyclopsExternalCams _externalCams;
	private PrefabIdentifier _prefabId;

	private SubSaveData _instanceSaveData;

	private bool _sawPlayerLeave;

	private bool CyclopsInteriorEnableOnReturn
	{
		get => _instanceSaveData.ShouldResetFloodlights;
		set => _instanceSaveData.ShouldResetFloodlights = value;
	}

	private bool CyclopsFloodlightsEnableOnReturn
	{
		get => _instanceSaveData.ShouldResetIntLights;
		set => _instanceSaveData.ShouldResetIntLights = value;
	}

	void Start()
	{
		_subRoot = GetComponent<SubRoot>();
		_prefabId = GetComponent<PrefabIdentifier>();
		_lightingPanel = GetComponentInChildren<CyclopsLightingPanel>();
		_externalCams = GetComponentInChildren<CyclopsExternalCams>();
		_instanceSaveData = SaveData.SubsData.GetOrAddNew(_prefabId.Id);
	}

	void Update()
	{
		if (_subRoot.playerInside == false && _sawPlayerLeave == false)
		{
			_sawPlayerLeave = true;

			if (Options.AutoCyclopsInteriorLights.Value && _lightingPanel.lightingOn)
			{
				CyclopsInteriorEnableOnReturn = true;
				_lightingPanel.ToggleInternalLighting();
			}

			if (Options.AutoCyclopsFloodlights.Value && _lightingPanel.floodlightsOn)
			{
				CyclopsFloodlightsEnableOnReturn = true;
				_lightingPanel.ToggleFloodlights();
			}
		}

		if (_subRoot.playerInside && _sawPlayerLeave)
		{
			_sawPlayerLeave = false;

			if (CyclopsInteriorEnableOnReturn && _lightingPanel.lightingOn == false)
			{
				_lightingPanel.ToggleInternalLighting();
			}
			CyclopsInteriorEnableOnReturn = false;

			if (CyclopsFloodlightsEnableOnReturn && _lightingPanel.floodlightsOn == false)
			{
				_lightingPanel.ToggleFloodlights();
			}
			CyclopsFloodlightsEnableOnReturn = false;
		}

		if (_subRoot.powerRelay.GetPowerStatus() == PowerSystem.Status.Offline)
		{
			return;
		}

		float powerToUse = 0;

		if (_lightingPanel.lightingOn)
		{
			powerToUse += Options.CyclopsInteriorLightsUse.Value * DayNightCycle.main.deltaTime;
		}

		if (_lightingPanel.floodlightsOn)
		{
			powerToUse += Options.CyclopsFloodLightsUse.Value * DayNightCycle.main.deltaTime;
		}

		if (_externalCams.active && CyclopsExternalCams.lightState > 0)
		{
			powerToUse += Options.CyclopsCamLightsUse.Value * DayNightCycle.main.deltaTime / CyclopsExternalCams.lightState;
		}

		if (powerToUse > 0)
		{
			_subRoot.powerRelay.ModifyPower(-powerToUse, out _);
		}
	}
}