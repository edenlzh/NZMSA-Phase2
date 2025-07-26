import { useQuery } from '@tanstack/react-query';
import { me, deleteMe } from '../api/user';
import { Link, useNavigate } from 'react-router-dom';
import { useState, useRef, useEffect } from 'react';
import { useAuth } from '../context/AuthContext';
import { useTranslation } from 'react-i18next';

const API  = import.meta.env.VITE_API_BASE || 'http://localhost:5129';
const DEF  = '/avatars/defaultAvatar.png';
const toAbsolute = (p: string) => (p.startsWith('/') ? API + p : p);

export default function AvatarMenu() {
  const { t }   = useTranslation();
  const nav     = useNavigate();
  const { jwt, setJwt } = useAuth();
  const [fallback, setFallback] = useState(false);
  const { data, isLoading, isError } = useQuery({
    queryKey: ['me'],
    queryFn:  me,
    enabled:  !!jwt,
  });
  const [open, setOpen] = useState(false);
  const timer = useRef<NodeJS.Timeout | null>(null);
  useEffect(() => {
    /* 组件销毁时清理定时器 */
    return () => {
      if (timer.current) clearTimeout(timer.current);
    };
  }, []);
  const logout = () => { setJwt(null); nav('/'); };
  const del = async () => {
    if (confirm(t('confirmDeleteAccount'))) {
      await deleteMe();
      logout();
    }
  };
  // 绝对地址，空字符串回退默认图
  const src = fallback
    ? toAbsolute(DEF)
    : toAbsolute(
        data?.avatarUrl?.trim() || DEF,
      );
  console.log({isError, data});

  if (!jwt || isError) return null;
  if (isLoading || !data) {
    return <div className="h-9 w-9 rounded-full bg-gray-300 animate-pulse" />;
  }

  return (
    <div className="relative" 
      onMouseEnter={() => {
        if (timer.current) clearTimeout(timer.current);
        setOpen(true);
      }}
      /* 悬停离开 → 300 ms 后关闭（给用户移动到菜单的时间） */
      onMouseLeave={() => {
        timer.current = setTimeout(() => setOpen(false), 300);
      }}>
      <img
        src={src}
        width={10}           /* 占位，无论下载与否始终 10×10 */
        height={10}
        alt='User Avatar'
        onError={() => setFallback(true)}
        onClick={() => setOpen(o => !o)}
        className="h-10 w-10 rounded-full object-cover cursor-pointer border-2 border-yellow-400"
      />

      {open && (
        <div className="absolute right-0 mt-2 w-44 bg-white dark:bg-surface-card-dark rounded shadow z-20">
          <Link  to="/profile"       className="block px-4 py-2 nav-link">{t('profile')}</Link>
          <Link  to="/my/skills"     className="block px-4 py-2 nav-link">{t('mySkills')}</Link>
          <Link  to="/my/requests"   className="block px-4 py-2 nav-link">{t('myRequests')}</Link>
          <button onClick={del}      className="w-full text-left px-4 py-2 nav-link">
            {t('deleteAccount')}
          </button>
          <button onClick={logout}   className="w-full text-left px-4 py-2 nav-link">
            {t('logout')}
          </button>
        </div>
      )}
    </div>
  );
}
