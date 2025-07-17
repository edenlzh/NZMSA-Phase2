import { api } from './client';

export interface Comment {
  id: number;
  authorName: string;
  content: string;
  createdAt: string;
  parentId?: number;
}

export const listComments = (skillId?: number, reqId?: number, page = 1) =>
  api.get<Comment[]>('/api/comments', {
    params: { skillId, helpRequestId: reqId, page },
  }).then(r => r.data);

export const postComment = (
  content: string,
  parentId: number | undefined,
  skillId?: number,
  reqId?: number,
) =>
  api.post('/api/comments', { content, parentId }, {
    params: { skillId, helpRequestId: reqId },
  });
