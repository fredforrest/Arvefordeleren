namespace Arvefordeleren.Models.Repositories;

public static class AssetsRepository
{
    public static List<Asset> Assets { get; set; } = new List<Asset>();

    public static void RemoveAsset(int id)
    {
        Asset asset = Assets.FirstOrDefault(a => a.Id == id);

        if (asset != null)
        {
            Assets.Remove(asset);
        }
    }
}