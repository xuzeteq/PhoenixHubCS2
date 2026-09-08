export interface User {
    id: number;
    steamId: string;
    username: string;
    avatarUrl: string;
    balance: number;
    role: number;
    roleName: string;
    isVerify: boolean;
    createdAt: string;
    lastLoginAt: string;
    subscribtionExpireAt: string;
}

export type RoleName = 'Владелец' | 'Администратор' | 'Модератор' | 'Phoenix' | 'Пользователь';