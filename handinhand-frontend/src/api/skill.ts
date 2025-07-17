import { api } from './client';

export interface Skill {
  id: number;
  title: string;
  description?: string;
  category?: string;
  userId: number;
  userName?: string;
}

export const fetchSkills = (userId?: number) =>
  api.get<Skill[]>('/api/skills', { params: { userId } }).then(r => r.data);

export const createSkill = (s: { title: string; description?: string; category?: string }) =>
  api.post<Skill>('/api/skills', s).then(r => r.data);

export const mySkills = () => api.get<Skill[]>('/api/skills/me').then(r => r.data);
export const updateSkill = (s: Skill) => api.put(`/api/skills/${s.id}`, s);
export const deleteSkill = (id: number) => api.delete(`/api/skills/${id}`);

/* 详情 */
export const getSkill = (id: number) =>
  api.get<Skill>(`/api/skills/${id}`).then(r => r.data);
