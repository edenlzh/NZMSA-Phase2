import { useParams } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import { getRequest } from '../api/helpRequest';
import CommentSection from '../components/CommentSection';
import { useTranslation } from 'react-i18next';

export default function RequestDetail() {
  const { t } = useTranslation();
  const { id } = useParams();
  const { data: req } = useQuery({
    queryKey: ['req', id],
    queryFn: () => getRequest(Number(id)),
  });

  if (!req) return null;

  return (
    <section className="main-content flex-1 flex flex-col responsive-container">
      <div className="py-10 space-y-6">
        <h1 className="text-2xl font-bold dark:text-yellow-200">{req.title}</h1>
        <p>{req.description}</p>

        <hr />

        <h2 className="text-xl font-semibold dark:text-yellow-200">{t('comments')}</h2>
        <CommentSection reqId={req.id} />
      </div>
    </section>
  );
}
