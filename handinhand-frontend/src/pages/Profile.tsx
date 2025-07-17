import { useState } from 'react';
import { useQuery, useMutation } from '@tanstack/react-query';
import { me, saveProfile, uploadAvatar } from '../api/user';
import type { Profile as ProfileData } from '../api/user';
import { useTranslation } from 'react-i18next';
// import { profile } from 'console';

/* Profile 可编辑字段 */
type EditableProfile = Partial<ProfileData> & {
  oldPassword?: string;
  newPassword?: string;
};

const API = import.meta.env.VITE_API_BASE || 'http://localhost:5129';
const DEF = '/avatars/defaultAvatar.png';
const toAbsolute = (p: string) => (p.startsWith('/') ? API + p : p);


export default function Profile() {
  const { t } = useTranslation();
  const { data, refetch } = useQuery({ queryKey: ['me'], queryFn: me });
  const [edit, setEdit] = useState(false);
  const [form, setForm] = useState<EditableProfile>({});

  const { mutateAsync } = useMutation({
    mutationFn: (payload: EditableProfile) => saveProfile(payload),
  });

  if (!data) return null;

  /* 上传头像 */
  const pick = async (e: React.ChangeEvent<HTMLInputElement>) => {
    if (!e.target.files?.[0]) return;
    const url = await uploadAvatar(e.target.files[0]);
    setForm(p => ({ ...p, avatarUrl: url }));
  };

  const save = async () => {
    await mutateAsync(form);
    setEdit(false);
    refetch();
  };

  return (
    <section className="main-content flex-1 flex flex-col justify-center responsive-container">
      <div className="flex justify-center py-10">
        {edit ? (
          <div className="card w-full max-w-md p-8 space-y-4">
            <img
              src={toAbsolute((form.avatarUrl?.trim() || data.avatarUrl?.trim() || DEF))}
              onError={e => (e.currentTarget.src = toAbsolute(DEF))}
              className="h-32 w-32 rounded-full object-cover border-4 border-white mx-auto"
            />

            <input type="file" onChange={pick} />

            <input
              className="input"
              placeholder={t('userName')}
              value={form.userName ?? data.userName}
              onChange={e => setForm({ ...form, userName: e.target.value })}
            />
            <input
              className="input"
              placeholder={t('email')}
              value={form.email ?? data.email}
              onChange={e => setForm({ ...form, email: e.target.value })}
            />

            <input
              type="password"
              className="input"
              placeholder={t('oldPassword')}
              value={form.oldPassword ?? ''}
              onChange={e => setForm({ ...form, oldPassword: e.target.value })}
            />
            <input
              type="password"
              className="input"
              placeholder={t('newPassword')}
              value={form.newPassword ?? ''}
              onChange={e => setForm({ ...form, newPassword: e.target.value })}
            />

            <button onClick={save} className="btn-primary w-full">
              {t('save')}
            </button>
          </div>
        ) : (
          <div className="card p-8 flex flex-col items-center gap-4">
            <img
              src={toAbsolute(data.avatarUrl?.trim() || DEF)}
              onError={e => (e.currentTarget.src = toAbsolute(DEF))}
              className="h-24 w-24 rounded-full object-cover"
            />
            <h2 className="text-xl font-bold">{data.userName}</h2>
            <p>{data.email}</p>

            <button
              className="btn-primary"
              onClick={() => {
                setEdit(true);
                setForm({ ...data });
              }}
            >
              {t('edit')}
            </button>
          </div>
        )}
      </div>
    </section>
  );
}
