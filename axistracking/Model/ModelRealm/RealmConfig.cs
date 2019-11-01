using System;
using Realms;

namespace axistracking.Model.ModelRealm
{
	public class RealmConfig
	{
		public static RealmConfiguration BuildDefaultConfig()
		{
			RealmConfiguration _realmConfig = new RealmConfiguration();
			_realmConfig.ShouldDeleteIfMigrationNeeded = true;
			_realmConfig.SchemaVersion = 1;
			return _realmConfig;
		}
	}
}
