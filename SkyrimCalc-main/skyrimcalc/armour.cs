using System;
using System.Collections.Generic;
					
public class Program
{
	public static void Main()
	{
		/******************** User Input ********************/
		
		// Character Stats
		double smithingSkill = 	100,	// Set to 0 for non-tempered quality
			smithingEnchant = 	0,		// Fortify smithing enchantments
			smithingPotion = 	0,		// Fortify smithing potions
			armorSkill = 		100,	// Skill level for armor type to be calculated (Heavy or Light)
			skillEffect = 		0,		// Fortify Light/Heavy Armor enchantments/potions, Ancient Knowledge
			armorEffect = 		0,		// Bonus armor rating from flesh spells or The Lord Stone
			armorPerk = 		1;		// Agile Defender or Juggernaut as decimal (i.e. 0.8 for 80%)
		bool hasSmithingPerk = 	true,	// Player has smithing perk for armor type to be calculated (Orcish, Glass, etc.)
			hasUnisonPerk = 	true, 	// Player has Custom Fit or Well Fitted
			hasMatchPerk = 		true;	// Player has Matching Set perk
		
		// Base Armor Ratings (found here: http://en.uesp.net/wiki/Skyrim:Armor#Light_Armor)
		int chestArmorRating = 		41,
			bootsArmorRating = 		12,
			gauntletsArmorRating = 	12,
			helmetArmorRating = 	17,
			shieldArmorRating = 	29;	// set to 0 if no shield
		
		/****************************************************/
		
		ArmorCalculator armorCalculator = 
			new ArmorCalculator(smithingSkill, smithingEnchant, smithingPotion, 
								armorSkill, skillEffect, armorEffect, armorPerk, 
								hasSmithingPerk, hasUnisonPerk, hasMatchPerk);
		
		List<int> armorRatings = new List<int>();
		
		if (chestArmorRating > 0)
		{
			armorRatings.Add(chestArmorRating);
		}
		
		if (bootsArmorRating > 0)
		{
			armorRatings.Add(bootsArmorRating);
		}
		
		if (gauntletsArmorRating > 0)
		{
			armorRatings.Add(gauntletsArmorRating);
		}
		
		if (helmetArmorRating > 0)
		{
			armorRatings.Add(helmetArmorRating);
		}
		
		if (shieldArmorRating > 0)
		{
			armorRatings.Add(shieldArmorRating);
		}
		
		armorCalculator.RunCalculations(armorRatings, shieldArmorRating > 0);
	}
}

public class ArmorCalculator
{
	private double smithingSkill;
	private double smithingEnchant;
	private double smithingPotion;
	private double armorSkill;
	private double skillEffect;
	private double armorEffect;
	private double armorPerk;
	private bool hasSmithingPerk;
	private bool hasUnisonPerk;
	private bool hasMatchPerk;
	
	public ArmorCalculator(double smithingSkill, double smithingEnchant, double smithingPotion, 
						   double armorSkill, double skillEffect, double armorEffect, double armorPerk, 
						   bool hasSmithingPerk, bool hasUnisonPerk, bool hasMatchPerk)
	{
		this.smithingSkill = smithingSkill;
		this.smithingEnchant = smithingEnchant;
		this.smithingPotion = smithingPotion;
		this.armorSkill = armorSkill;
		this.skillEffect = skillEffect;
		this.armorEffect = armorEffect;
		this.armorPerk = armorPerk;
		this.hasSmithingPerk = hasSmithingPerk;
		this.hasUnisonPerk = hasUnisonPerk;
		this.hasMatchPerk = hasMatchPerk;
	}
	
	public void RunCalculations(List<int> armorRatings, bool shieldIncluded)
	{
		double displayedArmorRating = 0;
		double smithingQuality = GetSmithingQuality();
				
		for (int i = 0; i < armorRatings.Count; i++)
		{
			double baseArmorRating = armorRatings[i];
			double itemQuality = i > 0 ? Math.Ceiling(smithingQuality/2) : Math.Ceiling(smithingQuality);
						
			double armorRating =
				Math.Ceiling
				(
					(baseArmorRating + itemQuality) * (1 + 0.4 * (armorSkill + skillEffect)/100)
				)
				* (1 + (hasUnisonPerk ? .25 : 0))
				* (1 + (hasMatchPerk ? .25 : 0));
			
			if (i < armorRatings.Count - 1 || !shieldIncluded)
			{
				armorRating *= (1 + armorPerk);
			}
			
			displayedArmorRating += armorRating;
		}
		
		displayedArmorRating += armorEffect;
		double damageReduction = displayedArmorRating * 0.12 + 3 * armorRatings.Count;
		
		Console.WriteLine("Armor Rating:\t\t" + Math.Round(displayedArmorRating, 2));
		Console.WriteLine("Damage Reduction:\t" + Math.Round(damageReduction, 2) + "%");
		
		if (damageReduction > 80) 
		{ 
			Console.Write("CAPPED"); 
		}
		else 
		{ 
			Console.Write("NOT CAPPED"); 
		}
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
		
		return 3.6 * qualityLevel - 1.6;
	}
}