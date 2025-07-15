import { useEffect, useState } from 'react';
import { type Skill, fetchSkills } from '../api/skill';
import { useTranslation } from 'react-i18next';

export default function Skills() {
  const [skills, setSkills] = useState<Skill[]>([]);
  const { t } = useTranslation();

  useEffect(() => {
    fetchSkills().then(setSkills);
  }, []);

  return (
    <section className="main-content flex-1 flex flex-col responsive-container py-8">
      <h1 className="text-3xl font-bold dark:text-yellow-200 text-center mb-8">{t('skills')}</h1>
      <ul className="grid gap-6 grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 justify-items-center">
        {skills.map(s => (
          <li key={s.id} className="border rounded-xl p-6 shadow-sm bg-white dark:bg-surface-dark flex flex-col gap-2 w-full max-w-sm">
            <h2 className="font-bold text-lg mb-1">{s.title}</h2>
            <p className="text-sm text-gray-500 dark:text-gray-300 mb-1">{s.description}</p>
            <span className="text-xs text-brand-dark dark:text-yellow-200">{s.userName}</span>
          </li>
        ))}
      </ul>
    </section>
  );
}
