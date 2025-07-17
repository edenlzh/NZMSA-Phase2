import { listComments, postComment } from '../api/comment';
import { useQuery } from '@tanstack/react-query';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';

export default function CommentSection(
  { skillId, reqId }: { skillId?: number; reqId?: number },
) {
  const { t } = useTranslation();
  const [page, setPage] = useState(1);
  const [text, setText] = useState('');

  const { data, refetch, isLoading } = useQuery({
    queryKey: ['comments', skillId, reqId, page],
    queryFn: () => listComments(skillId, reqId, page),
  });

  const send = async () => {
    if (!text.trim()) return;
    await postComment(text, undefined, skillId, reqId);
    setText('');
    refetch();
  };

  return (
    <div className="space-y-4">
      {/* 列表 */}
      {data?.map(c => (
        <div key={c.id}
             className="border rounded p-3 bg-surface-card dark:bg-surface-card-dark">
          <b>{c.authorName}</b> · {new Date(c.createdAt).toLocaleString()}
          <p>{c.content}</p>
        </div>
      ))}

      {/* 无评论占位 */}
      {!isLoading && data?.length === 0 && (
        <p className="text-sm text-gray-400">{t('noComments')}</p>
      )}

      {/* 留言输入 */}
      <div className="flex gap-2">
        <input className="input flex-1"
               value={text}
               onChange={e => setText(e.target.value)}
               placeholder={t('leaveComment')} />
        <button className="btn-primary" onClick={send}>
          {t('send')}
        </button>
      </div>

      {/* 分页 */}
      <div className="flex justify-between">
        <button disabled={page === 1}
                onClick={() => setPage(p => p - 1)}
                className="btn-primary">
          {t('prev')}
        </button>
        <button disabled={data?.length! < 6}
                onClick={() => setPage(p => p + 1)}
                className="btn-primary">
          {t('next')}
        </button>
      </div>
    </div>
  );
}
