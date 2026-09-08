import { useEffect, useRef } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { useAuth } from '../contexts/AuthProvider';

export default function AuthCallback() {
  const { login } = useAuth();
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const hasProcessed = useRef(false);

  useEffect(() => {
    if (hasProcessed.current) return;

    const token = searchParams.get('token');
    const refreshToken = searchParams.get('refreshToken');

    if (token && refreshToken) {
      hasProcessed.current = true;

      const cleanUrl = window.location.pathname;
      window.history.replaceState({}, document.title, cleanUrl);

      void login(token, refreshToken).then(() => {
        navigate('/', { replace: true });
      });
    } else {
      navigate('/login?error=no_token', { replace: true });
    }
  }, [searchParams, login, navigate]);

  return (
    <div className="flex items-center justify-center min-h-screen bg-[#0a0a0f]">
      <div className="flex flex-col items-center gap-4">
        <div className="w-12 h-12 border-4 border-blue-500 border-t-transparent rounded-full animate-spin" />
        <p className="text-white text-xl font-medium">Выполняется вход...</p>
      </div>
    </div>
  );
}
