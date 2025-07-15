
import { useEffect, useState } from 'react';
import { fetchHelpRequests, type HelpRequest } from '../api/helpRequest';
import { useTranslation } from 'react-i18next';



export default function Requests() {
  const [reqs, setReqs] = useState<HelpRequest[]>([]);
  const { t } = useTranslation();

  useEffect(() => {
    fetchHelpRequests().then(setReqs);
  }, []);

  return (
    <section className="main-content flex-1 flex flex-col responsive-container py-8">
      <h1 className="text-3xl font-bold dark:text-yellow-200 text-center mb-8">{t('requests')}</h1>
      <ul className="grid gap-6 grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 justify-items-center">
        {reqs.map(r => (
          <li key={r.id} className="border rounded-xl p-6 shadow-sm bg-white dark:bg-surface-dark flex flex-col gap-2 w-full max-w-sm">
            <h2 className="font-bold text-lg mb-1">{r.title}</h2>
            <p className="text-sm text-gray-600 dark:text-gray-300 line-clamp-2 mb-1">
              {r.description}
            </p>
          </li>
        ))}
      </ul>
    </section>
  );
}
