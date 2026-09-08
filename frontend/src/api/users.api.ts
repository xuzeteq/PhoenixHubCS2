import type { User } from "../types/User";
import { api } from "./api";

export const usersApi = {
    getAllUsers: async (): Promise<User[]> =>
        api.get('/Users').then(res => res.data),

    getProfile: async (steamId: string): Promise<User> => 
        api.get(`/Users/${steamId}`).then(res => res.data)
}