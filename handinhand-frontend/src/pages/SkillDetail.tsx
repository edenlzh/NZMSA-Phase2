import { useParams } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import { getSkill } from '../api/skill';
import CommentSection from '../components/CommentSection';
import { useTranslation } from 'react-i18next';

export default function SkillDetail() {
  const { t } = useTranslation();
  const { id } = useParams();
  const { data: skill } = useQuery({
    queryKey: ['skill', id],
    queryFn: () => getSkill(Number(id)),
  });

  if (!skill) return null;

  return (
    <section className="main-content flex-1 flex flex-col responsive-container">
      <div className="py-10 space-y-6">
        <h1 className="text-2xl font-bold dark:text-yellow-200">{skill.title}</h1>
        <p>{skill.description}</p>

        <hr />

        <h2 className="mt-6 text-xl font-semibold dark:text-yellow-200">{t('comments')}</h2>
        <CommentSection skillId={skill.id} />
      </div>
    </section>
  );
}
