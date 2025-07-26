import { useState } from 'react';
import { Link, NavLink } from 'react-router-dom';
import { HiMenu, HiX } from 'react-icons/hi';
import logo from '/HandInHand-logo.png';
import { useAuth } from '../context/AuthContext';
import { useTheme } from '../context/ThemeContext';
import { useLang } from '../context/LangContext';
import { useTranslation } from 'react-i18next';
import AvatarMenu from './AvatarMenu';

export default function NavBar() {
  const { jwt, logout }          = useAuth();
  const { theme, toggle: tTheme} = useTheme();
  const { lang,  toggle: tLang } = useLang();
  const { t }                    = useTranslation();

  // --- 折叠状态 ---
  const [open, setOpen] = useState(false);

  // --- 主导航 ---
  const nav = [
    { to: '/',        label: t('home')     },
    { to: '/skills',  label: t('skills')   },
    { to: '/requests',label: t('requests') },
  ];

  /* ============== 组件渲染 ============== */
  return (
    <nav className="bg-brand-light dark:bg-brand-dark shadow sticky top-0 z-50">
      {/* 顶部栏 */}
      <div className="page h-16 flex items-center justify-between gap-4">
        {/* ========== 左侧 Logo ========== */}
        <Link to="/" className="flex items-center gap-2 shrink-0">
          <img
            src={logo}
            alt="logo"
            className="h-40 w-40 object-contain"
          />

          {/* 标语：≥1024px 才显示，避免手机挤占空间 */}
          <span className="hidden md:inline ml-3 font-extrabold text-base dark:text-white whitespace-nowrap">
            Make the world a better place
          </span>
        </Link>

        {/* ========== 桌面导航 + 功能区 ========== */}
        <ul className="hidden md:flex gap-6 font-medium ml-auto">
          {nav.map(i => (
            <NavLink
              key={i.to}
              to={i.to}
              className={({isActive}) =>
                `nav-link dark:text-white ${isActive ? 'underline underline-offset-4' : ''}`
              }>
              {i.label}
            </NavLink>
          ))}
        </ul>

        <div className="hidden md:flex items-center gap-4 ml-6">
          {/* 语言 / 主题 */}
          <button onClick={tLang}  className="text-sm  dark:text-white">{lang==='zh'?'EN':'中'}</button>
          <button onClick={tTheme} className="text-lg  dark:text-white">{theme==='light'?'🌙':'☀️'}</button>

          {/* 登录区 */}
          {jwt ? (
            <>
              <Link to="/skills/new"    className="btn-primary px-3 min-w-[80px] text-center whitespace-nowrap">{t('postSkill')}</Link>
              <Link to="/requests/new"  className="btn-primary px-3 min-w-[80px] text-center whitespace-nowrap">{t('postRequest')}</Link>
              <button onClick={logout} className="underline text-sm dark:text-white min-w-[60px] whitespace-nowrap px-2">
                {t('logout')}
              </button>
            </>
          ) : (
            <Link to="/login" className="btn-primary px-4 min-w-[80px] text-center whitespace-nowrap">{t('login')}</Link>
          )}
          <AvatarMenu />
        </div>

        {/* ========== 移动端汉堡按钮 ========== */}
        <button
          onClick={() => setOpen(!open)}
          className="md:hidden p-2 rounded hover:bg-[#f1e4a6] dark:hover:bg-[#decf8d] focus:outline-none"
          aria-label="Menu"
        >
          {open ? <HiX className="w-6 h-6" /> : <HiMenu className="w-6 h-6" />}
        </button>
      </div>

      {/* ========== 折叠菜单（仅移动端） ========== */}
      <div className={`md:hidden ${open ? 'block' : 'hidden'} bg-brand-light dark:bg-brand-dark`}>
        <ul className="flex flex-col gap-4 px-6 py-4 border-t border-gray-300 dark:border-gray-600">
          {nav.map(i => (
            <NavLink
              key={i.to}
              to={i.to}
              onClick={() => setOpen(false)}   /* 点完自动收起 */
              className="nav-link text-lg dark:text-white"
            >
              {i.label}
            </NavLink>
          ))}

          <hr className="border-gray-300 dark:border-gray-600" />

          {/* 语言 / 主题 / 登录区 */}
          <div className="flex flex-col gap-4">
            <button onClick={() => {tLang(); setOpen(false);}}  className="text-left nav-link dark:text-white">
              {lang==='zh' ? 'English' : '中文'}
            </button>
            <button onClick={() => {tTheme(); setOpen(false);}} className="text-left nav-link dark:text-white">
              {theme==='light' ? t('darkMode') : t('lightMode')}
            </button>

            {jwt ? (
              <>
                <Link to="/skills/new"   onClick={() => setOpen(false)} className="btn-primary text-center">{t('postSkill')}</Link>
                <Link to="/requests/new" onClick={() => setOpen(false)} className="btn-primary text-center">{t('postRequest')}</Link>
                <button onClick={() => {logout(); setOpen(false);}} className="underline dark:text-white text-left">
                  {t('logout')}
                </button>
                <AvatarMenu />
              </>
            ) : (
              <Link to="/login" onClick={() => setOpen(false)} className="btn-primary text-center">{t('login')}</Link>
            )}
          </div>
        </ul>
      </div>
    </nav>
  );
}
