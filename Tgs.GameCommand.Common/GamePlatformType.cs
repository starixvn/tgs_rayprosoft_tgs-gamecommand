namespace Tgs.GameCommand.Common;

[Flags]
public enum GamePlatformType : byte
{
	Desktop = 1,
	Mobile = 2,
	DesktopAndMobile = Desktop | Mobile
}
