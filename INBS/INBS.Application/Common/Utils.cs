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

            var colorTask = Task.Run(() =>
            {
                var colorJson = File.ReadAllText("./Color.json");
                tempColors = JsonSerializer.Deserialize<List<Color>>(colorJson) ?? [];
            });

            var occasionTask = Task.Run(() =>
            {
                var occasionJson = File.ReadAllText("./Occasion.json");
                tempOccasions = JsonSerializer.Deserialize<List<Occasion>>(occasionJson) ?? [];
            });

            var paintTypeTask = Task.Run(() =>
            {
                var paintTypeJson = File.ReadAllText("./PaintType.json");
                tempPaintTypes = JsonSerializer.Deserialize<List<PaintType>>(paintTypeJson) ?? [];
            });

            var skintoneTask = Task.Run(() =>
            {
                var skintoneJson = File.ReadAllText("./SkinTone.json");
                tempSkinTones = JsonSerializer.Deserialize<List<SkinTone>>(skintoneJson) ?? [];
            });

            await Task.WhenAll(colorTask, occasionTask, paintTypeTask, skintoneTask);

            return (tempColors, tempOccasions, tempPaintTypes, tempSkinTones);
        }
    }
}
