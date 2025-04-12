using Godot;
using System;

namespace WyrmspireStudios;
public partial class InputType : Node
{
	public static bool IsKeyboardAndMouse = true;

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
