import type { Promocode, PromocodeActivationResponse } from "../types/Promocode";
import { api } from "./api";

export const promocodeApi = {
    getAllPromocodes: async (): Promise<Promocode[]> =>
        api.get('/Promocode').then(res => res.data),

    getByIdPromocode: async (id: number): Promise<Promocode> =>
        api.get(`/Promocode/${id}`).then(res => res.data),

    activatePromocode: async (code: string): Promise<PromocodeActivationResponse> => 
        api.post('/Promocode/activate', { code }).then(res => res.data)
}