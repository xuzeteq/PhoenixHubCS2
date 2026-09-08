import { api, refreshSession } from "./api";
import { authStorage } from "../utils/authStorage";
import type { User } from "../types/User";

export interface AuthTokens {
    accessToken: string;
    refreshToken: string;
}

export const authApi = {
    getLoginUrl: () => `${api.defaults.baseURL}/Auth/signin`,

    getMe: async (): Promise<User> => {
        const res = await api.get("/Auth/me");
        return res.data;
    },

    refreshTokens: async (): Promise<string | null> => {
        return refreshSession();
    },

    logout: async () => {
        const refreshToken = authStorage.getRefreshToken();
        try {
            await api.post("/Auth/logout", { refreshToken });
        } catch {
            // local logout still happens
        } finally {
            authStorage.clear();
        }
    },
};
