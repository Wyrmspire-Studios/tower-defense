using Godot;

namespace WyrmspireStudios;
public partial class InputType : Node
{
	public static bool IsKeyboardAndMouse { get; private set; }

	public override void _Input(InputEvent ev)
	{
		IsKeyboardAndMouse = ev switch
		{
			InputEventKey or InputEventMouse => true,
			InputEventJoypadButton or InputEventJoypadMotion => false,
			_ => IsKeyboardAndMouse
		};
	}
}
