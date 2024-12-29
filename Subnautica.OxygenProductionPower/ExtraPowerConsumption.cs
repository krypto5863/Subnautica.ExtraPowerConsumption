using BepInEx;
using BepInEx.Logging;

namespace Subnautica.ExtraPowerConsumption
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	[BepInDependency(Nautilus.PluginInfo.PLUGIN_GUID)]
	public class ExtraPowerConsumption : BaseUnityPlugin
	{
		internal static ExtraPowerConsumptionSaveData SaveData;
		internal static ExtraPowerConsumptionOptions Options;
		public new static ManualLogSource Logger { get; private set; }

		private void Awake()
		{
			// set project-scoped logger instance
			Logger = base.Logger;

			SaveData = Nautilus.Handlers.SaveDataHandler.RegisterSaveDataCache<ExtraPowerConsumptionSaveData>();
			Options = new ExtraPowerConsumptionOptions(Config);
			// register harmony patches, if there are any
			OxygenConsumption.RegisterHooks();
			LightsConsumption.RegisterHooks();
		}
	}
}