using System.Runtime.Serialization;

namespace backend.Domain.Enums.Balance
{
    public enum BalanceTransactionTypeEnum
    {
        [EnumMember(Value = "Пополнение")]
        Пополнение,

        [EnumMember(Value = "Списание")]
        Списание,

        [EnumMember(Value = "Промокод")]
        Промокод
    }
}
