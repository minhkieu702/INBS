using INBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace INBS.Application.Common
{
    public static class Utils
    {
        public static async Task<(List<Color> colors, List<Occasion> occasions, List<PaintType> paintTypes, List<SkinTone> skintones)> GetPreferenceAsync()
        {
            List<Color> tempColors = [];
            List<Occasion> tempOccasions = [];
            List<PaintType> tempPaintTypes = [];
            List<SkinTone> tempSkinTones = [];

            var colorTask = Task.Run(async () =>
            {
                tempColors = await GetColors();
            });

            var occasionTask = Task.Run(async () =>
            {
                tempOccasions = await GetOccasions();
            });

            var paintTypeTask = Task.Run(async () =>
            {
                tempPaintTypes = await GetPaintTypes();
            });

            var skintoneTask = Task.Run(async () =>
            {
                tempSkinTones = await GetSkinTones();
            });

            await Task.WhenAll(colorTask, occasionTask, paintTypeTask, skintoneTask);

            return (tempColors, tempOccasions, tempPaintTypes, tempSkinTones);
        }

        public static Task<List<SkinTone>> GetSkinTones()
        {
            var skintoneJson = File.ReadAllText("./File/Skintone.json");
            return Task.FromResult(JsonSerializer.Deserialize<List<SkinTone>>(skintoneJson) ?? []);
        }

        public static Task<List<PaintType>> GetPaintTypes()
        {
            var paintTypeJson = File.ReadAllText("./File/PaintType.json");
            return Task.FromResult(JsonSerializer.Deserialize<List<PaintType>>(paintTypeJson) ?? []);
        }

        public static Task<List<Occasion>> GetOccasions()
        {
            var occasionJson = File.ReadAllText("./File/Occasion.json");
            return Task.FromResult(JsonSerializer.Deserialize<List<Occasion>>(occasionJson) ?? []);
        }

        public static Task<List<Color>> GetColors()
        {
            var colorJson = File.ReadAllText("./File/Color.json");
            return Task.FromResult(JsonSerializer.Deserialize<List<Color>>(colorJson) ?? []);
        }

        public static Task<List<Category>> GetCategories()
        {
            var categoryJson = File.ReadAllText("./File/ServiceCategory.json");
            return Task.FromResult(JsonSerializer.Deserialize<List<Category>>(categoryJson) ?? []);            
        }
    }
}
