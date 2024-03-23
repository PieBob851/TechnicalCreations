using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TechnicalCreations.Items
{
	public class BasicSword : ModItem
	{
        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.TechnicalCreations.hjson file.

		public override void SetDefaults()
		{
			Item.width = 48;
			Item.height = 32;
			Item.value = 10000;
			Item.rare = 2;
			Item.UseSound = SoundID.Item1;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 1);
			recipe.Register();
		}
	}
}