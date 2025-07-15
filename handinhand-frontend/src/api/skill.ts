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

export const createSkill = (s: {title:string; description?:string; category?:string}) =>
  api.post<Skill>('/api/skills', s).then(r=>r.data);
