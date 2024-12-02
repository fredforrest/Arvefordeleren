using System.IO;
using System.Globalization;
using Arvefordeleren.Models;
using Arvefordeleren.Models.Repositories;

namespace Arvefordeleren.Services;

public static class CSVImporter
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

    public static void ReadTestators(Stream stream)
    {
        using (var reader = new StreamReader(stream))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<Foo>();
        }
    }
}
