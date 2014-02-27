using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Models.Inventory.Weapon
{
    public class WeaponInventoryItem : CharacterInventoryItem
    {
	    public int point_blank_accuracy { get; set; }
	    public int point_blank_range { get; set; }
	    public int ideal_range { get; set; }
	    public int max_range { get; set; }
	    public int ideal_accuracy { get; set; }
	    public int max_range_accuracy { get; set; }
	    public int armor_piercing { get; set; }
	    public float attack_speed { get; set; }
	    public float max_damage { get; set; }
	    public float min_damage { get; set; }
	    public float wound_ratio { get; set; }
	    public float damage_radius { get; set; }
	    public int health_attack_cost { get; set; }
	    public int action_attack_cost { get; set; }
	    public int mind_attack_cost { get; set; }
	    public int force_attack_cost { get; set; }
	    public WeaponDamageType damage_type { get; set; }
	    public string weapon_type { get; set; }
    }
}
