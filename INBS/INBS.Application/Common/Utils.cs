﻿using INBS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace INBS.Application.Common
{
    public static class Utils
    {
        public static string GetHTMLForNewArtistAccount(string username, string password)
        {
            return $@"
    <!DOCTYPE html>
    <html>
    <head>
        <style>
            table {{ border-collapse: collapse; width: 100%; }}
            th, td {{ border: 1px solid black; padding: 8px; text-align: left; }}
            th {{ background-color: #f2f2f2; }}
        </style>
    </head>
    <body>
        <p>Hello,</p>
        <p>INBS sends you <b>account</b> to log in to artist portal as follows:</p>
        <table>
            <tr>
                <th>Username</th>
                <td>{username}</td>
            </tr>
            <tr>
                <th>Password</th>
                <td>{password}</td>
            </tr>
        </table>
    </body>
    </html>";
        }
        public static long GetID()
        {
            return (DateTime.UtcNow.Ticks / 100000) % 1_000_000_000_000L; // Lấy 12 chữ số cuối
        }
        public static string HashedPassword(string password)
        {
            try
            {
                var passwordHasher = new PasswordHasher<User>();

                var hashedPassword = passwordHasher.HashPassword(new User(), password);

                return hashedPassword;
            }
            catch (Exception ex)
            {
                throw new SecurityException("An error occurred while hashing the password.", ex);
            }
        }

        public static string RemoveNonAlphabetic(string input)
        {
            return Regex.Replace(input, "[^a-zA-Z]", "");
        }

        private static string RemoveDiacritics(string text)
        {
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string TransToUsername(string fullname)
        {
            var partOfName = RemoveDiacritics(fullname).ToLower().Split(" ");
            var username = partOfName[^1];
            for (int i = 0; i < partOfName.Length - 2; i++)
            {
                username += partOfName[i][0];
            }
            return username;
        }

        public static async Task<(List<Color> colors, List<Occasion> occasions, List<PaintType> paintTypes, List<Skintone> skintones)> GetPreferenceAsync()
        {
            List<Color> tempColors = [];
            List<Occasion> tempOccasions = [];
            List<PaintType> tempPaintTypes = [];
            List<Skintone> tempSkinTones = [];

            var colorTask = Task.Run(async () =>
            {
                tempColors = await GetColorsAsync();
            });

            var occasionTask = Task.Run(async () =>
            {
                tempOccasions = await GetOccasionsAsync();
            });

            var paintTypeTask = Task.Run(async () =>
            {
                tempPaintTypes = await GetPaintTypesAsync();
            });

            var skintoneTask = Task.Run(async () =>
            {
                tempSkinTones = await GetSkinTonesAsync();
            });

            await Task.WhenAll(colorTask, occasionTask, paintTypeTask, skintoneTask);

            return (tempColors, tempOccasions, tempPaintTypes, tempSkinTones);
        }

        public static Task<List<Skintone>> GetSkinTonesAsync()
        {
            var skintoneJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "File", "Skintone.json"));
            return Task.FromResult(JsonSerializer.Deserialize<List<Skintone>>(skintoneJson) ?? []);
        }

        public static Task<List<PaintType>> GetPaintTypesAsync()
        {
            var paintTypeJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "File", "PaintType.json"));
            return Task.FromResult(JsonSerializer.Deserialize<List<PaintType>>(paintTypeJson) ?? []);
        }

        public static Task<List<Occasion>> GetOccasionsAsync()
        {
            var occasionJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "File", "Occasion.json"));
            return Task.FromResult(JsonSerializer.Deserialize<List<Occasion>>(occasionJson) ?? []);
        }

        public static Task<List<Color>> GetColorsAsync()
        {
            var colorJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "File", "Color.json"));
            return Task.FromResult(JsonSerializer.Deserialize<List<Color>>(colorJson) ?? []);
        }

        public static Task<List<Category>> GetCategoriesAsync()
        {
            var categoryJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "File", "ServiceCategory.json"));
            return Task.FromResult(JsonSerializer.Deserialize<List<Category>>(categoryJson) ?? []);
        }

        public static List<Skintone> GetSkinTones()
        {
            var skintoneJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "File", "Skintone.json"));
            return JsonSerializer.Deserialize<List<Skintone>>(skintoneJson) ?? [];
        }

        public static List<PaintType> GetPaintTypes()
        {
            var paintTypeJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "File", "PaintType.json"));
            return JsonSerializer.Deserialize<List<PaintType>>(paintTypeJson) ?? [];
        }

        public static List<Occasion> GetOccasions()
        {
            var occasionJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "File", "Occasion.json"));
            return JsonSerializer.Deserialize<List<Occasion>>(occasionJson) ?? [];
        }

        public static List<Color> GetColors()
        {
            var colorJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "File", "Color.json"));
            return JsonSerializer.Deserialize<List<Color>>(colorJson) ?? [];
        }

        public static List<Category> GetCategories()
        {
            var categoryJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "File", "ServiceCategory.json"));
            return JsonSerializer.Deserialize<List<Category>>(categoryJson) ?? [];
        }
    }
}
