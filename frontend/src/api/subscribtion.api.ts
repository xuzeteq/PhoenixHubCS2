import type { Subscribtion } from "../types/Subscribtion";
import { api } from "./api";

export const subscribtionApi = {
    getAllSubscribtions: async (): Promise<Subscribtion[]> =>
        api.get('/Subscribtion').then(res => res.data),

    purchaseSubscribtion: async () => 
        api.post('/Subscribtion/purchase').then(res => res.data)
}