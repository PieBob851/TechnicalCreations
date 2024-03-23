using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TechnicalCreations.Items.Placeable
{
	public class ExampleDresser : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.ExampleDresser>());

			Item.width = 26;
			Item.height = 22;
			Item.value = 500;
		}

		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.DirtBlock)
				.Register();
		}
	}
}