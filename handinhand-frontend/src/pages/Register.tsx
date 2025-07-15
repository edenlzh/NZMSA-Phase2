import { useState } from 'react';
import { register } from '../api/auth';
import { useAuth } from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

export default function Register() {
  const nav = useNavigate();
  const { setJwt } = useAuth();
  const { t } = useTranslation();
  const [form, set] = useState({ userName: '', email: '', password: '' });
  const [error, setError] = useState('');

  const submit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    try {
      const { token } = await register(form);
      setJwt(token);
      nav('/');
    } catch (err: any) {
      setError(err.response?.data?.message ?? t('registerFailed'));
    }
  };

  return (
    <section className="main-content flex-1 flex flex-col responsive-container py-8 items-center justify-center w-full">
      <form onSubmit={submit} className="w-full max-w-lg space-y-6 card p-10 bg-white dark:bg-surface-dark rounded-2xl shadow-lg border border-gray-300 dark:border-gray-700">
        <h1 className="text-2xl font-bold dark:text-yellow-200 text-center mb-4">{t('register')}</h1>
        <hr className="border-b-2 border-gray-300 dark:border-gray-600 mb-4" />
        <input className="input" placeholder={t('userName')} required
               value={form.userName} onChange={e => set({ ...form, userName: e.target.value })} />
        <hr className="border-b-2 border-gray-300 dark:border-gray-600" />
        <input className="input" placeholder={t('email')} required
               value={form.email} onChange={e => set({ ...form, email: e.target.value })} />
        <hr className="border-b-2 border-gray-300 dark:border-gray-600" />
        <input className="input" type="password" placeholder={t('password')} required
               value={form.password} onChange={e => set({ ...form, password: e.target.value })} />
        <hr className="border-b-2 border-gray-300 dark:border-gray-600" />
        {error && <p className="text-red-600 text-center">{error}</p>}
        <button className="btn-primary w-full mt-2">{t('registerAndLogin')}</button>
      </form>
    </section>
  );
}
