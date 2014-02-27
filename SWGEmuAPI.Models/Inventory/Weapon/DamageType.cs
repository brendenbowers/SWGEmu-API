using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SWGEmuAPI.Models.Inventory.Weapon
{
    public enum WeaponDamageType : int
    {
	    NONE = 0,
	    LIGHT = 1,
	    MEDIUM = 2,
	    HEAVY = 3,
	    KINETIC = 1,
	    ENERGY = 2,
	    BLAST = 4,
	    STUN = 8,
	    LIGHTSABER = 16,
	    HEAT = 32,
	    COLD = 64,
	    ACID = 128,
	    ELECTRICITY = 256,
	    FORCE = 512,
	    /*MELEEATTACK = 0,
	    RANGEDATTACK = 1,
	    FORCEATTACK = 2,
	    TRAPATTACK = 3,
	    GRENADEATTACK = 4,*/
	    HEAVYACIDBEAMATTACK = 14,
	    HEAVYLIGHTNINGBEAMATTACK = 15,
	    HEAVYPARTICLEBEAMATTACK = 17,
	    HEAVYROCKETLAUNCHERATTACK = 18,
	    HEAVYLAUNCHERATTACK = 19,
    }
}
