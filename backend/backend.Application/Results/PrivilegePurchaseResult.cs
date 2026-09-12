using backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace backend.Application.Results
{
    public abstract record PrivilegePurchaseResult
    {
        public sealed record Success(Privilege privilege) : PrivilegePurchaseResult;
        public sealed record HaventMoney() : PrivilegePurchaseResult;
        public sealed record AlreadyPurchased() : PrivilegePurchaseResult;
        public sealed record Error(string message) : PrivilegePurchaseResult;

    }
}
