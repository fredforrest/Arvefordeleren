using System.IO;
using System.Globalization;
using Arvefordeleren.Models;
using Arvefordeleren.Models.Repositories;

namespace Arvefordeleren.Services;

public static class CsvReader
{
    public static void ReadAssets(string content)
    {
        var lines = File.ReadAllLines(content).Skip(1);
        foreach (var line in lines)
        {
            var columns = line.Split(',');
            if (columns.Length < 4) continue;

            AssetsRepository.Assets.Add(new Asset
            {
                Id = int.Parse(columns[0]),
                Type = Enum.Parse<AssetType>(columns[1]),
                Note = string.IsNullOrWhiteSpace(columns[2]) ? null : columns[2]
            });
        }
    }

    public static void ReadHeirs(string content)
    {
        var lines = File.ReadAllLines(content).Skip(1);
        foreach (var line in lines)
        {
            var columns = line.Split(',');
            if (columns.Length < 4) continue;

            HeirsRepository.Heirs.Add(new Heir
            {
                Id = int.Parse(columns[0]),
                Name = columns[1],
                Relation = Enum.Parse<RelationType>(columns[2])
            });
        }
    }

    public static void ReadTestators(string content)
    {
        var lines = content.Split('\n').Skip(1);
        foreach (var line in lines)
        {
            var columns = line.Split(',');
            if (columns.Length < 5) continue;

            TestatorRepository.testators.Add(new Testator
            {
                Id = int.Parse(columns[0]),
                Name = columns[1],
                isMarried = bool.Parse(columns[2]),
                Address = columns[3],
                Email = columns[4]
            });
        }
    }
}
