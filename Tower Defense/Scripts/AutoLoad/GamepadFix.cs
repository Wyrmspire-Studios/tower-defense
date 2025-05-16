using Godot;

namespace WyrmspireStudios.AutoLoad;

public partial class GamepadFix : Node
{
	public override void _Ready()
	{
		Input.AddJoyMapping(
			"03000000373500004010000000010000,Generic X-Box pad,a:b0,b:b1,x:b2,y:b3,back:b6,guide:b8,start:b7,leftstick:b9,rightstick:b10,leftshoulder:b4,rightshoulder:b5,dpup:h0.1,dpdown:h0.4,dpleft:h0.8,dpright:h0.2,leftx:a0,lefty:a1,rightx:a3,righty:a4,lefttrigger:a2,righttrigger:a5,crc:6586,platform:Linux,",
			true
		);
	}
}
