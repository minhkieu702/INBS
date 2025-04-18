using AutoMapper;
using AutoMapper.QueryableExtensions;
using INBS.Application.Common;
using INBS.Application.DTOs.Customer;
using INBS.Application.DTOs.Preference;
using INBS.Application.Interfaces;
using INBS.Application.IServices;
using INBS.Domain.Entities;
using INBS.Domain.Enums;
using INBS.Domain.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace INBS.Application.Services
{
    public class CustomerService(IUnitOfWork _unitOfWork, IMapper _mapper, IAuthentication _authentication, IHttpContextAccessor _contextAccessor) : ICustomerService
    {
        public async Task UpdatePreferencesAsync(PreferenceRequest request)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var id = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);

                var preferences = await _unitOfWork.PreferenceRepository.GetAsync(c => c.Where(c => c.CustomerId == id));

                if (preferences.Any()) _unitOfWork.PreferenceRepository.DeleteRange(preferences);

                _unitOfWork.PreferenceRepository.InsertRange(await Mapping(id, request));

                if (await _unitOfWork.SaveAsync() == 0) throw new Exception("This action failed");

                _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        private async Task<IList<Preference>> Mapping(Guid cusId, PreferenceRequest request)
        {
            var (colors, occasions, paintTypes, skintones) = await Utils.GetPreferenceAsync();

            var colorIds = colors.Select(c => c.ID).ToHashSet();
            var occasionIds = occasions.Select(c => c.ID).ToHashSet();
            var paintTypeIds = paintTypes.Select(c => c.ID).ToHashSet();
            var skintoneIds = skintones.Select(c => c.ID).ToHashSet();

            var preferences = new List<Preference>();

            // Hàm helper để thêm dữ liệu tránh lặp code
            void AddPreferences(IEnumerable<int> ids, PreferenceType type, HashSet<int> validIds)
            {
                preferences.AddRange(
                    ids.Distinct()
                       .Where(validIds.Contains) // Kiểm tra hợp lệ nhanh hơn
                       .Select(id => new Preference
                       {
                           CustomerId = cusId,
                           PreferenceId = id,
                           PreferenceType = (int)type
                       })
                );
            }

            // Áp dụng cho từng loại preference
            AddPreferences(request.ColorIds, PreferenceType.Color, colorIds);
            AddPreferences(request.OccasionIds, PreferenceType.Occasion, occasionIds);
            AddPreferences(request.PaintTypeIds, PreferenceType.PaintType, paintTypeIds);
            AddPreferences(request.SkintoneIds, PreferenceType.SkinTone, skintoneIds);

            return preferences;
        }

        public IQueryable<CustomerResponse> Get()
        {
            try
            {
                return _unitOfWork.CustomerRepository.Query().ProjectTo<CustomerResponse>(_mapper.ConfigurationProvider);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GetDesignRecommendation(Guid customerId, Stream imageStream)
        {
            _unitOfWork.BeginTransaction();

            try
            {
                var customer = _unitOfWork.CustomerRepository.Query()
                    .Where(c => c.ID == customerId)
                    .FirstOrDefault();

                if (customer == null)
                    throw new Exception("Customer not found");

                var pastSelections = _unitOfWork.CustomerSelectedRepository.Query()
                    .Where(s => s.CustomerID == customerId && !s.IsDeleted)
                    .Include(s => s.NailDesignServiceSelecteds)
                    .ThenInclude(ndss => ndss.NailDesignService)
                    .ThenInclude(nds => nds.NailDesign)
                    .ThenInclude(ds => ds.Design)
                    .OrderByDescending(s => s.ID)
                    .SelectMany(s => s.NailDesignServiceSelecteds)
                    .Select(ndss => ndss.NailDesignService.NailDesign.Design.Name)
                    .Distinct()
                    .Take(5)
                    .ToList();

                var currentTrends = _unitOfWork.DesignRepository.Query()
                    .Where(d => d.TrendScore > 0)
                    .OrderByDescending(d => d.TrendScore)
                    .Select(d => d.Name)
                    .Take(3)
                    .ToList();

                var skinTone = await DetectSkinToneFromImage(imageStream);
                var skinToneName = skinTone.Name;

                var season = GetCurrentSeason();
                var occasion = customer.Occasion;

                _unitOfWork.CommitTransaction();

                return await GetAIRecommendation(pastSelections, currentTrends, skinTone, season, occasion);
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        //public async Task<string> GetAIRecommendation(List<string> pastSelections, List<string> currentTrends, string skinTone, string season, string occasion)
        //{
        //    var requestBody = new
        //    {
        //        model = "meta-llama/Llama-Vision-Free",
        //        messages = new[]
        //        {
        //    new {
        //        role = "system",
        //        content = "Bạn là chuyên gia về Nail, đưa ra đề xuất phù hợp cho khách hàng dựa trên sở thích, màu da, xu hướng và dịp."
        //    },
        //    new {
        //        role = "user",
        //        content = $"Thông tin khách hàng:\n" +
        //                  $"- Màu da: {skinTone}\n" +
        //                  $"- Lựa chọn trước đây: {string.Join(", ", pastSelections)}\n" +
        //                  $"- Xu hướng hiện tại: {string.Join(", ", currentTrends)}\n" +
        //                  $"- Mùa/Dịp: {season} - {occasion}\n\n" +
        //                  $"Gợi ý thiết kế phù hợp (chỉ đưa ra mô tả ngắn gọn, tối đa 3 dòng):"
        //    }
        //},
        //        temperature = 0.8
        //    };

        //    var jsonRequest = JsonConvert.SerializeObject(requestBody);
        //    var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        //    if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
        //    {
        //        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
        //    }

        //    try
        //    {
        //        var response = await _httpClient.PostAsync(TogetherAIUrl, content);
        //        var responseBody = await response.Content.ReadAsStringAsync();

        //        var responseData = JsonConvert.DeserializeObject<dynamic>(responseBody);
        //        if (responseData?.choices?.Count > 0)
        //        {
        //            return responseData.choices[0].message.content.ToString();
        //        }
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        Console.WriteLine($"API error: {ex.Message}");
        //    }

        //    return "Không thể tạo gợi ý thiết kế.";
        //}

        private readonly string _skinTonePath = "File/Skintone.json";

        public async Task<SkinTone> DetectSkinToneFromImage(Stream imageStream)
        {
            using var bitmap = new Bitmap(imageStream);

            long totalR = 0, totalG = 0, totalB = 0;
            int count = 0;

            for (int x = 0; x < bitmap.Width; x += 10)
            {
                for (int y = 0; y < bitmap.Height; y += 10)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    totalR += pixel.R;
                    totalG += pixel.G;
                    totalB += pixel.B;
                    count++;
                }
            }

            var avgColor = ((int)(totalR / count), (int)(totalG / count), (int)(totalB / count));

            var json = await File.ReadAllTextAsync(_skinTonePath);
            var skinTones = JsonConvert.DeserializeObject<List<SkinTone>>(json);

            var bestMatch = skinTones
                .Select(st => new
                {
                    Tone = st,
                    Distance = ColorDistance(avgColor, HexToRgb(st.HexCode))
                })
                .OrderBy(x => x.Distance)
                .First().Tone;

            return bestMatch;
        }

        public static (int R, int G, int B) HexToRgb(string hex)
        {
            hex = hex.Replace("#", "");
            return (
                Convert.ToInt32(hex.Substring(0, 2), 16),
                Convert.ToInt32(hex.Substring(2, 2), 16),
                Convert.ToInt32(hex.Substring(4, 2), 16)
            );
        }

        public static double ColorDistance((int R, int G, int B) c1, (int R, int G, int B) c2)
        {
            return Math.Sqrt(Math.Pow(c1.R - c2.R, 2) + Math.Pow(c1.G - c2.G, 2) + Math.Pow(c1.B - c2.B, 2));
        }

        public class SkinTone
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string HexCode { get; set; }
        }

        public async Task<List<Occasion>> GetUpcomingOccasions()
        {
            var json = await File.ReadAllTextAsync("File/Occasion.json");
            var occasions = JsonConvert.DeserializeObject<List<Occasion>>(json);


            var today = DateTime.Today;
            var upcomingOccasions = new List<Occasion>();

            foreach (var occasion in occasions)
            {
                if (occasion.Date == null)
                {
                    upcomingOccasions.Add(occasion);
                }
                else if (occasion.Date == "variable")
                {
                    DateTime? calculatedDate = GetCalculatedDate(occasion.Name, today.Year);
                    if (calculatedDate.HasValue && calculatedDate.Value >= today)
                    {
                        upcomingOccasions.Add(occasion);
                    }
                }
                else
                {
                    var occasionDate = DateTime.ParseExact(occasion.Date, "MM-dd", CultureInfo.InvariantCulture);
                    if (occasionDate >= today)
                    {
                        upcomingOccasions.Add(occasion);
                    }
                }
            }
            return upcomingOccasions.OrderBy(o => o.Date).ToList();
        }

        public DateTime? GetCalculatedDate(string occasionName, int year)
        {
            if (occasionName == "Easter")
            {
                return GetEasterDate(year);
            }        
            return null;
        }
        public DateTime GetEasterDate(int year)
        {
            int a = year % 19;
            int b = year / 100;
            int c = year % 100;
            int d = b / 4;
            int e = b % 4;
            int f = (b + 8) / 25;
            int g = (b - f + 1) / 3;
            int h = (19 * a + b - d - g + 15) % 30;
            int i = c / 4;
            int k = c % 4;
            int l = (32 + 2 * e + 2 * i - h - k) % 7;
            int m = (a + 11 * h + 22 * l) / 451;
            int month = (h + l - 7 * m + 114) / 31;
            int day = ((h + l - 7 * m + 114) % 31) + 1;

            return new DateTime(year, month, day);
        }
        public class Occasion
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Date { get; set; }
        }
    }
}
