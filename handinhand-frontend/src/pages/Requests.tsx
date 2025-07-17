import { useQuery } from '@tanstack/react-query';
import {
  fetchRequests,
  myRequests,
  deleteRequest,
  type HelpRequest,
} from '../api/helpRequest';
import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

export default function Requests({ my = false }: { my?: boolean }) {
  const { t } = useTranslation();

  const { data: reqs = [], refetch } = useQuery({
    queryKey: ['reqs', my],
    queryFn: () => (my ? myRequests() : fetchRequests()),
  });

  const remove = async (id: number) => {
    if (confirm(t('confirmDeleteItem'))) {
      await deleteRequest(id);
      refetch();
    }
  };

  return (
    <section className="main-content flex-1 flex flex-col responsive-container">
      <div className="py-8">
        <h1 className="text-3xl font-bold dark:text-yellow-200 text-center mb-8">
          {my ? t('myRequests') : t('requests')}
        </h1>

        <ul className="grid gap-6 grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 justify-items-center">
          {reqs.map((r: HelpRequest) => (
            <li
              key={r.id}
              className="border rounded-xl p-6 shadow-sm bg-white dark:bg-surface-dark flex flex-col gap-2 w-full max-w-sm"
            >
              <Link to={`/requests/${r.id}`} className="font-bold text-lg hover:underline">
                {r.title}
              </Link>
              <p className="text-sm text-gray-600 dark:text-gray-300 line-clamp-2">
                {r.description}
              </p>
              <span className="text-xs text-brand-dark dark:text-yellow-200">{r.requesterName ?? t('anonymous')}</span>

              {my && (
                <button
                  onClick={() => remove(r.id)}
                  className="text-sm text-red-500 hover:underline self-end"
                >
                  {t('delete')}
                </button>
              )}
            </li>
          ))}
        </ul>
      </div>
    </section>
  );
}
