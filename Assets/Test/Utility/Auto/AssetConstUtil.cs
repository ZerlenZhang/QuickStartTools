namespace Test.Utility
{
	/// <summary>
	/// 这个类提供了Resources下文件名和路径字典访问方式，同名资源可能引起bug
	/// </summary>
	public class AssetConstUtil : ReadyGamerOne.MemorySystem.AssetConstUtil<AssetConstUtil>
	{
		private System.Collections.Generic.Dictionary<string,string> nameToPath 
			= new System.Collections.Generic.Dictionary<string,string>{
					{ @"Bursts_1" , @"Audio\EngineerSoundEffects\Bursts_1" },
					{ @"Bursts_3" , @"Audio\EngineerSoundEffects\Bursts_3" },
					{ @"Bursts_5" , @"Audio\EngineerSoundEffects\Bursts_5" },
					{ @"Bursts_7" , @"Audio\EngineerSoundEffects\Bursts_7" },
					{ @"Explosion_1" , @"Audio\EngineerSoundEffects\Explosion_1" },
					{ @"Explosion_3" , @"Audio\EngineerSoundEffects\Explosion_3" },
					{ @"Explosion_5" , @"Audio\EngineerSoundEffects\Explosion_5" },
					{ @"Explosion_7" , @"Audio\EngineerSoundEffects\Explosion_7" },
					{ @"Footsteps_Land" , @"Audio\EngineerSoundEffects\Footsteps_Land" },
					{ @"Footsteps_Stone" , @"Audio\EngineerSoundEffects\Footsteps_Stone" },
					{ @"Fort_Bullets" , @"Audio\EngineerSoundEffects\Fort_Bullets" },
					{ @"Fort_Setting" , @"Audio\EngineerSoundEffects\Fort_Setting" },
					{ @"Jumping_Land" , @"Audio\EngineerSoundEffects\Jumping_Land" },
					{ @"Landing" , @"Audio\EngineerSoundEffects\Landing" },
					{ @"Mines_Explosion" , @"Audio\EngineerSoundEffects\Mines_Explosion" },
					{ @"Mines_Setting" , @"Audio\EngineerSoundEffects\Mines_Setting" },
					{ @"Mine_Fully_Charged" , @"Audio\EngineerSoundEffects\Mine_Fully_Charged" },
					{ @"Running" , @"Audio\EngineerSoundEffects\Running" },
					{ @"Shields_Lasting" , @"Audio\EngineerSoundEffects\Shields_Lasting" },
					{ @"Shields_Setting" , @"Audio\EngineerSoundEffects\Shields_Setting" },
					{ @"OnClicking" , @"Audio\InterfaceSoundEffects\OnClicking" },
					{ @"OnMoving" , @"Audio\InterfaceSoundEffects\OnMoving" },
					{ @"LevelUp" , @"Audio\SceneSoundEffect\LevelUp" },
					{ @"MainMenu" , @"Audio\SceneSoundEffect\MainMenu" },
					{ @"SceneChange" , @"Audio\SceneSoundEffect\SceneChange" },
					{ @"Scene_1" , @"Audio\SceneSoundEffect\Scene_1" },
					{ @"TransportActivation" , @"Audio\SceneSoundEffect\TransportActivation" },
					{ @"TransportTransmission" , @"Audio\SceneSoundEffect\TransportTransmission" },
					{ @"Archer" , @"Audio\SlectionInterfaceSoundEffects\Archer" },
					{ @"Captain" , @"Audio\SlectionInterfaceSoundEffects\Captain" },
					{ @"Craftsman" , @"Audio\SlectionInterfaceSoundEffects\Craftsman" },
					{ @"Engineer" , @"Audio\SlectionInterfaceSoundEffects\Engineer" },
					{ @"Loader" , @"Audio\SlectionInterfaceSoundEffects\Loader" },
					{ @"Mercenaries" , @"Audio\SlectionInterfaceSoundEffects\Mercenaries" },
					{ @"MUL_T" , @"Audio\SlectionInterfaceSoundEffects\MUL_T" },
					{ @"PoisonDog" , @"Audio\SlectionInterfaceSoundEffects\PoisonDog" },
					{ @"BossData" , @"File\BossData" },
					{ @"CharacterData" , @"File\CharacterData" },
					{ @"EliteData" , @"File\EliteData" },
					{ @"EnemyData" , @"File\EnemyData" },
					{ @"EngineerRankData" , @"File\EngineerRankData" },
					{ @"ItemData" , @"File\ItemData" },
					{ @"PlaneData" , @"File\PlaneData" },
					{ @"RobotRankData" , @"File\RobotRankData" },
					{ @"SkillData" , @"File\SkillData" },
					{ @"SummonData" , @"File\SummonData" },
					{ @"TestImage" , @"Prefab\TestFolder\TestImage" },
					{ @"BeetleBody" , @"Texture\BeetleBody" },
					{ @"Boss" , @"Texture\Boss" },
				};
		public override System.Collections.Generic.Dictionary<string,string> NameToPath => nameToPath;
	}
}
