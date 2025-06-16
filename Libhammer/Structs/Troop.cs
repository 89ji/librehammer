using System.Collections.Generic;

namespace LibHammer.Structs;

public struct Troop
{
    public string Name{ get; set; }
    public int Movement{ get; set; }
    public int Toughness{ get; set; }
    public int ArmorSave{ get; set; }
    public int Wounds{ get; set; }
    public int Leadership{ get; set; }
    public int ObjectiveControl{ get; set; }
    public List<Weapon> Weapons{ get; set; }
    public int InvulnurableSave{ get; set; }

    public override string ToString()
    {
        return $"M: {Movement}\", T:{Toughness}, SV{ArmorSave}+, W:{Wounds}, LD:{Leadership}, OC:{ObjectiveControl}, Inv SV: {InvulnurableSave}";
    }
}