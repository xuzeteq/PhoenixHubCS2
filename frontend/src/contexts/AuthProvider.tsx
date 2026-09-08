import { createContext, useContext, useState, useEffect, useCallback, type ReactNode } from "react";
import { authApi } from "../api/auth.api";
import { authStorage } from "../utils/authStorage";
import type { User } from "../types/User";

interface AuthContextType {
  user: User | null;
  token: string | null;
  isAuthenticated: boolean;
  login: (accessToken: string, refreshToken: string) => Promise<void>;
  logout: () => Promise<void>;
  refreshUser: () => Promise<boolean>;
  updateBalance: (newBalance: number) => void;
  isLoading: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [token, setToken] = useState<string | null>(() => authStorage.getAccessToken());
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState(() => authStorage.hasSession());

  const applyUser = useCallback((nextUser: User, accessToken?: string | null) => {
    setUser(nextUser);
    if (accessToken) {
      setToken(accessToken);
    }
  }, []);

  const refreshUser = useCallback(async () => {
    if (!authStorage.hasSession()) {
      setUser(null);
      setToken(null);
      return false;
    }

    try {
      const me = await authApi.getMe();
      applyUser(me, authStorage.getAccessToken());
      return true;
    } catch {
      authStorage.clear();
      setUser(null);
      setToken(null);
      return false;
    }
  }, [applyUser]);

  useEffect(() => {
    let cancelled = false;

    const bootstrap = async () => {
      if (!authStorage.hasSession()) {
        setIsLoading(false);
        return;
      }

      setIsLoading(true);
      try {
        const me = await authApi.getMe();
        if (!cancelled) {
          applyUser(me, authStorage.getAccessToken());
        }
      } catch {
        if (!cancelled) {
          authStorage.clear();
          setUser(null);
          setToken(null);
        }
      } finally {
        if (!cancelled) {
          setIsLoading(false);
        }
      }
    };

    void bootstrap();
    return () => {
      cancelled = true;
    };
  }, [applyUser]);

  const login = useCallback(async (accessToken: string, refreshToken: string) => {
    authStorage.setTokens(accessToken, refreshToken);
    setToken(accessToken);
    setIsLoading(true);

    try {
      const me = await authApi.getMe();
      applyUser(me, accessToken);
    } finally {
      setIsLoading(false);
    }
  }, [applyUser]);

  const logout = useCallback(async () => {
    await authApi.logout();
    setToken(null);
    setUser(null);
  }, []);

  const updateBalance = useCallback((newBalance: number) => {
    setUser((current) => (current ? { ...current, balance: newBalance } : current));
  }, []);

  return (
    <AuthContext.Provider
      value={{
        user,
        token,
        isAuthenticated: !!user,
        login,
        logout,
        refreshUser,
        updateBalance,
        isLoading,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}

// eslint-disable-next-line react-refresh/only-export-components
export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
}
