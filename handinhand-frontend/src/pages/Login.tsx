import { useState } from 'react';
import { login } from '../api/auth';
import { useAuth } from '../context/AuthContext';
import { useNavigate, useLocation, Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

export default function Login() {
  const nav = useNavigate();
  const loc = useLocation();
  const { setJwt } = useAuth();
  const { t } = useTranslation();
  const [form, set] = useState({ userName: '', password: '' });

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const { token } = await login(form);
    setJwt(token);
    nav((loc.state as any)?.from?.pathname ?? '/');
  };

  return (
    <section className="main-content flex-1 flex flex-col responsive-container py-8 items-center justify-center w-full">
      <form onSubmit={onSubmit} className="w-full max-w-lg space-y-6 card p-10 bg-white dark:bg-surface-dark rounded-2xl shadow-lg border border-gray-300 dark:border-gray-700">
        <h1 className="text-2xl font-bold dark:text-yellow-200 text-center mb-4">{t('login')}</h1>
        <hr className="border-b-2 border-gray-300 dark:border-gray-600 mb-4" />
        <input className="input" placeholder={t('userName')} required
               value={form.userName} onChange={e => set({ ...form, userName: e.target.value })} />
        <hr className="border-b-2 border-gray-300 dark:border-gray-600" />
        <input className="input" type="password" placeholder={t('password')} required
               value={form.password} onChange={e => set({ ...form, password: e.target.value })} />
        <hr className="border-b-2 border-gray-300 dark:border-gray-600" />
        <button className="btn-primary w-full mt-2">{t('login')}</button>
        <hr className="border-b-2 border-gray-300 dark:border-gray-600" />
        <p className="text-sm text-center dark:text-white mt-2">
          {t('noAccount')}{' '}
          <Link to="/register" className="underline">{t('register')}</Link>
        </p>
      </form>
    </section>
  );
}
