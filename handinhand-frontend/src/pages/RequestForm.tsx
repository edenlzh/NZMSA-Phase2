import { useState } from 'react';
import { createRequest } from '../api/helpRequest';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

export default function RequestForm() {
  const nav = useNavigate();
  const { t } = useTranslation();
  const [form, set] = useState({ title: '', description: '' });

  const submit = async (e: React.FormEvent) => {
    e.preventDefault();
    await createRequest(form);
    nav('/requests');
  };

  return (
    <section className="main-content flex-1 flex flex-col responsive-container py-8 justify-center items-center">
      <form onSubmit={submit} className="w-full max-w-sm mx-auto space-y-6 bg-white dark:bg-surface-dark p-8 rounded-xl shadow">
        <h1 className="text-2xl font-bold dark:text-yellow-200 text-center mb-4">{t('postRequest')}</h1>
        <input required placeholder={t('title')} className="input" value={form.title}
               onChange={e => set({ ...form, title: e.target.value })} />
        <textarea placeholder={t('description')} className="input h-24" value={form.description}
                  onChange={e => set({ ...form, description: e.target.value })} />
        <button className="btn-primary w-full">{t('submit')}</button>
      </form>
    </section>
  );
}
