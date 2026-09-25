using System;
using System.Linq;
using BankingManagement.DTOs;
using BankingManagement.Interfaces;
using BankingManagement.Models;
using BankingManagement.Services;

namespace BankingManagement.Business
{
    public class PaymentBusiness
    {
        private readonly IGenericRepository<Account> _accountRepository;
        private readonly IGenericRepository<PaymentTransaction> _paymentRepository;
        private readonly RazorpayService _razorpayService;

        public PaymentBusiness(
            IGenericRepository<Account> accountRepository,
            IGenericRepository<PaymentTransaction> paymentRepository,
            RazorpayService razorpayService)
        {
            _accountRepository = accountRepository;
            _paymentRepository = paymentRepository;
            _razorpayService = razorpayService;
        }

        public string CreatePaymentOrder(
            int userId,
            CreatePaymentTransactionDTO paymentDTO)
        {
            if (paymentDTO.Amount <= 0)
            {
                return null;
            }

            Account account =
                _accountRepository.GetById(
                    paymentDTO.AccountId
                );

            if (account == null)
            {
                return null;
            }

            if (account.Status.ToLower() != "active")
            {
                return null;
            }

            if (account.Customer == null ||
                account.Customer.UserId != userId)
            {
                return null;
            }

            if (account.Balance < paymentDTO.Amount)
            {
                return null;
            }

            string receiptNumber =
                "PAY" +
                DateTime.Now.ToString(
                    "yyyyMMddHHmmssfff"
                );

            string orderId =
                _razorpayService.CreateOrder(
                    paymentDTO.Amount,
                    receiptNumber
                );

            return orderId;
        }

        public bool ProcessSuccessfulPayment(
            int userId,
            PaymentSuccessDTO paymentDTO)
        {
            bool signatureValid =
                _razorpayService.VerifyPaymentSignature(
                    paymentDTO.RazorpayOrderId,
                    paymentDTO.RazorpayPaymentId,
                    paymentDTO.RazorpaySignature
                );

            if (!signatureValid)
            {
                return false;
            }

            Account account =
                _accountRepository.GetById(
                    paymentDTO.AccountId
                );

            if (account == null)
            {
                return false;
            }

            if (account.Status.ToLower() != "active")
            {
                return false;
            }

            if (account.Customer == null ||
                account.Customer.UserId != userId)
            {
                return false;
            }

            if (paymentDTO.Amount <= 0)
            {
                return false;
            }

            if (account.Balance < paymentDTO.Amount)
            {
                return false;
            }

            account.Balance =
                account.Balance - paymentDTO.Amount;

            PaymentTransaction payment =
                new PaymentTransaction
                {
                    AccountId =
                        paymentDTO.AccountId,

                    Amount =
                        paymentDTO.Amount,

                    PaymentType =
                        paymentDTO.PaymentType,

                    PaymentDate =
                        DateTime.Now,

                    Status =
                        "Success",

                    ReferenceNumber =
                        "PAY" +
                        DateTime.Now.ToString(
                            "yyyyMMddHHmmssfff"
                        )
                };

            _accountRepository.Update(account);

            _paymentRepository.Add(payment);

            _paymentRepository.Save();

            return true;
        }

        public bool ProcessPaymentFlag(
            int userId,
            PaymentFlagDTO paymentDTO)
        {
            if (paymentDTO == null)
            {
                return false;
            }

            if (paymentDTO.Amount <= 0)
            {
                return false;
            }

            if (paymentDTO.PaymentFlag != "Success" &&
                paymentDTO.PaymentFlag != "Failed")
            {
                return false;
            }

            Account account =
                _accountRepository.GetById(
                    paymentDTO.AccountId
                );

            if (account == null)
            {
                return false;
            }

            if (account.Status.ToLower() != "active")
            {
                return false;
            }

            if (account.Customer == null ||
                account.Customer.UserId != userId)
            {
                return false;
            }

            string referenceNumber =
                !string.IsNullOrEmpty(
                    paymentDTO.RazorpayOrderId
                )
                ? "PAY-" +
                  paymentDTO.RazorpayOrderId
                : "PAY" +
                  DateTime.Now.ToString(
                      "yyyyMMddHHmmssfff"
                  );

            bool alreadyProcessed =
                _paymentRepository
                    .GetAll()
                    .Any(p =>
                        p.ReferenceNumber ==
                        referenceNumber);

            if (alreadyProcessed)
            {
                return false;
            }

            if (paymentDTO.PaymentFlag == "Success")
            {
                if (account.Balance < paymentDTO.Amount)
                {
                    return false;
                }

                account.Balance =
                    account.Balance -
                    paymentDTO.Amount;

                _accountRepository.Update(account);
            }

            PaymentTransaction payment =
                new PaymentTransaction
                {
                    AccountId =
                        paymentDTO.AccountId,

                    Amount =
                        paymentDTO.Amount,

                    PaymentType =
                        paymentDTO.PaymentType,

                    PaymentDate =
                        DateTime.Now,

                    Status =
                        paymentDTO.PaymentFlag,

                    ReferenceNumber =
                        referenceNumber
                };

            _paymentRepository.Add(payment);

            _paymentRepository.Save();

            return true;
        }
    }
}