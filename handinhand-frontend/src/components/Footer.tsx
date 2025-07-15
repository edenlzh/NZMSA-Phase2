import { NavLink } from 'react-router-dom';
import { useLang } from '../context/LangContext';
import { useTheme } from '../context/ThemeContext';
import { useTranslation } from 'react-i18next';

export default function Footer() {
  useLang();
  useTheme();
  const { t } = useTranslation();
  const links = [
    { to: '/', label: t('home') },
    { to: '/skills', label: t('skills') },
    { to: '/requests', label: t('requests') },
  ];
  return (
    <footer className="bg-brand-light dark:bg-brand-dark">
      <div className="page py-4 flex flex-col md:flex-row items-center justify-between gap-4">
        {/* 链接 */}
        <nav className="flex gap-8 order-2 md:order-1">
          {links.map(i => (
            <NavLink key={i.to} to={i.to} className="footer-text hover:underline dark:text-white">
              {i.label}
            </NavLink>
          ))}
        </nav>

        {/* 联系方式 + 版权 */}
        <div className="flex flex-col items-center">
          <span className="footer-text">{t('emailLabel')}：edenlzh@outlook.com</span>
          <span className="footer-text">{t('phoneLabel')}：+64&nbsp;027&nbsp;386&nbsp;6326</span>
          <span className="footer-text">{t('copyright', { year: new Date().getFullYear() })}</span>
        </div>
      </div>
    </footer>
  );
}
