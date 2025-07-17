import { useQuery } from '@tanstack/react-query';
import { fetchSkills, mySkills, deleteSkill, type Skill } from '../api/skill';
import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

export default function Skills({ my = false }: { my?: boolean }) {
  const { t } = useTranslation();

  /* 列表 */
  const { data: skills = [], refetch } = useQuery({
    queryKey: ['skills', my],
    queryFn: () => (my ? mySkills() : fetchSkills()),
  });

  const remove = async (id: number) => {
    if (confirm(t('confirmDeleteItem'))) {
      await deleteSkill(id);
      refetch();
    }
  };

  return (
    <section className="main-content flex-1 flex flex-col responsive-container">
      <div className="py-8">
        <h1 className="text-3xl font-bold dark:text-yellow-200 text-center mb-8">
          {my ? t('mySkills') : t('skills')}
        </h1>

        <ul className="grid gap-6 grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 justify-items-center">
          {skills.map((s: Skill) => (
            <li
              key={s.id}
              className="border rounded-xl p-6 shadow-sm bg-white dark:bg-surface-dark flex flex-col gap-2 w-full max-w-sm"
            >
              <Link to={`/skills/${s.id}`} className="font-bold text-lg hover:underline">
                {s.title}
              </Link>
              <p className="text-sm text-gray-500 dark:text-gray-300 line-clamp-2">
                {s.description}
              </p>
              <span className="text-xs text-brand-dark dark:text-yellow-200">{s.userName ?? t('anonymous')}</span>

              {my && (
                <button
                  onClick={() => remove(s.id)}
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
