namespace Arvefordeleren.Models.Repositories;

public static class AssetsRepository
{
    public static List<Asset> Assets { get; set; } = new List<Asset>();

    public static void AddAsset(Asset asset)
    {
        asset.Id = Assets.Count + 1;
        Assets.Add(asset);
    }
}