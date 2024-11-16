namespace Arvefordeleren.Models;

public class Asset
{
    public int Id { get; set; }
    public AssetType Type { get; set; }
    public string? Note { get; set; } 
    
    public Asset() {}

    public Asset(AssetType type)
    {
        Type = type;
    }
}

public enum AssetType
{
    Køretøj, Bolig, Værdigenstand
}