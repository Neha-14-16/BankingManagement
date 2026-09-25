using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Razorpay.Api;

namespace BankingManagement.Services
{
    public class RazorpayService
    {
        private readonly RazorpayClient _client;

        public RazorpayService()
        {
            _client = new RazorpayClient(
                RazorpayConfig.KeyId,
                RazorpayConfig.KeySecret
            );
        }

        public string CreateOrder(
            decimal amount,
            string receiptNumber)
        {
            int amountInPaise =
                (int)(amount * 100);

            Dictionary<string, object> options =
                new Dictionary<string, object>
                {
                    { "amount", amountInPaise },
                    { "currency", "INR" },
                    { "receipt", receiptNumber },
                    { "payment_capture", 1 }
                };

            Order order =
                _client.Order.Create(options);

            return order["id"].ToString();
        }

        public bool VerifyPaymentSignature(
            string orderId,
            string paymentId,
            string signature)
        {
            string message =
                orderId + "|" + paymentId;

            using (HMACSHA256 hmac =
                   new HMACSHA256(
                       Encoding.UTF8.GetBytes(
                           RazorpayConfig.KeySecret
                       )))
            {
                byte[] hash =
                    hmac.ComputeHash(
                        Encoding.UTF8.GetBytes(message)
                    );

                string generatedSignature =
                    BitConverter
                        .ToString(hash)
                        .Replace("-", "")
                        .ToLowerInvariant();

                return string.Equals(
                    generatedSignature,
                    signature,
                    StringComparison.OrdinalIgnoreCase
                );
            }
        }
    }
}