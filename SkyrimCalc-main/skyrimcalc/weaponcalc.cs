using System;
					
public class Program
{
	public static void Main()
	{
		/******************** User Input ********************/
		
		// Character Stats
		double smithingSkill = 	0,		// Set to 0 for non-sharpened quality
			smithingEnchant = 	0,		// Fortify smithing enchantment as decimal (ie 120% = 1.2)
			smithingPotion = 	0,		// Fortify smithing potions as decimal (ie 120% = 1.2)
			weaponSkill = 		100,	// Skill level for weapon type to be calculated
			perkEffect = 		1,		// Barbarian/Armsman/Overdraw rank level as decimal (ie 80% = 0.8)
			itemEffect = 		1.6,	// Fortify weapon type enchantments as decimal (ie 120% = 1.2)
			potionEffect = 		0,		// Fortify weapon type potions as decimal (ie 120% = 1.2) (Alchemy sandbox here: http://www.garralab.com/skyrim/alchemy.php)
			seekerOfMight = 	0;		// Black Book: The Sallow Regent quest reward (0 or 0.1)
		bool hasSmithingPerk = 	false;	// Player has smithing perk (Orcish, Elven, etc.)
		
		// Base Weapon Damage (found here: http://en.uesp.net/wiki/Skyrim:Weapons#One-handed_Weapons)
		int baseDamage = 		24,
			arrowDamage = 		24;		// Set to 0 for melee weapons
		
		/****** DO NOT CHANGE ANYTHING BELOW THIS LINE ******/
		
		DamageCalculator damageCalculator = 
			new DamageCalculator(smithingSkill, smithingEnchant, smithingPotion, 
								 weaponSkill, perkEffect, itemEffect, potionEffect, 
								 seekerOfMight, hasSmithingPerk);
		
		damageCalculator.RunCalculations(baseDamage, arrowDamage);
	}
}

public class DamageCalculator
{	
	private double smithingSkill;
	private double smithingEnchant;
	private double smithingPotion;
	private double weaponSkill;
	private double perkEffect;
	private double itemEffect;
	private double potionEffect;
	private double seekerOfMight;
	private bool hasSmithingPerk;
	
	public DamageCalculator(double smithingSkill, double smithingEnchant, double smithingPotion, 
							double weaponSkill, double perkEffect, double itemEffect, 
							double potionEffect, double seekerOfMight, bool hasSmithingPerk)
	{
		this.smithingSkill = smithingSkill;
		this.smithingEnchant = smithingEnchant;
		this.smithingPotion = smithingPotion;
		this.weaponSkill = weaponSkill;
		this.perkEffect = perkEffect;
		this.itemEffect = itemEffect;
		this.potionEffect = potionEffect;
		this.seekerOfMight = seekerOfMight;
		this.hasSmithingPerk = hasSmithingPerk;
	}
	
	public void RunCalculations(int baseDamage, int arrowDamage)
	{
		double displayedWeaponDamage = Math.Floor( 
			(baseDamage + GetSmithingQuality()) 
			* (1 + weaponSkill/200) 
			* (1 + perkEffect) 
			* (1 + itemEffect) 
			* (1 + potionEffect) 
			* (1 + seekerOfMight)
			+ arrowDamage
			);
		
		Console.Write(displayedWeaponDamage);	
	}
	
	private double GetSmithingQuality()
	{	
		if (smithingSkill < 14) { return 0; }
		
		double effectiveSkill = 
			(smithingSkill - 13.29)
			* (1 + (hasSmithingPerk ? 1 : 0))
			* (1 + smithingEnchant)
			* (1 + smithingPotion)
			+ 13.29;
		
		double qualityLevel = Math.Floor((effectiveSkill + 38) * 3 / 103);
		
		double smithingQuality = 3.6 * qualityLevel - 1.6;
		
		return Math.Ceiling(smithingQuality/2);
	}
}