using System;
// TODO: Figure out a better name
[Flags]
public enum HeadAxis : byte
{
    Floor = 1 << 0,
    WallLeft = 1 << 1,
    Ceiling =  1 << 2,
    WallRight = 1 << 3
}