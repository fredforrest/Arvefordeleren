namespace Arvefordeleren.Models.Repositories;

public static class AssetsRepository
{
    public static List<Asset> Assets { get; set; } = new List<Asset>();

    public static void AddAsset(Asset asset)
    {
        asset.Id = Assets.Count + 1;
        Assets.Add(asset);
    }

    public static void RemoveAsset(int id)
    {
        Asset asset = Assets.FirstOrDefault(a => a.Id == id);

        if (asset != null)
        {
            Assets.Remove(asset);
        }
    }

    public static Asset? GetAssetById(int id)
    {
        return Assets.FirstOrDefault(a => a.Id == id);
    }

    public static List<Asset> GetAssets() => new List<Asset>(Assets);
}
