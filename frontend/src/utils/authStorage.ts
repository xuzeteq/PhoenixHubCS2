const ACCESS_TOKEN_KEY = 'access_token';
const REFRESH_TOKEN_KEY = 'refresh_token';

function migrateLegacyTokens() {
    const legacyAccess = localStorage.getItem('auth_token') ?? localStorage.getItem('jwt_token');
    if (legacyAccess && !localStorage.getItem(ACCESS_TOKEN_KEY)) {
        localStorage.setItem(ACCESS_TOKEN_KEY, legacyAccess);
    }
    localStorage.removeItem('auth_token');
    localStorage.removeItem('jwt_token');
}

migrateLegacyTokens();

export const authStorage = {
    getAccessToken: () => localStorage.getItem(ACCESS_TOKEN_KEY),
    getRefreshToken: () => localStorage.getItem(REFRESH_TOKEN_KEY),
    setTokens: (accessToken: string, refreshToken?: string | null) => {
        localStorage.setItem(ACCESS_TOKEN_KEY, accessToken);
        if (refreshToken) {
            localStorage.setItem(REFRESH_TOKEN_KEY, refreshToken);
        }
    },
    clear: () => {
        localStorage.removeItem(ACCESS_TOKEN_KEY);
        localStorage.removeItem(REFRESH_TOKEN_KEY);
        localStorage.removeItem('auth_token');
        localStorage.removeItem('jwt_token');
    },
    hasSession: () => !!localStorage.getItem(ACCESS_TOKEN_KEY) || !!localStorage.getItem(REFRESH_TOKEN_KEY),
};
