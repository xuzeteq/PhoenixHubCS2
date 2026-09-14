using backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace backend.Application.Results
{
    public abstract record SubscribtionPurchaseResult
    {
        public sealed record Success(Subscribtion subscribtion) : SubscribtionPurchaseResult;
        public sealed record HaventMoney() : SubscribtionPurchaseResult;
        public sealed record Error(string message) : SubscribtionPurchaseResult;
    }
}
