using LibHammer.Gamestate;

namespace LibHammer.Structs;

public struct Weapon
{
    public string Name { get; set; }
    public int Range { get; set; }
    public int Attack { get; set; }
    public int Skill { get; set; }
    public int Strength { get; set; }
    public int ArmorPiercing { get; set; }
    public int Damage { get; set; }

    public override string ToString()
    {
        return $"Range:{Range}\" blah";
    }
}