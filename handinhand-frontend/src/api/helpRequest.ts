import { api } from './client';

export interface HelpRequest {
  id: number;
  title: string;
  description?: string;
  createdAt: string;
  isResolved: boolean;
  requesterName?: string;
}

export const fetchHelpRequests = () =>
  api.get<HelpRequest[]>('/api/helprequests').then(r => r.data);

export const createHelpRequest = (h: { title: string; description?: string }) =>
  api.post<HelpRequest>('/api/helprequests', h).then(r=>r.data);
