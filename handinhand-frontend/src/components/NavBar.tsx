import { Link, NavLink } from 'react-router-dom';
import logo from '/HandInHand-logo.png';
import { useAuth } from '../context/AuthContext';
import { useTheme } from '../context/ThemeContext';
import { useLang } from '../context/LangContext';
import { useTranslation } from 'react-i18next';
import AvatarMenu from './AvatarMenu';

export default function NavBar() {
  const { jwt, logout } = useAuth();
  const { theme, toggle: toggleTheme } = useTheme();
  const { lang, toggle: toggleLang } = useLang();
  const { t } = useTranslation();

  const nav = [
    { to: '/', label: t('home') },
    { to: '/skills', label: t('skills') },
    { to: '/requests', label: t('requests') },
  ];

  return (
    <nav className="bg-brand-light dark:bg-brand-dark shadow">
      <div className="page h-16 flex items-center justify-between gap-6">
        {/* logo */}
        <Link to="/" className="flex items-center gap-3 shrink-0">
          <img src={logo} alt="logo" className="h-40 w-40 object-contain" />
          <span className="font-extrabold text-lg dark:text-white">Make the world a better place</span>
        </Link>

        {/* 主导航（桌面） */}
        <ul className="hidden md:flex gap-6 font-medium w-full justify-end">
          {nav.map(i => (
            <NavLink
              key={i.to}
              to={i.to}
              className={({isActive})=>`nav-link dark:text-white ${isActive?'underline underline-offset-4':''}` }>
                {i.label}
            </NavLink>
          ))}
        </ul>

        {/* 右侧功能 */}
        <div className="hidden md:flex items-center gap-4">
          {/* 语言 / 主题 */}
          <button onClick={toggleLang} className="text-sm dark:text-white">
            {lang === 'zh' ? 'EN' : '中'}
          </button>
          <button onClick={toggleTheme} className="text-lg dark:text-white">
            {theme === 'light' ? '🌙' : '☀️'}
          </button>

          {/* 登录区 */}
          {jwt ? (
            <>
              <Link to="/skills/new" className="btn-primary min-w-[80px] text-center whitespace-nowrap px-4">{t('postSkill')}</Link>
              <Link to="/requests/new" className="btn-primary min-w-[80px] text-center whitespace-nowrap px-4">{t('postRequest')}</Link>
              <button onClick={logout} className="underline text-sm dark:text-white min-w-[60px] whitespace-nowrap px-2">{t('logout')}</button>
            </>
          ) : (
            <Link to="/login" className="btn-primary min-w-[80px] text-center whitespace-nowrap px-4">{t('login')}</Link>
          )}
        </div>
        <div className="flex items-center gap-4">
          <AvatarMenu />
        </div>
      </div>
    </nav>
  );
}
