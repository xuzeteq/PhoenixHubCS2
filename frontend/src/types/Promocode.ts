export interface Promocode {
    id: number,
    code: string,
    giveBalance: number,
    usedCount: number,
    maxUses: number,
    createdAt: Date,
    expireAt: Date
}

export interface PromocodeActivationResponse {
    message: string;
    giveBalance: number;
    code: string;
}