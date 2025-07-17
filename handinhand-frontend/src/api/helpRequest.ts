import { api } from './client';

export interface HelpRequest {
  id: number;
  title: string;
  description?: string;
  createdAt: string;
  isResolved: boolean;
  requesterName?: string;
}

export const fetchRequests = (userId?: number) =>
  api.get<HelpRequest[]>('/api/helprequests', { params: { userId } }).then(r => r.data);

export const createRequest = (r: { title: string; description?: string }) =>
  api.post<HelpRequest>('/api/helprequests', r).then(res => res.data);

export const myRequests = () => api.get<HelpRequest[]>('/api/helprequests/me').then(r => r.data);
export const updateRequest = (r: HelpRequest) => api.put(`/api/helprequests/${r.id}`, r);
export const deleteRequest = (id: number) => api.delete(`/api/helprequests/${id}`);

/* 详情 */
export const getRequest = (id: number) =>
  api.get<HelpRequest>(`/api/helprequests/${id}`).then(r => r.data);
