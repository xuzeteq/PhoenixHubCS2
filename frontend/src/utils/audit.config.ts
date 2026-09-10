export interface AuditActionInfo {
    label: string;
    color: string;
    bgColor: string;
    icon: string;
}

export const auditActions: Record<string, AuditActionInfo> = {
    REGISTERED: { label: 'Регистрация', color: 'text-blue-400', bgColor: 'bg-blue-500/10 border-blue-500/30', icon: 'user-plus' },
    LOGIN: { label: 'Вход', color: 'text-blue-400', bgColor: 'bg-blue-500/10 border-blue-500/30', icon: 'login' },
    LOGIN_FAILED: { label: 'Неудачный вход', color: 'text-red-400', bgColor: 'bg-red-500/10 border-red-500/30', icon: 'login-2' },
    LOGOUT: { label: 'Выход', color: 'text-gray-400', bgColor: 'bg-gray-500/10 border-gray-500/30', icon: 'logout' },
    PASSWORD_CHANGED: { label: 'Смена пароля', color: 'text-yellow-400', bgColor: 'bg-yellow-500/10 border-yellow-500/30', icon: 'key' },

    PROMOCODE_ACTIVATED: { label: 'Промокод активирован', color: 'text-green-400', bgColor: 'bg-green-500/10 border-green-500/30', icon: 'ticket' },
    PROMOCODE_NOT_FOUND: { label: 'Промокод не найден', color: 'text-red-400', bgColor: 'bg-red-500/10 border-red-500/30', icon: 'ticket-off' },
    PROMOCODE_EXPIRED: { label: 'Промокод истёк', color: 'text-orange-400', bgColor: 'bg-orange-500/10 border-orange-500/30', icon: 'clock-x' },
    PROMOCODE_LIMIT_REACHED: { label: 'Лимит промокода', color: 'text-orange-400', bgColor: 'bg-orange-500/10 border-orange-500/30', icon: 'alert-triangle' },
    PROMOCODE_ALREADY_USED: { label: 'Промокод уже использован', color: 'text-red-400', bgColor: 'bg-red-500/10 border-red-500/30', icon: 'ban' },

    SUBSCRIPTION_PURCHASED: { label: 'Подписка куплена', color: 'text-purple-400', bgColor: 'bg-purple-500/10 border-purple-500/30', icon: 'shopping-cart' },
    SUBSCRIPTION_RENEWED: { label: 'Подписка продлена', color: 'text-cyan-400', bgColor: 'bg-cyan-500/10 border-cyan-500/30', icon: 'refresh' },

    BALANCE_DEPOSITED: { label: 'Пополнение баланса', color: 'text-emerald-400', bgColor: 'bg-emerald-500/10 border-emerald-500/30', icon: 'coin-plus' },
    BALANCE_WITHDRAWN: { label: 'Списание с баланса', color: 'text-orange-400', bgColor: 'bg-orange-500/10 border-orange-500/30', icon: 'coin-minus' },
};

export const defaultAuditAction: AuditActionInfo = {
    label: 'Неизвестно',
    color: 'text-gray-400',
    bgColor: 'bg-gray-500/10 border-gray-500/30',
    icon: 'help-circle'
};

export const getAuditAction = (action: string): AuditActionInfo => {
    return auditActions[action] || defaultAuditAction;
};