using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace INBS.Infrastructure.Payment.PayOSIntegration
{
    public class PayOSHelpers
    {
        public static string GenerateSignature(Dictionary<string, object> parameters, string secretKey)
        {
            // 1️⃣ Sắp xếp key theo thứ tự tăng dần (alphabet)
            var sortedParams = parameters
                .Where(kv => kv.Value != null) // Loại bỏ giá trị null
                .OrderBy(kv => kv.Key)
                .ToList();

            // 2️⃣ Nối các giá trị thành một chuỗi
            string rawData = string.Join("|", sortedParams.Select(kv => kv.Value));

            // 3️⃣ Mã hóa bằng HMAC-SHA256 với `secretKey`
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                string signature = BitConverter.ToString(hash).Replace("-", "").ToLower();
                return signature;
            }
        }
    }
}
