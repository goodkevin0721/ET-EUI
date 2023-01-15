using System.Collections.Generic;


namespace ET
{
	public static class RealmGateAddressHelper
	{
		public static StartSceneConfig GetGate(int zone,long account)
		{
			List<StartSceneConfig> zoneGates = StartSceneConfigCategory.Instance.Gates[zone];
			
			int n = account.GetHashCode() % zoneGates.Count;

			return zoneGates[n];
		}
	}
}
