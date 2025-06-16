using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using LibHammer.Structs;

namespace LibHammer.Serialization;

static class Serializer
{
    static readonly string weaponPath = "weapon_list.json";
    static readonly string troopPath = "troop_list.json";
    static readonly string armyPath = "army_list.json";

    public static Dictionary<string, Weapon> LoadWeapons()
    {
        if (!File.Exists(weaponPath))
        {
            Dictionary<string, Weapon> template = new();
            File.CreateText(weaponPath).Write(JsonSerializer.Serialize(template));
        }
        string loaded = File.ReadAllText(weaponPath);
        return JsonSerializer.Deserialize<Dictionary<string, Weapon>>(loaded);
    }

    public static void SaveWeapons(Dictionary<string, Weapon> weaponsDict)
    {
        var options = new JsonSerializerOptions();
        options.WriteIndented = true;
        string serialized = JsonSerializer.Serialize(weaponsDict, options: options);
        File.Create(weaponPath).Close();
        File.WriteAllText(weaponPath, serialized);
    }

    public static Dictionary<string, Troop> LoadTroops()
    {
        if (!File.Exists(troopPath))
        {
            Dictionary<string, Troop> template = new();
            File.CreateText(troopPath).Write(JsonSerializer.Serialize(template));
        }
        string loaded = File.ReadAllText(troopPath);
        return JsonSerializer.Deserialize<Dictionary<string, Troop>>(loaded);
    }

    public static void SaveTroops(Dictionary<string, Troop> troopDict)
    {
        var options = new JsonSerializerOptions();
        options.WriteIndented = true;
        string serialized = JsonSerializer.Serialize(troopDict, options: options);
        File.Create(troopPath).Close();
        File.WriteAllText(troopPath, serialized);
    }

    public static Dictionary<string, List<Troop>> LoadArmies()
    {
        if (!File.Exists(armyPath))
        {
            Dictionary<string, Troop> template = new();
            File.CreateText(troopPath).Write(JsonSerializer.Serialize(template));
        }
        string loaded = File.ReadAllText(armyPath);
        return JsonSerializer.Deserialize<Dictionary<string, List<Troop>>>(loaded);
    }

    public static void SaveArmies(Dictionary<string, Troop> armyDict)
    {
        var options = new JsonSerializerOptions();
        options.WriteIndented = true;
        string serialized = JsonSerializer.Serialize(armyDict, options: options);
        File.Create(armyPath).Close();
        File.WriteAllText(armyPath, serialized);
    }
}