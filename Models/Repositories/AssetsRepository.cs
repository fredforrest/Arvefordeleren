namespace Arvefordeleren.Models.Repositories;

public static class AssetsRepository
{
    public static List<Asset> Assets { get; set; } = new List<Asset>();

    public static Asset? GetAssetById(int id)
    {
        return Assets.FirstOrDefault(a => a.Id == id);
    }
}