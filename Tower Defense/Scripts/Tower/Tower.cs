using Godot;

namespace WyrmspireStudios;
public partial class Tower : Node2D
{
	[Export(PropertyHint.Enum, "One:0,Two:1,Three:2,Four:3")] public int TowerTier;
	
	public TowerTexture TowerTexture;
	public TowerRange TowerRange;
	public TowerShoot TowerShoot;
}
