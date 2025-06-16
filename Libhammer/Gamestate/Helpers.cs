using System;
using LibHammer.Structs;

namespace LibHammer.Gamestate;

static class Helpers
{
    static int CalculateDamage(Weapon weapon, BoardTroop attacker, BoardTroop defender)
    {
        // Roll to hit
        int hits = 0;
        for (int att = 0; att < weapon.Attack; att++) if (rollD6() >= weapon.Skill) hits++;

        // Roll to wound
        int wounds = 0;
        int woundBar = StrengthVsToughness(weapon.Strength, defender.Stats.Toughness);
        for (int w = 0; w < hits; w++) if (rollD6() >= woundBar) wounds++;

        // Roll to defend
        int modifiedArmorSave = defender.Stats.ArmorSave - weapon.ArmorPiercing;
        int armorSave = (modifiedArmorSave > defender.Stats.InvulnurableSave) ? modifiedArmorSave : defender.Stats.InvulnurableSave;
        int hit = 0;
        for (int a = 0; a < wounds; a++) if (rollD6() < armorSave) hit++;


        // TODO: for now, all attacks have static damage, allow for rolling later
        return hit * weapon.Damage;
    }

    // D6 roll provider, can be tapped into
    public static int rollD6(int n=1)
    {
        Random rng = new();
        int sum = 0;
        for (int roll = 0; roll < n; roll++) sum += rng.Next(1, 7);
        return sum;
    }

    // Dy roll provider, can be tapped into
    public static int rollxDy(int x, int y)
    {
        Random rng = new();
        int sum = 0;
        for (int roll = 0; roll < x; roll++) sum += rng.Next(1, y + 1);
        return sum;
    }

    public static int StrengthVsToughness(int strength, int toughness)
    {
        if (strength <= toughness / 2) return 6;
        if (strength < toughness) return 5;
        if (strength == toughness) return 4;
        if (strength >=toughness * 2) return 2;
        else return 3;
    }
}