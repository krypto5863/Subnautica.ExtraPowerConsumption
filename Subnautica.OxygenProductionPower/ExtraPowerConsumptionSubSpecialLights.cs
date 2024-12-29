using Nautilus.Extensions;
using UnityEngine;

namespace Subnautica.ExtraPowerConsumption;

internal class ExtraPowerConsumptionSubSpecialLights : MonoBehaviour
{
	private static ExtraPowerConsumptionOptions Options => ExtraPowerConsumption.Options;
	//private static ExtraPowerConsumptionSaveData SaveData => ExtraPowerConsumption.SaveData;
	internal static void SetupSpecialLights(SubRoot subRoot)
	{
		if (subRoot.GetComponentInChildren<ExtraPowerConsumptionSubSpecialLights>() != null)
		{
			return;
		}

		var floodAlarm = subRoot.gameObject.SearchChild("FloodAlarm");

		if (floodAlarm == null)
		{
			return;
		}

		var customAlarmLights = Instantiate(floodAlarm, floodAlarm.transform.parent, true);
		var subFloodAlarm = customAlarmLights.GetComponent<SubFloodAlarm>();
		Destroy(subFloodAlarm);
		customAlarmLights.gameObject.AddComponent<ExtraPowerConsumptionSubSpecialLights>();
	}

	private SubRoot _subRoot;
	private CyclopsLightingPanel _lightingPanel;
	//private PrefabIdentifier _prefabId;

	private GameObject _lightsContainer;
	private Light[] _lights;

	//private SubSaveData _instanceSaveData;

	public bool LightsShouldBeActive => _lightingPanel.lightingOn == false && Options.LowPowerCyclopsInteriorLights.Value;

	public bool LightsActive { get; private set; }

	void Start()
	{
		name = "ExtraPowerConsumptionSpecialLights";

		_subRoot = GetComponentInParent<SubRoot>();
		_lightingPanel = _subRoot.GetComponentInChildren<CyclopsLightingPanel>();
		_lightsContainer = gameObject.SearchChild("lights");
		_lights = GetComponentsInChildren<Light>();
		//_instanceSaveData = SaveData.SubsData.GetOrAddNew(_prefabId.id);

		var lightColor = new Color(0.25f, 0, 0, 1);

		foreach (Transform tform in _lightsContainer.transform)
		{
			tform.gameObject.SetActive(true);
			var light = tform.gameObject.GetComponent<Light>();
			light.color = lightColor;

			var lightAnimator = tform.gameObject.GetComponent<LightAnimator>();
			lightAnimator.enabled = false;
		}
	}

	void Update()
	{
		if (_subRoot.fireSuppressionState || _subRoot.subWarning || _subRoot.silentRunning || _subRoot.powerRelay.GetPowerStatus() == PowerSystem.Status.Offline)
		{
			if (LightsActive)
			{
				SetLightsActive(false);
			}

			return;
		}

		if (LightsActive != LightsShouldBeActive)
		{
			SetLightsActive(LightsShouldBeActive);
		}
	}

	private void SetLightsActive(bool active)
	{
		LightsActive = active;
		_lightsContainer.SetActive(active);
	}
}