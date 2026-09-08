using System.Runtime.Serialization;

namespace backend.Domain.Enums.User
{
    public enum RoleEnum
    {
        [EnumMember(Value = "Владелец")]
        Владелец = 0,

        [EnumMember(Value = "Администратор")]
        Администратор,

        [EnumMember(Value = "Модератор")]
        Модератор,

        [EnumMember(Value = "Phoenix")]
        Phoenix,

        [EnumMember(Value = "Пользователь")]
        Пользователь
    }
}
