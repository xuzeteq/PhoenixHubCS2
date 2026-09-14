import { api } from "./api";

export const wheelApi = {
    spinWheel: async () => {
        const res = await api.post('/Wheel/spin');
        return res.data;
    }
}