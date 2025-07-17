import { api } from './client';

export interface Profile {
  userName: string;
  email: string;
  avatarUrl?: string;
}

export const me = () => api.get<Profile>('/api/users/me').then(r => r.data);
export const saveProfile = (p: Partial<Profile> & { oldPassword?: string; newPassword?: string }) =>
  api.put('/api/users/me', p);
export const deleteMe = () => api.delete('/api/users/me');

export const uploadAvatar = (f: File) => {
  const fd = new FormData();
  fd.append('file', f);
  return api.post<string>('/api/users/me/avatar', fd, {
    headers: { 'Content-Type': 'multipart/form-data' },
  }).then(r => r.data);
};
