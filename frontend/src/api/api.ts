import axios, { type AxiosError, type InternalAxiosRequestConfig } from "axios";
import { authStorage } from "../utils/authStorage";

const API_BASE = import.meta.env.VITE_API_URL;

type RetryConfig = InternalAxiosRequestConfig & { _retry?: boolean };

export const api = axios.create({
    baseURL: API_BASE,
    timeout: 30000,
    headers: {
        "Content-Type": "application/json",
    },
});

let refreshPromise: Promise<string | null> | null = null;

async function refreshAccessToken(): Promise<string | null> {
    const refreshToken = authStorage.getRefreshToken();
    if (!refreshToken) {
        return null;
    }

    try {
        const { data } = await axios.post(`${API_BASE}/Auth/refresh-token`, {
            refreshToken,
        });

        const accessToken: string | undefined = data.accessToken ?? data.AccessToken;
        const nextRefresh: string | undefined = data.refreshToken ?? data.RefreshToken;

        if (!accessToken || !nextRefresh) {
            return null;
        }

        authStorage.setTokens(accessToken, nextRefresh);
        return accessToken;
    } catch {
        authStorage.clear();
        return null;
    }
}

export function refreshSession(): Promise<string | null> {
    if (!refreshPromise) {
        refreshPromise = refreshAccessToken().finally(() => {
            refreshPromise = null;
        });
    }

    return refreshPromise;
}

api.interceptors.request.use((config) => {
    const token = authStorage.getAccessToken();
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

api.interceptors.response.use(
    (response) => response,
    async (error: AxiosError) => {
        const original = error.config as RetryConfig | undefined;
        const status = error.response?.status;
        const url = original?.url ?? "";

        if (status !== 401 || !original || original._retry || url.includes("/Auth/refresh-token")) {
            return Promise.reject(error);
        }

        original._retry = true;
        const accessToken = await refreshSession();

        if (!accessToken) {
            return Promise.reject(error);
        }

        original.headers.Authorization = `Bearer ${accessToken}`;
        return api(original);
    },
);
