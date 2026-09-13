using System;
using System.Collections.Generic;
using System.Text;

namespace backend.Domain.Constants
{
    public static class AuditActions
    {
        public const string LOGIN = "успешно авторизовался.";
        public const string REGISTERED = "впервые авторизовался.";
        public const string LOGIN_FAILED = "не авторизовался.";
        public const string LOGOUT = "вышел.";

        public const string PROMOCODE_ACTIVATED = "активировал промокод.";
        public const string PROMOCODE_FAILED = "не смог активировал промокод. Ошибка промокодов.";
        public const string PROMOCODE_EXPIRED = "не смог активировать промокод. Промокод истек.";
        public const string PROMOCODE_NOT_FOUND = "не смог активировать промокод. Промокод не найден.";
        public const string PROMOCODE_USAGE_LIMIT = "не смог активировать промокод. Промокод использован максимальное кол-во раз.";
        public const string PROMOCODE_ALREADY_USED = "попытался активировать промокод повторно.";

        public const string SUBSCRIBTION_PURCHASED = "успешно приобрел подписку Phoenix на 1 мес.";
        public const string SUBSCRIBTION_RENEWED = "успешно продлил подписку Phoenix на 1 мес.";
        public const string SUBSCRIBTION_FAILED_PURCHASE_HAVENT_BALANCE = "не смог купить подписку. Недостаточно средств.";
        public const string SUBSCRIBTION_FAILED_RENEW_HAVENT_BALANCE = "не смог продлить подписку. Недостаточно средств.";
        public const string SUBSCRIBTION_FAILED = "не смог продлить подписку. Ошибка сервиса подписок.";

        public const string PURCHASE_PRIVILEGE_FAILED_HAVENT_BALANCE = "не смог приобрести привилегию. Недостаточно средств.";
        public const string PURCHASE_PRIVILEGE_SUCCESS = "успешно приобрел привилегию.";

        public const string BALANCE_DEPOSIT = "";
        public const string BALANCE_WITHDROW = "";

        public const string WHEEL_SPIN_SUCCESS = "прокрутил колесо фортуны.";

    }
}
